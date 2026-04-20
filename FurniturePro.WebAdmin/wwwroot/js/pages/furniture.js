// wwwroot/js/furniture.js

// --- GLOBAL VARIABLES ---
let cached_furniture = [];
let cached_categories = [];
let cached_compositions = [];
let cached_parts = [];
let cached_prices = [];
let furniture = []; // Отфильтрованный список для отображения

// Состояние редактора состава
let currentCompositionDraft = []; // { partId, count, name, price }
let currentEditingFurnitureId = null;

let currentPage = 1;
const perPage = 20;
let sortField = null;
let sortDirection = 'asc';

// --- DOM ELEMENTS: MAIN ---
const searchInput = document.getElementById('searchInput');
const searchButton = document.getElementById('searchButton');
const clearSearchTextBtn = document.getElementById('clearSearchText');
const activityFilter = document.getElementById('activityFilter');
const categoryFilter = document.getElementById('categoryFilter');
const createFurnitureButton = document.getElementById('createFurnitureButton');
const furnitureCount = document.getElementById('furnitureCount');
const pageInfo = document.getElementById('pageInfo');
const prevBtn = document.getElementById('prev');
const nextBtn = document.getElementById('next');

// --- DOM ELEMENTS: MODALS ---
const createModal = document.getElementById('createModal');
const editModal = document.getElementById('editModal');
const deleteModal = document.getElementById('deleteModal');
const descriptionModal = document.getElementById('descriptionModal');
const compositionModal = document.getElementById('compositionModal'); // Просмотр (Read-only)
const editCompositionModal = document.getElementById('editCompositionModal'); // Редактирование (New)
const errorModal = document.getElementById('errorModal');

// --- DOM ELEMENTS: FORMS ---
const createForm = document.getElementById('createForm');
const editForm = document.getElementById('editForm');
const deleteForm = document.getElementById('deleteForm');

// --- DOM ELEMENTS: COMPOSITION EDITOR ---
const partSearchInput = document.getElementById('partSearchInput');
const clearPartSearchBtn = document.getElementById('clearPartSearch'); // NEW
const saveCompositionBtn = document.getElementById('saveCompositionBtn');
const pModeFilter = document.getElementById('pModeFilter');
const pModeHighlight = document.getElementById('pModeHighlight');
const currentCompTableBody = document.querySelector('#currentCompTable tbody');
const availablePartsTableBody = document.querySelector('#availablePartsTable tbody');

// Элементы итогов
const headerTotalCountLabel = document.getElementById('headerTotalCount');
const summaryPriceLabel = document.getElementById('summaryTotalPrice');

// --- SPECIFIC INPUTS ---
const createCategorySelect = document.getElementById('createCategory');
const editCategorySelect = document.getElementById('editCategory');
const editIdInput = document.getElementById('editId');
const deleteIdInput = document.getElementById('deleteId');
const descriptionContent = document.getElementById('descriptionContent');
const compositionContent = document.getElementById('compositionContent');

// --- TABLE ---
const table = document.getElementById('furnitureTable');
const headers = table.querySelectorAll('thead th');

// ==========================================
// 1. INITIALIZATION & DATA FETCHING
// ==========================================

headers.forEach((th, index) => {
    // Пропускаем заголовки без сортировки (Описание, Состав, Действия)
    if (index < 6) {
        th.style.cursor = 'pointer';
        th.addEventListener('click', () => onHeaderClick(index, th));
    }
});

document.addEventListener('DOMContentLoaded', async () => {
    try {
        toggleLoader(true);

        initSearchModeSwitcher(() => renderTable());
        attachRowSelection('#furnitureTable tbody');

        await initFurniture();

        initFiltersFromData();
        initCustomSelects();

        renderTable();
    } catch (e) {
        console.error("Init error:", e);
    } finally {
        toggleLoader(false);
    }
    // startAutoSync(); // Раскомментировать для автообновления
});

