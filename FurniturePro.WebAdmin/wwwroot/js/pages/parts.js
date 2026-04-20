// wwwroot/js/pages/parts.js

let cached_parts = [];
let cached_materials = [];
let cached_colors = [];
let cached_prices = [];
let parts = [];

// Переменные для аналитики
let analyticsChart = null;
let currentAnalyticsPartId = null;
let currentAnalyticsYear = new Date().getFullYear();

let currentPage = 1;
const perPage = 20;
let sortField = null;
let sortDirection = 'asc';

// --- UI Elements ---
const searchInput = document.getElementById('searchInput');
const searchButton = document.getElementById('searchButton');
const clearSearchTextBtn = document.getElementById('clearSearchText');

const activityFilter = document.getElementById('activityFilter');
const materialFilter = document.getElementById('materialFilter');
const colorFilter = document.getElementById('colorFilter');
const createPartButton = document.getElementById('createPartButton');
const partsCount = document.getElementById('partsCount');

// --- Modals ---
const createModal = document.getElementById('createModal');
const editModal = document.getElementById('editModal');
const deleteModal = document.getElementById('deleteModal');
const descriptionModal = document.getElementById('descriptionModal');
const errorModal = document.getElementById('errorModal');
const priceModal = document.getElementById('priceModal');
const analyticsModal = document.getElementById('analyticsModal');

// Analytics Controls
const analyticsPrevYearBtn = document.getElementById('analyticsPrevYear');
const analyticsNextYearBtn = document.getElementById('analyticsNextYear');
const analyticsYearDisplay = document.getElementById('analyticsYearDisplay');

// --- Forms ---
const createForm = document.getElementById('createForm');
const editForm = document.getElementById('editForm');
const deleteForm = document.getElementById('deleteForm');
const priceForm = document.getElementById('priceForm');

// --- Specific Inputs ---
const createMaterialSelect = document.getElementById('createMaterial');
const editMaterialSelect = document.getElementById('editMaterial');
const createColorSelect = document.getElementById('createColor');
const editColorSelect = document.getElementById('editColor');

const editIdInput = document.getElementById('editId');
const deleteIdInput = document.getElementById('deleteId');
const descriptionContent = document.getElementById('descriptionContent');

const pricePartIdInput = document.getElementById('pricePartId');
const pricePartNameDisplay = document.getElementById('pricePartName');
const priceValueInput = document.getElementById('priceValue');
const priceDateInput = document.getElementById('priceDate');
const priceHistoryList = document.getElementById('priceHistoryList');

// --- Pagination & Table ---
const prevBtn = document.getElementById('prev');
const nextBtn = document.getElementById('next');
const pageInfo = document.getElementById('pageInfo');
const table = document.getElementById('partsTable');
const headers = table.querySelectorAll('thead th');

// === INITIALIZATION ===

headers.forEach((th, index) => {
    th.style.cursor = 'pointer';
    th.addEventListener('click', () => onHeaderClick(index, th));
});

document.addEventListener('DOMContentLoaded', async () => {
    try {
        toggleLoader(true);
        initSearchModeSwitcher(() => renderTable());
        attachRowSelection('#partsTable tbody');

        // Инициализация автозамены запятых
        initAutoDecimalCorrection();

        if (analyticsPrevYearBtn) analyticsPrevYearBtn.addEventListener('click', () => changeAnalyticsYear(-1));
        if (analyticsNextYearBtn) analyticsNextYearBtn.addEventListener('click', () => changeAnalyticsYear(1));

        await initParts();
        initFiltersFromData();
        initCustomSelects();
        renderTable();
    } finally {
        toggleLoader(false);
    }
    startAutoSync();
});

// === DATA FETCHING ===

async function initParts() {
    await Promise.all([
        syncTable('Parts'),
        syncTable('Materials'),
        syncTable('Colors'),
        syncTable('Prices')
    ]);

    const [partsData, materials, colors, prices] = await Promise.all([
        getAll('Parts'),
        getAll('Materials'),
        getAll('Colors'),
        getAll('Prices')
    ]);

    cached_parts = partsData || [];
    cached_materials = materials || [];
    cached_colors = colors || [];
    cached_prices = prices || [];
}