async function initFurniture() {
    await Promise.all([
        syncTable('Furniture'), syncTable('Categories'),
        syncTable('FurnitureCompositions'), syncTable('Parts'),
        syncTable('Prices')
    ]);

    const [furnitureData, categories, compositions, parts, prices] = await Promise.all([
        getAll('Furniture'), getAll('Categories'),
        getAll('FurnitureCompositions'), getAll('Parts'),
        getAll('Prices')
    ]);

    cached_furniture = furnitureData || [];
    cached_categories = categories || [];
    cached_compositions = compositions || [];
    cached_parts = parts || [];
    cached_prices = prices || [];
}

function initFiltersFromData() {
    fillSelect(categoryFilter, 'Категория: все записи', cached_categories);
    fillSelect(createCategorySelect, 'Выберите категорию', cached_categories);
    fillSelect(editCategorySelect, 'Выберите категорию', cached_categories);

    if (activityFilter.options.length === 1) {
        activityFilter.innerHTML = '';
        const allOpt = document.createElement('option'); allOpt.value = ''; allOpt.textContent = 'Активность: все записи';
        const activeOpt = document.createElement('option'); activeOpt.value = 'active'; activeOpt.textContent = 'Только активные';
        const inactiveOpt = document.createElement('option'); inactiveOpt.value = 'inactive'; inactiveOpt.textContent = 'Только неактивные';
        activityFilter.append(allOpt, activeOpt, inactiveOpt);
    }
}

// ==========================================
// 2. SORTING LOGIC
// ==========================================

function onHeaderClick(index, th) {
    // Indexes: 0:Name, 1:Category, 2:SelfPrice, 3:Markup, 4:TotalPrice, 5:Activity
    const fieldMap = ['name', 'category', 'selfPrice', 'markup', 'totalPrice', 'activity'];
    const field = fieldMap[index];
    if (!field) return;

    if (sortField === field) {
        sortDirection = sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
        sortField = field;
        sortDirection = 'asc'; // Price usually sorts ASC first (cheapest first)
        if (field === 'price' || field === 'selfPrice') sortDirection = 'asc';
    }

    updateHeaderSortUI(headers, index, sortDirection);
    // Reset page to 1 when sorting changes
    currentPage = 1;
    renderTable();
}

// ==========================================
// 3. MAIN TABLE RENDERING
// ==========================================

function renderTable() {
    const tbody = document.querySelector('#furnitureTable tbody');
    if (!tbody) return;

    const search = (searchInput.value || '').toLowerCase().trim();
    const activity = activityFilter.value;
    const category = categoryFilter.value;

    tbody.innerHTML = '';

    // --- PRE-CALCULATION PHASE (Performance Fix) ---
    // Create lookup maps once
    const categoryById = new Map(cached_categories.map(c => [c.id, c]));
    const partsById = new Map(cached_parts.map(p => [p.id, p]));

    // Create Price Map (Latest price per part)
    const priceMap = new Map();
    const sortedPrices = [...cached_prices].sort((a, b) => new Date(b.date) - new Date(a.date));
    sortedPrices.forEach(p => { if (!priceMap.has(p.partId)) priceMap.set(p.partId, p.value); });

    // Group Compositions
    const compositionsByFurnitureId = new Map();
    cached_compositions.forEach(comp => {
        const fId = comp.idFurniture ?? comp.entity1Id;
        if (!compositionsByFurnitureId.has(fId)) compositionsByFurnitureId.set(fId, []);
        compositionsByFurnitureId.get(fId).push(comp);
    });

    // Create a "View Model" list with calculated prices
    // This allows us to filter and sort efficiently
    let viewData = cached_furniture.map(item => {
        const comps = compositionsByFurnitureId.get(item.id) || [];

        let selfPrice = 0;
        comps.forEach(comp => {
            const pId = comp.idPart ?? comp.entity2Id;
            const count = comp.count ?? 0;
            const currentPrice = priceMap.get(pId) || 0;
            selfPrice += currentPrice * count;
        });

        const markup = item.markup ?? 0;
        const totalPrice = selfPrice + (selfPrice * markup / 100);

        return {
            ...item, // Original data
            _categoryName: categoryById.get(item.categoryId)?.name || '',
            _selfPrice: selfPrice,
            _totalPrice: totalPrice,
            _compositions: comps // Pass compositions to render loop
        };
    });

    // --- FILTERING ---
    let finded_items = viewData.filter(f => {
        if (search) {
            const haystack = [f.name, f.description, (f.markup ?? '').toString()].map(v => (v ?? '').toString().toLowerCase()).join(' ');
            if (!haystack.includes(search)) return false;
        }
        if (activity === 'active' && !f.activity) return false;
        if (activity === 'inactive' && f.activity) return false;
        const categoryId = category ? Number(category) : null;
        if (categoryId !== null && f.categoryId !== categoryId) return false;
        return true;
    });

    // Handle Search Mode (Filter vs Highlight)
    const mode = getSearchMode();
    let itemsToRender = [];
    let isHighlight = false;

    if (mode === 'filter') {
        itemsToRender = finded_items;
    } else if (mode === 'highlight') {
        // Base list for highlight mode is everything (filtered by selects only)
        itemsToRender = viewData.filter(f => {
            if (activity === 'active' && !f.activity) return false;
            if (activity === 'inactive' && f.activity) return false;
            const categoryId = category ? Number(category) : null;
            if (categoryId !== null && f.categoryId !== categoryId) return false;
            return true;
        });
        isHighlight = true;
    }

    // --- COUNTER ---
    if (furnitureCount) {
        const total = cached_furniture.length;
        const filtered = finded_items.length;
        furnitureCount.textContent = mode === 'filter'
            ? `Записей: ${filtered} из ${total}`
            : `Записей: ${itemsToRender.length} (совпадений: ${filtered}) из ${total}`;
    }

    // --- SORTING (Fast now because properties are pre-calculated) ---
    if (itemsToRender.length > 0 && sortField) {
        itemsToRender.sort((a, b) => {
            const dir = sortDirection === 'asc' ? 1 : -1;
            let valA, valB;

            switch (sortField) {
                case 'name': valA = a.name.toLowerCase(); valB = b.name.toLowerCase(); break;
                case 'category': valA = a._categoryName.toLowerCase(); valB = b._categoryName.toLowerCase(); break;
                case 'selfPrice': valA = a._selfPrice; valB = b._selfPrice; break;
                case 'markup': valA = a.markup; valB = b.markup; break;
                case 'totalPrice': valA = a._totalPrice; valB = b._totalPrice; break;
                case 'activity': valA = a.activity ? 1 : 0; valB = b.activity ? 1 : 0; break;
                default: return 0;
            }

            return valA < valB ? -1 * dir : (valA > valB ? 1 * dir : 0);
        });
    }

    furniture = itemsToRender; // Update global for pagination

    // --- PAGINATION ---
    currentPage = updatePaginationUI(currentPage, furniture.length, perPage, pageInfo, prevBtn, nextBtn);

    if (furniture.length === 0) {
        tbody.innerHTML = '<tr><td colspan="9" class="text-center" style="color:var(--text-muted);font-style:italic; padding: 2rem;">Записей нет</td></tr>';
        return;
    }

    const pageItems = paginateData(furniture, currentPage, perPage);
    const fragment = document.createDocumentFragment();
    const currencyFmt = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' });

    // --- RENDERING LOOP ---
    for (const item of pageItems) {
        const tr = document.createElement('tr');
        // Check highlight based on original ID match in filtered list
        if (isHighlight && finded_items.find(x => x.id === item.id)) tr.classList.add('highlight-match');

        const tdName = document.createElement('td'); tdName.textContent = item.name;

        const tdCategory = document.createElement('td');
        tdCategory.textContent = item._categoryName || '—';

        const tdSelfPrice = document.createElement('td');
        tdSelfPrice.textContent = currencyFmt.format(item._selfPrice);

        const tdMarkup = document.createElement('td');
        tdMarkup.className = 'fw-bold';
        tdMarkup.textContent = (item.markup ?? 0) + '%';

        const tdPrice = document.createElement('td');
        tdPrice.className = 'fw-bold';
        tdPrice.textContent = currencyFmt.format(item._totalPrice);

        const tdActivity = document.createElement('td');
        tdActivity.textContent = item.activity ? 'Активна' : 'Нет';

        // Description
        const tdDescription = document.createElement('td');
        tdDescription.classList.add('col-fit');
        const descBtn = document.createElement('button');
        descBtn.textContent = '📄'; descBtn.title = 'Описание';
        descBtn.className = 'btn btn-sm btn-description btn-square';
        descBtn.addEventListener('click', () => {
            descriptionContent.textContent = item.description || 'Описания нет';
            showModal(descriptionModal);
        });
        tdDescription.appendChild(descBtn);

        // Composition
        const tdComposition = document.createElement('td');
        tdComposition.classList.add('col-fit');
        const comps = item._compositions; // Used pre-grouped

        if (comps.length > 0) {
            const compBtn = document.createElement('button');
            compBtn.innerHTML = `<span>🧩</span>`;
            compBtn.title = `Просмотр состава (${comps.length} поз.)`;
            compBtn.className = 'btn btn-sm btn-composition btn-square';
            compBtn.addEventListener('click', () => {
                renderReadOnlyComposition(item.name, comps, partsById);
                showModal(compositionModal);
            });
            tdComposition.appendChild(compBtn);
        } else {
            tdComposition.innerHTML = '<span style="color: var(--warm-border); opacity: 0.6;">—</span>';
        }

        // Actions
        const tdActions = document.createElement('td');
        tdActions.classList.add('col-fit');
        const actionsDiv = document.createElement('div');
        actionsDiv.className = 'actions-group justify-content-end';

        const configBtn = document.createElement('button');
        configBtn.textContent = '⚙'; configBtn.title = 'Редактировать состав';
        configBtn.className = 'btn btn-sm btn-outline-secondary btn-square';
        configBtn.addEventListener('click', () => openEditCompositionModal(item));

        const editBtn = document.createElement('button');
        editBtn.textContent = '✎'; editBtn.title = 'Изменить';
        editBtn.className = 'btn btn-sm btn-primary btn-square';
        editBtn.addEventListener('click', () => openEditModal(item));

        const deleteBtn = document.createElement('button');
        deleteBtn.textContent = '🗑'; deleteBtn.title = 'Удалить';
        deleteBtn.className = 'btn btn-sm btn-danger btn-square';
        deleteBtn.addEventListener('click', () => openDeleteModal(item));

        actionsDiv.append(configBtn, editBtn, deleteBtn);
        tdActions.appendChild(actionsDiv);

        tr.append(tdName, tdCategory, tdSelfPrice, tdMarkup, tdPrice, tdActivity, tdDescription, tdComposition, tdActions);
        fragment.appendChild(tr);
    }
    tbody.appendChild(fragment);
}