function initFiltersFromData() {
    fillSelect(materialFilter, 'Материал: все записи', cached_materials);
    fillSelect(colorFilter, 'Цвет: все записи', cached_colors);
    fillSelect(createMaterialSelect, 'Выберите материал', cached_materials);
    fillSelect(editMaterialSelect, 'Выберите материал', cached_materials);
    fillSelect(createColorSelect, 'Выберите цвет', cached_colors);
    fillSelect(editColorSelect, 'Выберите цвет', cached_colors);

    if (activityFilter.options.length === 1) {
        activityFilter.innerHTML = '';
        const allOpt = document.createElement('option');
        allOpt.value = ''; allOpt.textContent = 'Активность: все детали';
        activityFilter.appendChild(allOpt);

        const activeOpt = document.createElement('option');
        activeOpt.value = 'active';
        activeOpt.textContent = 'Только активные детали';
        activityFilter.appendChild(activeOpt);

        const inactiveOpt = document.createElement('option');
        inactiveOpt.value = 'inactive';
        inactiveOpt.textContent = 'Только неактивные детали';
        activityFilter.appendChild(inactiveOpt);
    }
}

function startAutoSync() {
    async function loop() {
        await initParts();
        initFiltersFromData();
        renderTable();
        setTimeout(loop, 5 * 60 * 1000);
    }
    loop();
}

// === SORTING ===

function onHeaderClick(index, th) {
    const fieldMap = ['name', 'material', 'color', 'size', 'weight', 'activity', 'price', 'description', 'actions'];
    const field = fieldMap[index];

    if (!field || field === 'actions') return;

    if (sortField === field) {
        sortDirection = sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
        sortField = field;
        sortDirection = 'asc';
    }

    updateHeaderSortUI(headers, index, sortDirection);
    currentPage = 1;
    renderTable();
}

// === RENDERING ===