// ==========================================
// 4. READ-ONLY COMPOSITION MODAL (FIXED & STYLED)
// ==========================================

function renderReadOnlyComposition(furnitureName, compositions, partsMap) {
    const container = document.getElementById('compositionContent');
    if (!container) return;

    // Сброс стилей контейнера
    container.className = '';
    container.innerHTML = '';
    container.style.padding = '0';
    container.style.border = 'none';
    container.style.background = 'transparent';
    container.style.maxHeight = 'none';
    container.style.overflow = 'visible';

    // Обработка пустого состава
    if (!compositions || compositions.length === 0) {
        container.innerHTML = `<div class="d-flex flex-column align-items-center justify-content-center p-4" style="color: var(--app-text);">
            <span class="fs-4 mb-2">∅</span>
            <span>Состав для "<b>${furnitureName}</b>" не указан.</span>
        </div>`;
        return;
    }

    // --- 1. ЗАГОЛОВОК (СТИЛИЗОВАННЫЙ) ---
    const header = document.createElement('div');
    header.className = 'd-flex justify-content-between align-items-center mb-2 px-1';

    // Убран text-muted. "ИЗДЕЛИЕ" цвета accent-primary (темный тил) или text-muted (коричневый), но без прозрачности.
    // Сделаем "ИЗДЕЛИЕ" коричневым (theme text-muted color but opacity 1), а имя - черным.
    header.innerHTML = `
        <div class="d-flex flex-column">
            <span class="small text-uppercase fw-bold" style="color: var(--text-muted); letter-spacing: 0.05em; font-size: 0.75rem;">Изделие:</span>
            <span class="fw-bold" style="color: var(--app-text); font-size: 1.1rem; line-height: 1.2;">${furnitureName}</span>
        </div>
        <span class="badge badge-theme" style="font-size: 0.85rem;">${compositions.length} позиций</span>`;

    container.appendChild(header);

    // Обертка таблицы
    const wrapper = document.createElement('div');
    wrapper.className = 'composition-table-wrapper';
    wrapper.style.height = '400px';
    wrapper.style.marginBottom = '0';

    // Скролл
    const scroll = document.createElement('div');
    scroll.className = 'composition-table-scroll';

    // Таблица
    const table = document.createElement('table');
    table.className = 'table table-sm table-hover align-middle mb-0 admin-table';

    // Шапка
    const thead = document.createElement('thead');
    thead.className = 'table-light';
    thead.innerHTML = `
        <tr>
            <th>Деталь</th>
            <th class="text-center" style="width: 100px;">Кол-во</th>
        </tr>`;
    table.appendChild(thead);

    // Тело
    const tbody = document.createElement('tbody');
    compositions.forEach(comp => {
        const pId = comp.idPart ?? comp.entity2Id;
        const part = partsMap.get(pId);
        const name = part ? part.name : `<span class="text-danger fst-italic">Деталь удалена (ID: ${pId})</span>`;

        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td>${name}</td>
            <td class="text-center fw-bold fs-6" style="color: var(--app-text);">${comp.count}</td>
        `;
        tbody.appendChild(tr);
    });

    table.appendChild(tbody);
    scroll.appendChild(table);
    wrapper.appendChild(scroll);
    container.appendChild(wrapper);

    // --- 2. ФУТЕР (СТИЛИЗОВАННЫЙ) ---
    const totalItems = compositions.reduce((sum, c) => sum + c.count, 0);
    const footer = document.createElement('div');
    // Убран text-muted, добавлен цвет текста приложения
    footer.className = 'd-flex justify-content-end align-items-center mt-2 px-1 small';
    footer.style.color = 'var(--app-text)';

    footer.innerHTML = `
        <span style="font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; margin-right: 5px; color: var(--text-muted);">Всего деталей:</span> 
        <b class="fs-6">${totalItems}</b>`;

    container.appendChild(footer);
}

// ==========================================
// 5. COMPOSITION EDITOR (INTERACTIVE)
// ==========================================

function attachInnerRowSelection(tbody) {
    tbody.querySelectorAll('tr').forEach(tr => {
        tr.addEventListener('click', (e) => {
            // Игнорируем клики по интерактивным элементам
            if (['INPUT', 'BUTTON', 'DIV'].includes(e.target.tagName)) return;

            tbody.querySelectorAll('tr.selected-row').forEach(r => r.classList.remove('selected-row'));
            tr.classList.add('selected-row');
        });
    });
}

function getLatestPrice(partId) {
    const prices = cached_prices.filter(p => p.partId === partId);
    if (!prices.length) return 0;
    prices.sort((a, b) => new Date(b.date) - new Date(a.date));
    return prices[0].value;
}

// --- OPEN EDITOR ---
function openEditCompositionModal(item) {
    currentEditingFurnitureId = item.id;

    const existingComps = cached_compositions.filter(c =>
        (c.idFurniture === item.id) || (c.entity1Id === item.id)
    );

    const partsMap = new Map(cached_parts.map(p => [p.id, p]));
    currentCompositionDraft = existingComps.map(c => {
        const pId = c.idPart ?? c.entity2Id;
        const part = partsMap.get(pId);
        return {
            partId: pId,
            count: c.count,
            name: part ? part.name : 'Неизвестная деталь',
            price: getLatestPrice(pId)
        };
    });

    // Сброс UI поиска
    partSearchInput.value = '';
    if (pModeFilter) pModeFilter.checked = true;

    renderDraftTable();
    renderAvailablePartsTable();
    showModal(editCompositionModal);
}

// --- LEFT SIDE: DRAFT TABLE ---
function renderDraftTable() {
    currentCompTableBody.innerHTML = '';

    let totalCount = 0;
    let totalSumMoney = 0;

    currentCompositionDraft.forEach((item, index) => {
        totalCount += item.count;
        const itemTotalCost = item.price * item.count;
        totalSumMoney += itemTotalCost;

        const tr = document.createElement('tr');

        // Name
        const tdName = document.createElement('td');
        tdName.textContent = item.name;

        // Qty Input
        const tdQty = document.createElement('td');
        tdQty.className = 'text-center';
        const input = document.createElement('input');
        input.type = 'number';
        input.value = item.count;
        input.min = 1;
        input.className = 'qty-input';

        input.onkeydown = (e) => {
            if (e.key === '-' || e.key === 'Subtract' || e.key === 'e') e.preventDefault();
        };

        input.addEventListener('change', (e) => {
            let val = parseInt(e.target.value);
            if (val < 1 || isNaN(val)) { val = 1; e.target.value = 1; }
            item.count = val;
            renderDraftTable(); // Пересчет
        });
        tdQty.appendChild(input);

        // Sum
        const tdSum = document.createElement('td');
        tdSum.className = 'text-end small fw-bold';
        tdSum.textContent = Math.round(itemTotalCost).toLocaleString('ru-RU') + ' ₽';

        // Delete
        const tdAct = document.createElement('td');
        tdAct.className = 'col-fit';
        const delBtn = document.createElement('button');
        delBtn.textContent = '×';
        delBtn.className = 'btn btn-sm btn-danger btn-square';
        delBtn.title = 'Убрать из состава';
        delBtn.onclick = (e) => {
            e.stopPropagation();
            currentCompositionDraft.splice(index, 1);
            renderDraftTable();
            renderAvailablePartsTable();
        };
        tdAct.appendChild(delBtn);

        tr.append(tdName, tdQty, tdSum, tdAct);
        currentCompTableBody.appendChild(tr);
    });

    // UPDATE UI TOTALS
    if (headerTotalCountLabel) headerTotalCountLabel.textContent = totalCount + ' шт.';
    if (summaryPriceLabel) {
        summaryPriceLabel.textContent = totalSumMoney.toLocaleString('ru-RU', {
            style: 'currency', currency: 'RUB'
        });
    }

    attachInnerRowSelection(currentCompTableBody);
}

// --- RIGHT SIDE: AVAILABLE PARTS ---
function renderAvailablePartsTable() {
    availablePartsTableBody.innerHTML = '';

    const searchText = (partSearchInput.value || '').toLowerCase().trim();
    const mode = document.querySelector('input[name="partSearchMode"]:checked')?.value || 'filter';

    const addedIds = new Set(currentCompositionDraft.map(x => x.partId));
    let partsToRender = cached_parts;

    if (mode === 'filter' && searchText) {
        partsToRender = cached_parts.filter(p => p.name.toLowerCase().includes(searchText));
    }

    const fragment = document.createDocumentFragment();

    partsToRender.forEach(part => {
        const isMatch = searchText && part.name.toLowerCase().includes(searchText);
        const isAdded = addedIds.has(part.id);

        const tr = document.createElement('tr');

        // Подсветка поиска
        if (mode === 'highlight' && isMatch) {
            tr.classList.add('highlight-match');
        }

        // Визуализация уже добавленных (светло-серый фон)
        if (isAdded) {
            tr.style.backgroundColor = 'rgba(237, 217, 183, 0.3)';
        }

        // Name
        const tdName = document.createElement('td');
        tdName.textContent = part.name;

        // Price
        const tdPrice = document.createElement('td');
        tdPrice.className = 'text-end small';
        const price = getLatestPrice(part.id);
        tdPrice.textContent = price.toLocaleString('ru-RU', { style: 'currency', currency: 'RUB' });

        // Action
        const tdAction = document.createElement('td');
        tdAction.className = 'col-fit text-center';

        if (isAdded) {
            // Галочка
            const check = document.createElement('div');
            check.className = 'icon-added-check';
            check.textContent = '✓';
            check.title = 'Уже в составе';
            tdAction.appendChild(check);
        } else {
            // Кнопка (+)
            const addBtn = document.createElement('button');
            addBtn.textContent = '+';
            addBtn.className = 'btn btn-sm btn-success btn-square';
            addBtn.onclick = (e) => {
                e.stopPropagation();
                currentCompositionDraft.push({
                    partId: part.id,
                    count: 1,
                    name: part.name,
                    price: price
                });
                renderDraftTable();
                renderAvailablePartsTable();
            };
            tdAction.appendChild(addBtn);
        }

        tr.append(tdName, tdPrice, tdAction);
        fragment.appendChild(tr);
    });

    availablePartsTableBody.appendChild(fragment);
    attachInnerRowSelection(availablePartsTableBody);
}

// --- LISTENERS ---
partSearchInput.addEventListener('input', renderAvailablePartsTable);
if (pModeFilter) pModeFilter.addEventListener('change', renderAvailablePartsTable);
if (pModeHighlight) pModeHighlight.addEventListener('change', renderAvailablePartsTable);

// Кнопка очистки поиска
if (clearPartSearchBtn) {
    clearPartSearchBtn.addEventListener('click', () => {
        partSearchInput.value = '';
        partSearchInput.focus();
        renderAvailablePartsTable();
    });
}

// --- SAVE ACTION ---
saveCompositionBtn.addEventListener('click', async () => {
    if (!currentEditingFurnitureId) return;

    try {
        toggleLoader(true);

        const originalComps = cached_compositions.filter(c =>
            (c.idFurniture === currentEditingFurnitureId) || (c.entity1Id === currentEditingFurnitureId)
        );
        const newComps = currentCompositionDraft;

        const toCreate = [];
        const toUpdate = [];
        const toDelete = [];

        // А. Удаление
        originalComps.forEach(old => {
            const oldPartId = old.idPart ?? old.entity2Id;
            const exists = newComps.find(n => n.partId === oldPartId);
            if (!exists) {
                // Используем имена свойств как в классе SimpleKey (C#)
                toDelete.push({
                    IdFurniture: Number(currentEditingFurnitureId),
                    IdPart: Number(oldPartId)
                });
            }
        });

        // Б. Создание и Обновление
        newComps.forEach(newItem => {
            const oldItem = originalComps.find(o => (o.idPart ?? o.entity2Id) === newItem.partId);

            // Используем имена свойств как в FurnitureCompositionDTO
            // ОБЯЗАТЕЛЬНО добавляем UpdateDate, так как в DTO оно required
            const dto = {
                IdFurniture: Number(currentEditingFurnitureId),
                IdPart: Number(newItem.partId),
                Count: Number(newItem.count),
                UpdateDate: new Date().toISOString()
            };

            if (!oldItem) {
                toCreate.push(dto);
            } else if (oldItem.count !== newItem.count) {
                toUpdate.push(dto);
            }
        });

        // Параметры запроса
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
        const headers = {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        };

        // --- 1. CREATE ---
        if (toCreate.length > 0) {
            console.log('Sending Create:', toCreate);
            const res = await fetch('?handler=CreateCompositionRange', {
                method: 'POST', headers: headers, body: JSON.stringify(toCreate)
            });
            const data = await res.json();
            if (!data.success) throw new Error(data.message || 'Ошибка создания');
        }

        // --- 2. UPDATE ---
        if (toUpdate.length > 0) {
            console.log('Sending Update:', toUpdate);
            const res = await fetch('?handler=UpdateCompositionRange', {
                method: 'POST', headers: headers, body: JSON.stringify(toUpdate)
            });
            const data = await res.json();
            if (!data.success) throw new Error(data.message || 'Ошибка обновления');
        }

        // --- 3. DELETE ---
        if (toDelete.length > 0) {
            console.log('Sending Delete:', toDelete);
            const res = await fetch('?handler=DeleteCompositionRange', {
                method: 'POST', headers: headers, body: JSON.stringify(toDelete)
            });
            const data = await res.json();
            if (!data.success) throw new Error(data.message || 'Ошибка удаления');
            else {
                clearStore('FurnitureCompositions');

            }
        }

        // Успех
        hideModal(editCompositionModal);
        await initFurniture();
        renderTable();

    } catch (e) {
        console.error(e);
        alert(e.message || 'Ошибка сохранения');
    } finally {
        toggleLoader(false);
    }
});

// ==========================================
// 6. CRUD MODALS (CREATE, EDIT, DELETE)
// ==========================================

createFurnitureButton.addEventListener('click', () => {
    if (createForm) {
        initCustomSelects();
        createForm.reset();
        document.getElementById('createActivity').value = 'true';
    }
    showModal(createModal);
});

function openEditModal(item) {
    editIdInput.value = item.id;
    document.getElementById('editName').value = item.name || '';
    document.getElementById('editMarkup').value = item.markup ?? 0;
    document.getElementById('editDescription').value = item.description || '';
    document.getElementById('editIsActive').checked = item.activity;

    if (editCategorySelect) {
        editCategorySelect.value = item.categoryId ?? '';
        editCategorySelect.dispatchEvent(new Event('change'));
    }
    initCustomSelects();
    showModal(editModal);
}

function openDeleteModal(item) {
    deleteIdInput.value = item.id;
    showModal(deleteModal);
}

function validateFurnitureForm(prefix) {
    const errors = [];
    const name = document.getElementById(prefix + 'Name');
    const category = document.getElementById(prefix + 'Category');
    const markup = document.getElementById(prefix + 'Markup');

    if (!name.value.trim()) errors.push('Поле "Название" обязательно.');
    if (!category.value) errors.push('Поле "Категория" обязательно.');
    if (markup.value === '') errors.push('Поле "Наценка" обязательно.');
    else if (Number(markup.value) < 0) errors.push('Поле "Наценка" должно быть больше или равно 0.');

    if (errors.length > 0) {
        const errorList = document.getElementById('errorList');
        errorList.innerHTML = '';
        errors.forEach(msg => {
            const li = document.createElement('li'); li.textContent = msg; errorList.appendChild(li);
        });
        showModal(errorModal);
        return false;
    }
    return true;
}

async function handleFormSubmit(e, handlerName, modalToClose) {
    e.preventDefault();
    const formPrefix = e.target.id.replace('Form', '');
    if (formPrefix !== 'delete' && !validateFurnitureForm(formPrefix)) return;

    try {
        toggleLoader(true);
        const formData = new FormData(e.target);
        if (formPrefix === 'edit') formData.set('Activity', document.getElementById('editIsActive').checked);

        const response = await fetch(`?handler=${handlerName}`, { method: 'POST', body: formData });
        let result = {};
        try { result = await response.json(); } catch { result = { success: response.ok }; }

        if (!response.ok || (result.hasOwnProperty('success') && !result.success)) {
            const errorList = document.getElementById('errorList');
            errorList.innerHTML = `<li>${result.message || 'Ошибка операции'}</li>`;
            showModal(errorModal);
        } else {
            hideModal(modalToClose);
            await initFurniture();
            renderTable();
        }
    } catch (err) { console.error(err); } finally { toggleLoader(false); }
}

createForm.addEventListener('submit', (e) => handleFormSubmit(e, 'CreateFurniture', createModal));
editForm.addEventListener('submit', (e) => handleFormSubmit(e, 'UpdateFurniture', editModal));
deleteForm.addEventListener('submit', (e) => handleFormSubmit(e, 'DeleteFurniture', deleteModal));

// ==========================================
// 7. COMMON LISTENERS (PAGINATION, SEARCH)
// ==========================================

prevBtn.addEventListener('click', () => { if (currentPage > 1) { currentPage--; renderTable(); } });
nextBtn.addEventListener('click', () => {
    const totalPages = Math.ceil(furniture.length / perPage);
    if (currentPage < totalPages) { currentPage++; renderTable(); }
});

searchButton.addEventListener('click', () => { currentPage = 1; renderTable(); });
searchInput.addEventListener('keyup', (e) => { if (e.key === 'Enter') { currentPage = 1; renderTable(); } });

if (clearSearchTextBtn) {
    clearSearchTextBtn.addEventListener('click', () => {
        searchInput.value = '';
        currentPage = 1;
        renderTable();
    });
}

activityFilter.addEventListener('change', () => { currentPage = 1; renderTable(); });
categoryFilter.addEventListener('change', () => { currentPage = 1; renderTable(); });