function renderTable() {
    const tbody = document.querySelector('#partsTable tbody');
    if (!tbody) return;

    const search = (searchInput.value || '').toLowerCase().trim();
    const activity = activityFilter.value;
    const material = materialFilter.value;
    const color = colorFilter.value;

    tbody.innerHTML = '';

    let finded_parts = cached_parts.filter(p => {
        if (search) {
            const haystack = [
                p.name, p.description, p.length, p.width, p.height, p.diameter, p.weight
            ].map(v => (v ?? '').toString().toLowerCase()).join(' ');

            if (!haystack.includes(search)) return false;
        }

        if (activity === 'active' && !p.activity) return false;
        if (activity === 'inactive' && p.activity) return false;

        const materialId = material ? Number(material) : null;
        const colorId = color ? Number(color) : null;

        if (materialId !== null && p.materialId !== materialId) return false;
        if (colorId !== null && p.colorId !== colorId) return false;

        return true;
    });

    const mode = getSearchMode();
    let isHighlight = false;

    if (mode === 'filter') {
        parts = finded_parts;
    } else if (mode === 'highlight') {
        parts = cached_parts.filter(p => {
            if (activity === 'active' && !p.activity) return false;
            if (activity === 'inactive' && p.activity) return false;
            const materialId = material ? Number(material) : null;
            const colorId = color ? Number(color) : null;
            if (materialId !== null && p.materialId !== materialId) return false;
            if (colorId !== null && p.colorId !== colorId) return false;
            return true;
        });
        isHighlight = true;
    }

    if (partsCount) {
        const total = cached_parts.length;
        const filtered = finded_parts.length;
        partsCount.textContent = mode === 'filter'
            ? `Деталей: ${filtered} из ${total}`
            : `Деталей: ${parts.length} (совпадений: ${filtered}) из ${total}`;
    }

    const priceMap = new Map();
    const sortedPrices = [...cached_prices].sort((a, b) => new Date(b.date) - new Date(a.date));
    sortedPrices.forEach(p => {
        if (!priceMap.has(p.partId)) {
            priceMap.set(p.partId, p.value);
        }
    });

    if (parts.length > 0 && sortField) {
        const materialById = new Map(cached_materials.map(m => [m.id, m]));
        const colorById = new Map(cached_colors.map(c => [c.id, c]));

        parts.sort((a, b) => {
            const dir = sortDirection === 'asc' ? 1 : -1;
            const getValue = (item) => {
                switch (sortField) {
                    case 'name': return (item.name || '').toLowerCase();
                    case 'material': return (materialById.get(item.materialId)?.name || '').toLowerCase();
                    case 'color': return (colorById.get(item.colorId)?.name || '').toLowerCase();
                    case 'size': return [item.length || 0, item.width || 0, item.height || 0, item.diameter || 0];
                    case 'weight': return Number(item.weight ?? 0);
                    case 'activity': return item.activity ? 1 : 0;
                    case 'price': return priceMap.get(item.id) || 0;
                    case 'description': return (item.description || '').toLowerCase();
                    default: return '';
                }
            };
            const va = getValue(a);
            const vb = getValue(b);
            if (Array.isArray(va) && Array.isArray(vb)) {
                for (let i = 0; i < va.length; i++) {
                    if (va[i] < vb[i]) return -1 * dir;
                    if (va[i] > vb[i]) return 1 * dir;
                }
                return 0;
            }
            return va < vb ? -1 * dir : (va > vb ? 1 * dir : 0);
        });
    }

    currentPage = updatePaginationUI(currentPage, parts.length, perPage, pageInfo, prevBtn, nextBtn);

    if (parts.length === 0) {
        tbody.innerHTML = '<tr><td colspan="9" class="text-center">Записей нет</td></tr>';
        return;
    }

    const pageParts = paginateData(parts, currentPage, perPage);
    const materialById = new Map(cached_materials.map(m => [m.id, m]));
    const colorById = new Map(cached_colors.map(c => [c.id, c]));

    const fragment = document.createDocumentFragment();

    for (const item of pageParts) {
        const tr = document.createElement('tr');
        if (isHighlight && finded_parts.includes(item)) {
            tr.classList.add('highlight-match');
        }

        const tdName = document.createElement('td');
        tdName.textContent = item.name;

        const tdMaterial = document.createElement('td');
        tdMaterial.textContent = materialById.get(item.materialId)?.name || '—';

        const tdColor = document.createElement('td');
        tdColor.textContent = colorById.get(item.colorId)?.name || '—';

        const tdSize = document.createElement('td');
        tdSize.textContent = (item.diameter && item.diameter > 0)
            ? `${item.length} мм Ø${item.diameter} мм`
            : `${item.length}x${item.width}x${item.height} мм`;

        const tdWeight = document.createElement('td');
        tdWeight.textContent = item.weight + ' кг';

        const tdActivity = document.createElement('td');
        tdActivity.textContent = item.activity ? 'Активна' : 'Деактивирована';

        // PRICE COLUMN
        const currentPrice = priceMap.get(item.id) || 0;
        const tdPrice = document.createElement('td');
        tdPrice.classList.add('col-fit');

        const wrapper = document.createElement('div');
        wrapper.className = 'price-cell-wrapper justify-content-end';

        // 1. Цена текстом
        const priceText = document.createElement('span');
        priceText.className = 'price-value-text me-2';
        priceText.textContent = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' }).format(currentPrice);

        // 2. Кнопка Динамики
        const analyticsBtn = document.createElement('button');
        analyticsBtn.textContent = '📈';
        analyticsBtn.title = 'Динамика цен';
        // Добавляем btn-square. me-2 оставляем для отступа от следующей кнопки
        analyticsBtn.className = 'btn btn-icon-mini btn-square me-2';
        analyticsBtn.addEventListener('click', (e) => {
            e.stopPropagation();
            openAnalyticsModal(item);
        });

        // 3. Кнопка добавить цену
        const addPriceBtn = document.createElement('button');
        // Добавляем btn-square
        addPriceBtn.className = 'btn btn-sm btn-success btn-add-price btn-square';
        addPriceBtn.title = 'Обновить цену';
        addPriceBtn.innerHTML = '+'; // Упрощаем HTML для идеального центрирования
        addPriceBtn.onclick = (e) => {
            e.stopPropagation();
            const history = cached_prices
                .filter(p => p.partId === item.id)
                .sort((a, b) => new Date(b.date) - new Date(a.date));
            openPriceModal(item, history);
        };

        wrapper.appendChild(priceText);
        wrapper.appendChild(analyticsBtn);
        wrapper.appendChild(addPriceBtn);
        tdPrice.appendChild(wrapper);

        // Description (строки ~300)
        const tdDescription = document.createElement('td');
        tdDescription.classList.add('col-fit');
        const descBtn = document.createElement('button');
        descBtn.textContent = '📄'; // Убрали span
        descBtn.title = 'Описание';
        descBtn.className = 'btn btn-sm btn-description btn-square'; // Добавили btn-square
        descBtn.addEventListener('click', () => {
            descriptionContent.textContent = item.description || 'Описания нет';
            showModal(descriptionModal);
        });
        tdDescription.appendChild(descBtn);

        // Actions
        const tdActions = document.createElement('td');
        tdActions.classList.add('col-fit');

        const actionsDiv = document.createElement('div');
        actionsDiv.className = 'actions-group justify-content-end';

        const editBtn = document.createElement('button');
        editBtn.textContent = '✎'; // Убрали span
        editBtn.title = 'Изменить';
        editBtn.className = 'btn btn-sm btn-primary btn-square'; // Добавили btn-square
        editBtn.addEventListener('click', () => openEditModal(item));

        const deleteBtn = document.createElement('button');
        deleteBtn.textContent = '🗑'; // Убрали span
        deleteBtn.title = 'Удалить';
        deleteBtn.className = 'btn btn-sm btn-danger btn-square'; // Добавили btn-square
        deleteBtn.addEventListener('click', () => openDeleteModal(item));

        actionsDiv.appendChild(editBtn);
        actionsDiv.appendChild(deleteBtn);
        tdActions.appendChild(actionsDiv);

        tr.appendChild(tdName);
        tr.appendChild(tdMaterial);
        tr.appendChild(tdColor);
        tr.appendChild(tdSize);
        tr.appendChild(tdWeight);
        tr.appendChild(tdActivity);
        tr.appendChild(tdPrice);
        tr.appendChild(tdDescription);
        tr.appendChild(tdActions);

        fragment.appendChild(tr);
    }
    tbody.appendChild(fragment);
}

// === FORM HANDLING ===
createPartButton.addEventListener('click', () => {
    if (createForm) {
        initCustomSelects()
        createForm.reset();
    }
    const activeCheck = document.getElementById('createIsActive');
    if (activeCheck) activeCheck.checked = true;
    showModal(createModal);
});

function openEditModal(item) {
    editIdInput.value = item.id;
    document.getElementById('editName').value = item.name || '';
    document.getElementById('editLength').value = item.length ?? '';
    document.getElementById('editWidth').value = item.width ?? '';
    document.getElementById('editHeight').value = item.height ?? '';
    document.getElementById('editDiameter').value = item.diameter ?? '';
    document.getElementById('editWeight').value = item.weight ?? '';
    document.getElementById('editIsActive').checked = item.activity;
    document.getElementById('editDescription').value = item.description || '';
    if (editMaterialSelect) editMaterialSelect.value = item.materialId ?? '';
    if (editColorSelect) editColorSelect.value = item.colorId ?? '';
    initCustomSelects();
    showModal(editModal);
}

function openDeleteModal(item) {
    deleteIdInput.value = item.id;
    showModal(deleteModal);
}

function validatePartForm(prefix) {
    const errors = [];
    const name = document.getElementById(prefix + 'Name');
    const material = document.getElementById(prefix + 'Material');
    const color = document.getElementById(prefix + 'Color');
    const length = document.getElementById(prefix + 'Length');
    const width = document.getElementById(prefix + 'Width');
    const height = document.getElementById(prefix + 'Height');
    const diameter = document.getElementById(prefix + 'Diameter');
    const weight = document.getElementById(prefix + 'Weight');

    if (!name.value.trim()) errors.push('Поле "Название" обязательно.');
    if (!material.value) errors.push('Поле "Материал" обязательно.');
    if (!color.value) errors.push('Поле "Цвет" обязательно.');

    if (!length.value || Number(length.value) <= 0) errors.push('Поле "Длина" должно быть больше 0.');
    if (!weight.value || Number(weight.value) <= 0) errors.push('Поле "Вес" должно быть больше 0.');

    const w = Number(width.value), h = Number(height.value), d = Number(diameter.value);
    if ((!w || w <= 0 || !h || h <= 0) && (!d || d <= 0)) {
        errors.push('Необходимо указать корректные габариты (Диаметр или Ширина+Высота).');
    }

    if (errors.length > 0) {
        const errorList = document.getElementById('errorList');
        errorList.innerHTML = '';
        errors.forEach(msg => {
            const li = document.createElement('li');
            li.textContent = msg;
            errorList.appendChild(li);
        });
        showModal(errorModal);
        return false;
    }
    return true;
}

prevBtn.addEventListener('click', () => { if (currentPage > 1) { currentPage--; renderTable(); } });
nextBtn.addEventListener('click', () => {
    const totalPages = Math.ceil(parts.length / perPage);
    if (currentPage < totalPages) { currentPage++; renderTable(); }
});

function validatePriceAddForm() {
    const errors = [];
    const valInput = document.getElementById('priceValue');
    const dateInput = document.getElementById('priceDate');

    if (!valInput.value) {
        errors.push('Поле "Стоимость" обязательно.');
    } else if (Number(valInput.value) <= 0) {
        errors.push('Поле "Стоимость" должно быть больше 0.');
    }

    if (!dateInput.value) {
        errors.push('Поле "Дата установки" обязательно.');
    } else if (dateInput.min && dateInput.value < dateInput.min) {
        const minDatePretty = new Date(dateInput.min).toLocaleString('ru-RU');
        errors.push(`Дата не может быть раньше предыдущей записи (${minDatePretty}).`);
    }

    if (errors.length > 0) {
        const errorList = document.getElementById('errorList');
        errorList.innerHTML = '';
        errors.forEach(msg => {
            const li = document.createElement('li');
            li.textContent = msg;
            errorList.appendChild(li);
        });
        showModal(errorModal);
        return false;
    }
    return true;
}

async function handleFormSubmit(e, handlerName, modalToClose) {
    e.preventDefault();

    if (e.target.id === 'priceForm') {
        if (!validatePriceAddForm()) return;
    }
    else if (e.target.id !== 'deleteForm') {
        const formPrefix = e.target.id.replace('Form', '');
        if (!validatePartForm(formPrefix)) return;
    }

    try {
        toggleLoader(true);
        const formData = new FormData(e.target);
        const response = await fetch(`?handler=${handlerName}`, { method: 'POST', body: formData });
        const result = await response.json();

        if (!result.success) {
            const errorList = document.getElementById('errorList');
            errorList.innerHTML = `<li>${result.message || 'Ошибка операции'}</li>`;
            showModal(errorModal);
        } else {
            hideModal(modalToClose);
            if (handlerName === 'CreatePrice') {
                await syncTable('Prices');
                const prices = await getAll('Prices');
                cached_prices = prices || [];
            } else {
                await initParts();
            }
            renderTable();
        }
    } finally {
        toggleLoader(false);
    }
}

createForm.addEventListener('submit', (e) => handleFormSubmit(e, 'CreatePart', createModal));
editForm.addEventListener('submit', (e) => handleFormSubmit(e, 'UpdatePart', editModal));
deleteForm.addEventListener('submit', (e) => handleFormSubmit(e, 'DeletePart', deleteModal));
priceForm.addEventListener('submit', (e) => handleFormSubmit(e, 'CreatePrice', priceModal));

function openPriceModal(part, history) {
    const pricePartIdInput = document.getElementById('pricePartId');
    const pricePartNameDisplay = document.getElementById('pricePartName');
    const priceForm = document.getElementById('priceForm');
    const priceDateInput = document.getElementById('priceDate');
    const historyContainer = document.getElementById('priceHistoryContainer');

    pricePartIdInput.value = part.id;
    pricePartNameDisplay.textContent = part.name;

    priceForm.reset();
    const now = new Date();
    now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
    priceDateInput.value = now.toISOString().slice(0, 16);

    if (history && history.length > 0) {
        const lastPrice = history[0];
        const prevDate = new Date(lastPrice.date);
        prevDate.setMinutes(prevDate.getMinutes() - prevDate.getTimezoneOffset());
        priceDateInput.min = prevDate.toISOString().slice(0, 16);
    } else {
        priceDateInput.removeAttribute('min');
    }

    historyContainer.innerHTML = '';

    if (history && history.length > 0) {
        history.slice(0, 10).forEach(h => {
            const card = document.createElement('div');
            card.className = 'price-history-card';
            const dateObj = new Date(h.date);
            const dateStr = dateObj.toLocaleDateString();
            card.innerHTML = `
                <div class="price-history-date">${dateStr}</div>
                <div class="price-history-val">${h.value} ₽</div>
            `;
            historyContainer.appendChild(card);
        });
    } else {
        const emptyMsg = document.createElement('div');
        emptyMsg.className = 'price-history-empty';
        emptyMsg.textContent = 'История цен пуста';
        historyContainer.appendChild(emptyMsg);
    }

    showModal(document.getElementById('priceModal'));
}

// === ANALYTICS MODAL LOGIC ===

function openAnalyticsModal(part) {
    currentAnalyticsPartId = part.id;
    currentAnalyticsYear = new Date().getFullYear();
    renderAnalytics(part);
    showModal(analyticsModal);
}

function changeAnalyticsYear(delta) {
    const part = parts.find(p => p.id === currentAnalyticsPartId) || cached_parts.find(p => p.id === currentAnalyticsPartId);
    if (!part) return;

    const allHistory = cached_prices
        .filter(p => p.partId === part.id)
        .sort((a, b) => new Date(a.date) - new Date(b.date));

    let minYear = new Date().getFullYear();
    if (allHistory.length > 0) {
        minYear = new Date(allHistory[0].date).getFullYear();
    }
    const maxYear = new Date().getFullYear();

    const newYear = currentAnalyticsYear + delta;
    if (newYear < minYear || newYear > maxYear) return;

    currentAnalyticsYear = newYear;
    renderAnalytics(part);
}

function renderAnalytics(part) {
    const nameDisplay = document.getElementById('analyticsPartName');
    const priceDisplay = document.getElementById('analyticsCurrentPrice');
    const yearDisplay = document.getElementById('analyticsYearDisplay');
    const minDisplay = document.getElementById('analyticsMinPrice');
    const maxDisplay = document.getElementById('analyticsMaxPrice');
    const prevBtnYear = document.getElementById('analyticsPrevYear');
    const nextBtnYear = document.getElementById('analyticsNextYear');

    nameDisplay.textContent = part.name;
    yearDisplay.textContent = currentAnalyticsYear;

    const allHistory = cached_prices
        .filter(p => p.partId === part.id)
        .sort((a, b) => new Date(a.date) - new Date(b.date));

    const currentPrice = allHistory.length > 0 ? allHistory[allHistory.length - 1].value : 0;
    priceDisplay.textContent = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' }).format(currentPrice);

    if (allHistory.length > 0) {
        const vals = allHistory.map(h => h.value);
        const minVal = Math.min(...vals);
        const maxVal = Math.max(...vals);
        minDisplay.textContent = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' }).format(minVal);
        maxDisplay.textContent = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' }).format(maxVal);
    } else {
        minDisplay.textContent = "0 ₽";
        maxDisplay.textContent = "0 ₽";
    }

    let minYear = new Date().getFullYear();
    if (allHistory.length > 0) {
        minYear = new Date(allHistory[0].date).getFullYear();
    }
    const realCurrentYear = new Date().getFullYear();

    prevBtnYear.disabled = currentAnalyticsYear <= minYear;
    nextBtnYear.disabled = currentAnalyticsYear >= realCurrentYear;

    const startOfYearDate = new Date(currentAnalyticsYear, 0, 1);
    const endOfYearDate = new Date(currentAnalyticsYear, 11, 31, 23, 59, 59);

    const now = new Date();
    const isCurrentViewingYear = currentAnalyticsYear === now.getFullYear();
    const lineEndDate = isCurrentViewingYear ? now : endOfYearDate;

    let startPrice = null;
    const previousPrices = allHistory.filter(h => new Date(h.date) < startOfYearDate);
    if (previousPrices.length > 0) {
        startPrice = previousPrices[previousPrices.length - 1].value;
    }

    const yearPrices = allHistory.filter(h => {
        const d = new Date(h.date);
        return d.getFullYear() === currentAnalyticsYear;
    });

    const seriesData = [];

    if (startPrice !== null) {
        seriesData.push([startOfYearDate.getTime(), startPrice]);
    }

    yearPrices.forEach(h => {
        const d = new Date(h.date);
        seriesData.push([d.getTime(), h.value]);
    });

    if (seriesData.length > 0) {
        const lastKnownVal = seriesData[seriesData.length - 1][1];
        seriesData.push([lineEndDate.getTime(), lastKnownVal]);
    }

    // Helper to fetch CSS variables
    const getCssVar = (name) => getComputedStyle(document.documentElement).getPropertyValue(name).trim();

    // Theme Colors
    const colAccent = getCssVar('--accent-secondary') || '#e27b32';
    const colMuted = getCssVar('--text-muted') || '#8a6540';
    const colGrid = getCssVar('--warm-divider') || 'rgba(125, 94, 60, 0.4)';
    const colMin = getCssVar('--action-create') || '#1f5a35';
    const colMax = getCssVar('--danger-main') || '#500805';

    let annotationsY = [];
    if (allHistory.length > 0) {
        const vals = allHistory.map(h => h.value);
        const minVal = Math.min(...vals);
        const maxVal = Math.max(...vals);

        annotationsY = [
            {
                y: minVal,
                borderColor: colMin,
                layer: 'back',
                strokeDashArray: 4
            },
            {
                y: maxVal,
                borderColor: colMax,
                layer: 'back',
                strokeDashArray: 4
            }
        ];
    }

    if (analyticsChart) {
        analyticsChart.destroy();
    }

    // Render Options
    const options = {
        colors: [colAccent],
        series: [{
            name: "Цена",
            data: seriesData
        }],
        annotations: {
            yaxis: annotationsY
        },
        chart: {
            type: 'line',
            height: 300,
            toolbar: { show: false },
            zoom: { enabled: false },
            fontFamily: 'inherit',
            locales: [{
                "name": "ru",
                "options": {
                    "months": ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
                    "shortMonths": ["Янв", "Фев", "Мар", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек"],
                    "days": ["Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота"],
                    "shortDays": ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
                    "toolbar": {
                        "exportToSVG": "Скачать SVG",
                        "exportToPNG": "Скачать PNG",
                        "menu": "Меню"
                    }
                }
            }],
            defaultLocale: "ru"
        },
        stroke: {
            curve: 'stepline',
            width: 3
        },
        grid: {
            borderColor: colGrid,
            strokeDashArray: 4,
            padding: {
                top: 25,
                bottom: 25,
                left: 10,
                right: 10
            }
        },
        dataLabels: { enabled: false },
        xaxis: {
            type: 'datetime',
            min: startOfYearDate.getTime(),
            max: endOfYearDate.getTime(),
            tooltip: { enabled: false },
            labels: {
                datetimeFormatter: {
                    year: 'yyyy',
                    month: 'MMM',
                    day: 'dd MMM'
                },
                style: { colors: colMuted },
                datetimeUTC: false
            },
            axisBorder: { show: false },
            axisTicks: { show: false }
        },
        yaxis: {
            labels: {
                formatter: (val) => val.toFixed(0) + " ₽",
                style: { colors: colMuted }
            }
        },
        fill: {
            type: 'gradient',
            gradient: {
                shade: 'dark',
                gradientToColors: [colAccent],
                shadeIntensity: 1,
                type: 'horizontal',
                opacityFrom: 0.9,
                opacityTo: 0.9,
                stops: [0, 100]
            },
        },
        markers: {
            size: 4,
            colors: ['#fff'],
            strokeColors: colAccent,
            strokeWidth: 2,
            hover: { size: 6 }
        },
        tooltip: {
            x: { format: 'dd MMM yyyy HH:mm' },
            y: {
                formatter: function (val) {
                    return val + " ₽";
                }
            },
            marker: {
                show: true,
                fillColors: [colAccent]
            },
            theme: 'false', // Disable default theme to let CSS take control
            cssClass: 'apexcharts-tooltip-custom'
        },
        noData: {
            text: 'Нет данных за этот период',
            align: 'center',
            verticalAlign: 'middle',
            style: { color: colMuted, fontSize: '16px' }
        }
    };

    analyticsChart = new ApexCharts(document.querySelector("#priceChart"), options);
    analyticsChart.render();
}

if (clearSearchTextBtn) {
    clearSearchTextBtn.addEventListener('click', () => {
        searchInput.value = '';
        currentPage = 1;
        renderTable();
    });
}

activityFilter.addEventListener('change', () => { currentPage = 1; renderTable(); });
materialFilter.addEventListener('change', () => { currentPage = 1; renderTable(); });
colorFilter.addEventListener('change', () => { currentPage = 1; renderTable(); });

// === AUTO DECIMAL CORRECTION ===

function initAutoDecimalCorrection() {
    const decimalInputIds = [
        // Создание детали
        'createLength', 'createWidth', 'createHeight', 'createDiameter', 'createWeight',
        // Редактирование детали
        'editLength', 'editWidth', 'editHeight', 'editDiameter', 'editWeight',
        // Управление ценой
        'priceValue'
    ];

    decimalInputIds.forEach(id => {
        const el = document.getElementById(id);
        if (el) {
            el.addEventListener('input', function () {
                if (this.value.includes(',')) {
                    this.value = this.value.replace(/,/g, '.');
                }
            });
        }
    });
}