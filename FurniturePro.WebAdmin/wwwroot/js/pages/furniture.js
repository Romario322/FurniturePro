import { syncTable } from '../api/syncService.js';
import { getAll, clearStore } from '../db/repository.js';
import { toggleLoader } from '../ui/loader.js';
import { showModal, hideModal } from '../ui/modal.js';
import { attachRowSelection, updateHeaderSortUI } from '../ui/tables.js';
import { getSearchMode, initSearchModeSwitcher, fillSelect } from '../utils/domHelper.js';
import { paginateData, updatePaginationUI } from '../utils/pagination.js';
import { initCustomSelects } from '../ui/customSelect.js';

class FurniturePage {
    constructor() {
        // Данные
        this.cachedFurniture = [];
        this.cachedCategories = [];
        this.cachedCompositions = [];
        this.cachedParts = [];
        this.cachedPrices = [];
        this.currentData = [];

        // Состояние редактора
        this.currentCompositionDraft = [];
        this.currentEditingFurnitureId = null;

        // Пагинация и сортировка
        this.currentPage = 1;
        this.perPage = 20;
        this.sortField = null;
        this.sortDirection = 'asc';

        this.initDOM();
        this.attachEvents();
    }

    initDOM() {
        this.table = document.getElementById('furnitureTable');
        this.tbody = this.table?.querySelector('tbody');
        this.headers = this.table?.querySelectorAll('thead th');

        this.searchInput = document.getElementById('searchInput');
        this.searchButton = document.getElementById('searchButton');
        this.clearSearchTextBtn = document.getElementById('clearSearchText');
        this.activityFilter = document.getElementById('activityFilter');
        this.categoryFilter = document.getElementById('categoryFilter');
        this.createFurnitureButton = document.getElementById('createFurnitureButton');
        this.furnitureCount = document.getElementById('furnitureCount');
        this.pageInfo = document.getElementById('pageInfo');
        this.prevBtn = document.getElementById('prev');
        this.nextBtn = document.getElementById('next');

        this.createModal = document.getElementById('createModal');
        this.editModal = document.getElementById('editModal');
        this.deleteModal = document.getElementById('deleteModal');
        this.descriptionModal = document.getElementById('descriptionModal');
        this.compositionModal = document.getElementById('compositionModal');
        this.editCompositionModal = document.getElementById('editCompositionModal');
        this.errorModal = document.getElementById('errorModal');

        this.createForm = document.getElementById('createForm');
        this.editForm = document.getElementById('editForm');
        this.deleteForm = document.getElementById('deleteForm');

        this.partSearchInput = document.getElementById('partSearchInput');
        this.clearPartSearchBtn = document.getElementById('clearPartSearch');
        this.saveCompositionBtn = document.getElementById('saveCompositionBtn');
        this.pModeFilter = document.getElementById('pModeFilter');
        this.pModeHighlight = document.getElementById('pModeHighlight');
        this.currentCompTableBody = document.querySelector('#currentCompTable tbody');
        this.availablePartsTableBody = document.querySelector('#availablePartsTable tbody');

        this.headerTotalCountLabel = document.getElementById('headerTotalCount');
        this.summaryPriceLabel = document.getElementById('summaryTotalPrice');

        this.createCategorySelect = document.getElementById('createCategory');
        this.editCategorySelect = document.getElementById('editCategory');
        this.editIdInput = document.getElementById('editId');
        this.deleteIdInput = document.getElementById('deleteId');
        this.descriptionContent = document.getElementById('descriptionContent');
        this.compositionContent = document.getElementById('compositionContent');
    }

    async init() {
        try {
            toggleLoader(true);
            initSearchModeSwitcher(() => this.renderTable());
            attachRowSelection('#furnitureTable tbody');

            await this.loadData();
            this.initFiltersFromData();
            initCustomSelects();

            this.renderTable();
        } catch (e) {
            console.error("Init error:", e);
        } finally {
            toggleLoader(false);
        }
        this.startAutoSync();
    }

    async loadData() {
        await Promise.all([
            syncTable('Furniture'), syncTable('Categories'),
            syncTable('FurnitureCompositions'), syncTable('Parts'), syncTable('Prices')
        ]);

        const [furnitureData, categories, compositions, parts, prices] = await Promise.all([
            getAll('Furniture'), getAll('Categories'),
            getAll('FurnitureCompositions'), getAll('Parts'), getAll('Prices')
        ]);

        this.cachedFurniture = furnitureData || [];
        this.cachedCategories = categories || [];
        this.cachedCompositions = compositions || [];
        this.cachedParts = parts || [];
        this.cachedPrices = prices || [];
    }

    initFiltersFromData() {
        fillSelect(this.categoryFilter, 'Категория: все записи', this.cachedCategories);
        fillSelect(this.createCategorySelect, 'Выберите категорию', this.cachedCategories);
        fillSelect(this.editCategorySelect, 'Выберите категорию', this.cachedCategories);

        if (this.activityFilter && this.activityFilter.options.length <= 1) {
            this.activityFilter.innerHTML = '';
            const allOpt = document.createElement('option'); allOpt.value = ''; allOpt.textContent = 'Активность: все записи';
            const activeOpt = document.createElement('option'); activeOpt.value = 'active'; activeOpt.textContent = 'Только активные';
            const inactiveOpt = document.createElement('option'); inactiveOpt.value = 'inactive'; inactiveOpt.textContent = 'Только неактивные';
            this.activityFilter.append(allOpt, activeOpt, inactiveOpt);
        }
    }

    startAutoSync() {
        const loop = async () => {
            await this.loadData();
            this.initFiltersFromData();
            this.renderTable();
            setTimeout(loop, 5 * 60 * 1000);
        };
        loop();
    }

    attachEvents() {
        if (this.headers) {
            this.headers.forEach((th, index) => {
                if (index < 6) {
                    th.style.cursor = 'pointer';
                    th.addEventListener('click', () => this.onHeaderClick(index));
                }
            });
        }

        const triggerRender = () => { this.currentPage = 1; this.renderTable(); };

        if (this.prevBtn) this.prevBtn.addEventListener('click', () => { if (this.currentPage > 1) { this.currentPage--; this.renderTable(); } });
        if (this.nextBtn) this.nextBtn.addEventListener('click', () => {
            const totalPages = Math.ceil(this.currentData.length / this.perPage);
            if (this.currentPage < totalPages) { this.currentPage++; this.renderTable(); }
        });

        if (this.searchButton) this.searchButton.addEventListener('click', triggerRender);
        if (this.searchInput) this.searchInput.addEventListener('keyup', (e) => { if (e.key === 'Enter') triggerRender(); });
        if (this.clearSearchTextBtn) this.clearSearchTextBtn.addEventListener('click', () => { this.searchInput.value = ''; triggerRender(); });
        if (this.activityFilter) this.activityFilter.addEventListener('change', triggerRender);
        if (this.categoryFilter) this.categoryFilter.addEventListener('change', triggerRender);

        if (this.createFurnitureButton) {
            this.createFurnitureButton.addEventListener('click', () => {
                if (this.createForm) { initCustomSelects(); this.createForm.reset(); }
                document.getElementById('createActivity').value = 'true';
                showModal(this.createModal);
            });
        }

        if (this.createForm) this.createForm.addEventListener('submit', (e) => this.handleFormSubmit(e, 'CreateFurniture', this.createModal));
        if (this.editForm) this.editForm.addEventListener('submit', (e) => this.handleFormSubmit(e, 'UpdateFurniture', this.editModal));
        if (this.deleteForm) this.deleteForm.addEventListener('submit', (e) => this.handleFormSubmit(e, 'DeleteFurniture', this.deleteModal));

        if (this.partSearchInput) this.partSearchInput.addEventListener('input', () => this.renderAvailablePartsTable());
        if (this.pModeFilter) this.pModeFilter.addEventListener('change', () => this.renderAvailablePartsTable());
        if (this.pModeHighlight) this.pModeHighlight.addEventListener('change', () => this.renderAvailablePartsTable());
        if (this.clearPartSearchBtn) this.clearPartSearchBtn.addEventListener('click', () => {
            this.partSearchInput.value = ''; this.partSearchInput.focus(); this.renderAvailablePartsTable();
        });

        if (this.saveCompositionBtn) this.saveCompositionBtn.addEventListener('click', () => this.handleCompositionSave());
    }

    onHeaderClick(index) {
        const fieldMap = ['name', 'category', 'selfPrice', 'markup', 'totalPrice', 'activity'];
        const field = fieldMap[index];
        if (!field) return;

        if (this.sortField === field) {
            this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
        } else {
            this.sortField = field;
            this.sortDirection = 'asc';
        }

        updateHeaderSortUI(this.headers, index, this.sortDirection);
        this.currentPage = 1;
        this.renderTable();
    }

    renderTable() {
        if (!this.tbody) return;

        const search = (this.searchInput?.value || '').toLowerCase().trim();
        const activity = this.activityFilter?.value;
        const category = this.categoryFilter?.value;

        this.tbody.innerHTML = '';

        const categoryById = new Map(this.cachedCategories.map(c => [c.id, c]));
        const partsById = new Map(this.cachedParts.map(p => [p.id, p]));
        const priceMap = new Map();

        const sortedPrices = [...this.cachedPrices].sort((a, b) => new Date(b.date) - new Date(a.date));
        sortedPrices.forEach(p => { if (!priceMap.has(p.partId)) priceMap.set(p.partId, p.value); });

        const compositionsByFurnitureId = new Map();
        this.cachedCompositions.forEach(comp => {
            const fId = comp.idFurniture ?? comp.entity1Id;
            if (!compositionsByFurnitureId.has(fId)) compositionsByFurnitureId.set(fId, []);
            compositionsByFurnitureId.get(fId).push(comp);
        });

        let viewData = this.cachedFurniture.map(item => {
            const comps = compositionsByFurnitureId.get(item.id) || [];
            let selfPrice = 0;
            comps.forEach(comp => {
                const pId = comp.idPart ?? comp.entity2Id;
                const count = comp.count ?? 0;
                selfPrice += (priceMap.get(pId) || 0) * count;
            });
            const markup = item.markup ?? 0;
            const totalPrice = selfPrice + (selfPrice * markup / 100);

            return {
                ...item,
                _categoryName: categoryById.get(item.categoryId)?.name || '',
                _selfPrice: selfPrice,
                _totalPrice: totalPrice,
                _compositions: comps
            };
        });

        const checkStaticFilters = (f) => {
            if (activity === 'active' && !f.activity) return false;
            if (activity === 'inactive' && f.activity) return false;
            const categoryId = category ? Number(category) : null;
            if (categoryId !== null && f.categoryId !== categoryId) return false;
            return true;
        };

        let finded_items = viewData.filter(f => {
            if (search) {
                const haystack = [f.name, f.description, (f.markup ?? '').toString()].map(v => (v ?? '').toString().toLowerCase()).join(' ');
                if (!haystack.includes(search)) return false;
            }
            return checkStaticFilters(f);
        });

        const mode = getSearchMode();
        let isHighlight = false;

        if (mode === 'filter') {
            this.currentData = finded_items;
        } else if (mode === 'highlight') {
            this.currentData = viewData.filter(checkStaticFilters);
            isHighlight = true;
        }

        if (this.furnitureCount) {
            const total = this.cachedFurniture.length;
            const filtered = finded_items.length;
            this.furnitureCount.textContent = mode === 'filter'
                ? `Записей: ${filtered} из ${total}`
                : `Записей: ${this.currentData.length} (совпадений: ${filtered}) из ${total}`;
        }

        if (this.currentData.length > 0 && this.sortField) {
            this.currentData.sort((a, b) => {
                const dir = this.sortDirection === 'asc' ? 1 : -1;
                let valA, valB;
                switch (this.sortField) {
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

        this.currentPage = updatePaginationUI(this.currentPage, this.currentData.length, this.perPage, this.pageInfo, this.prevBtn, this.nextBtn);

        if (this.currentData.length === 0) {
            this.tbody.innerHTML = '<tr><td colspan="9" class="text-center" style="color:var(--text-muted);font-style:italic; padding: 2rem;">Записей нет</td></tr>';
            return;
        }

        const pageItems = paginateData(this.currentData, this.currentPage, this.perPage);
        const fragment = document.createDocumentFragment();
        const currencyFmt = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' });

        for (const item of pageItems) {
            const tr = document.createElement('tr');
            if (isHighlight && finded_items.find(x => x.id === item.id)) tr.classList.add('highlight-match');

            const tdName = document.createElement('td'); tdName.textContent = item.name;
            const tdCategory = document.createElement('td'); tdCategory.textContent = item._categoryName || '—';
            const tdSelfPrice = document.createElement('td'); tdSelfPrice.textContent = currencyFmt.format(item._selfPrice);
            const tdMarkup = document.createElement('td'); tdMarkup.className = 'fw-bold'; tdMarkup.textContent = (item.markup ?? 0) + '%';
            const tdPrice = document.createElement('td'); tdPrice.className = 'fw-bold'; tdPrice.textContent = currencyFmt.format(item._totalPrice);
            const tdActivity = document.createElement('td'); tdActivity.textContent = item.activity ? 'Активна' : 'Нет';

            const tdDescription = document.createElement('td');
            tdDescription.classList.add('col-fit');
            const descBtn = document.createElement('button');
            descBtn.textContent = '📄'; descBtn.title = 'Описание';
            descBtn.className = 'btn btn-sm btn-description btn-square';
            descBtn.onclick = () => {
                if (this.descriptionContent) this.descriptionContent.textContent = item.description || 'Описания нет';
                showModal(this.descriptionModal);
            };
            tdDescription.appendChild(descBtn);

            const tdComposition = document.createElement('td');
            tdComposition.classList.add('col-fit');
            const comps = item._compositions;
            if (comps.length > 0) {
                const compBtn = document.createElement('button');
                compBtn.innerHTML = `<span>🧩</span>`;
                compBtn.title = `Просмотр состава (${comps.length} поз.)`;
                compBtn.className = 'btn btn-sm btn-composition btn-square';
                compBtn.onclick = () => {
                    this.renderReadOnlyComposition(item.name, comps, partsById);
                    showModal(this.compositionModal);
                };
                tdComposition.appendChild(compBtn);
            } else {
                tdComposition.innerHTML = '<span style="color: var(--warm-border); opacity: 0.6;">—</span>';
            }

            const tdActions = document.createElement('td');
            tdActions.classList.add('col-fit');
            const actionsDiv = document.createElement('div');
            actionsDiv.className = 'actions-group justify-content-end';

            const configBtn = document.createElement('button');
            configBtn.textContent = '⚙'; configBtn.title = 'Редактировать состав';
            configBtn.className = 'btn btn-sm btn-outline-secondary btn-square';
            configBtn.onclick = () => this.openEditCompositionModal(item);

            const editBtn = document.createElement('button');
            editBtn.textContent = '✎'; editBtn.title = 'Изменить';
            editBtn.className = 'btn btn-sm btn-primary btn-square';
            editBtn.onclick = () => this.openEditModal(item);

            const deleteBtn = document.createElement('button');
            deleteBtn.textContent = '🗑'; deleteBtn.title = 'Удалить';
            deleteBtn.className = 'btn btn-sm btn-danger btn-square';
            deleteBtn.onclick = () => this.openDeleteModal(item);

            actionsDiv.append(configBtn, editBtn, deleteBtn);
            tdActions.appendChild(actionsDiv);

            tr.append(tdName, tdCategory, tdSelfPrice, tdMarkup, tdPrice, tdActivity, tdDescription, tdComposition, tdActions);
            fragment.appendChild(tr);
        }
        this.tbody.appendChild(fragment);
    }

    renderReadOnlyComposition(furnitureName, compositions, partsMap) {
        if (!this.compositionContent) return;
        this.compositionContent.className = '';
        this.compositionContent.innerHTML = '';
        this.compositionContent.style.padding = '0';
        this.compositionContent.style.border = 'none';
        this.compositionContent.style.background = 'transparent';
        this.compositionContent.style.maxHeight = 'none';
        this.compositionContent.style.overflow = 'visible';

        if (!compositions || compositions.length === 0) {
            this.compositionContent.innerHTML = `<div class="d-flex flex-column align-items-center justify-content-center p-4" style="color: var(--app-text);">
                <span class="fs-4 mb-2">∅</span><span>Состав для "<b>${furnitureName}</b>" не указан.</span></div>`;
            return;
        }

        const header = document.createElement('div');
        header.className = 'd-flex justify-content-between align-items-center mb-2 px-1';
        header.innerHTML = `
            <div class="d-flex flex-column">
                <span class="small text-uppercase fw-bold" style="color: var(--text-muted); letter-spacing: 0.05em; font-size: 0.75rem;">Изделие:</span>
                <span class="fw-bold" style="color: var(--app-text); font-size: 1.1rem; line-height: 1.2;">${furnitureName}</span>
            </div>
            <span class="badge badge-theme" style="font-size: 0.85rem;">${compositions.length} позиций</span>`;
        this.compositionContent.appendChild(header);

        const wrapper = document.createElement('div');
        wrapper.className = 'composition-table-wrapper';
        wrapper.style.height = '400px'; wrapper.style.marginBottom = '0';

        const scroll = document.createElement('div');
        scroll.className = 'composition-table-scroll';

        const table = document.createElement('table');
        table.className = 'table table-sm table-hover align-middle mb-0 admin-table';
        table.innerHTML = `<thead class="table-light"><tr><th>Деталь</th><th class="text-center" style="width: 100px;">Кол-во</th></tr></thead>`;

        const tbody = document.createElement('tbody');
        compositions.forEach(comp => {
            const pId = comp.idPart ?? comp.entity2Id;
            const part = partsMap.get(pId);
            const name = part ? part.name : `<span class="text-danger fst-italic">Деталь удалена (ID: ${pId})</span>`;
            const tr = document.createElement('tr');
            tr.innerHTML = `<td>${name}</td><td class="text-center fw-bold fs-6" style="color: var(--app-text);">${comp.count}</td>`;
            tbody.appendChild(tr);
        });

        table.appendChild(tbody);
        scroll.appendChild(table);
        wrapper.appendChild(scroll);
        this.compositionContent.appendChild(wrapper);

        const totalItems = compositions.reduce((sum, c) => sum + c.count, 0);
        const footer = document.createElement('div');
        footer.className = 'd-flex justify-content-end align-items-center mt-2 px-1 small';
        footer.style.color = 'var(--app-text)';
        footer.innerHTML = `<span style="font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; margin-right: 5px; color: var(--text-muted);">Всего деталей:</span> <b class="fs-6">${totalItems}</b>`;
        this.compositionContent.appendChild(footer);
    }

    getLatestPrice(partId) {
        const prices = this.cachedPrices.filter(p => p.partId === partId);
        if (!prices.length) return 0;
        prices.sort((a, b) => new Date(b.date) - new Date(a.date));
        return prices[0].value;
    }

    openEditCompositionModal(item) {
        this.currentEditingFurnitureId = item.id;
        const existingComps = this.cachedCompositions.filter(c => (c.idFurniture === item.id) || (c.entity1Id === item.id));
        const partsMap = new Map(this.cachedParts.map(p => [p.id, p]));

        this.currentCompositionDraft = existingComps.map(c => {
            const pId = c.idPart ?? c.entity2Id;
            const part = partsMap.get(pId);
            return { partId: pId, count: c.count, name: part ? part.name : 'Неизвестная деталь', price: this.getLatestPrice(pId) };
        });

        if (this.partSearchInput) this.partSearchInput.value = '';
        if (this.pModeFilter) this.pModeFilter.checked = true;

        this.renderDraftTable();
        this.renderAvailablePartsTable();
        showModal(this.editCompositionModal);
    }

    attachInnerRowSelection(tbody) {
        if (!tbody) return;
        tbody.querySelectorAll('tr').forEach(tr => {
            tr.addEventListener('click', (e) => {
                if (['INPUT', 'BUTTON', 'DIV'].includes(e.target.tagName)) return;
                tbody.querySelectorAll('tr.selected-row').forEach(r => r.classList.remove('selected-row'));
                tr.classList.add('selected-row');
            });
        });
    }

    renderDraftTable() {
        if (!this.currentCompTableBody) return;
        this.currentCompTableBody.innerHTML = '';

        let totalCount = 0;
        let totalSumMoney = 0;

        this.currentCompositionDraft.forEach((item, index) => {
            totalCount += item.count;
            const itemTotalCost = item.price * item.count;
            totalSumMoney += itemTotalCost;

            const tr = document.createElement('tr');
            const tdName = document.createElement('td'); tdName.textContent = item.name;

            const tdQty = document.createElement('td'); tdQty.className = 'text-center';
            const input = document.createElement('input');
            input.type = 'number'; input.value = item.count; input.min = 1; input.className = 'qty-input';
            input.onkeydown = (e) => { if (e.key === '-' || e.key === 'Subtract' || e.key === 'e') e.preventDefault(); };
            input.onchange = (e) => {
                let val = parseInt(e.target.value);
                if (val < 1 || isNaN(val)) { val = 1; e.target.value = 1; }
                item.count = val;
                this.renderDraftTable();
            };
            tdQty.appendChild(input);

            const tdSum = document.createElement('td');
            tdSum.className = 'text-end small fw-bold';
            tdSum.textContent = Math.round(itemTotalCost).toLocaleString('ru-RU') + ' ₽';

            const tdAct = document.createElement('td'); tdAct.className = 'col-fit';
            const delBtn = document.createElement('button');
            delBtn.textContent = '×'; delBtn.className = 'btn btn-sm btn-danger btn-square'; delBtn.title = 'Убрать из состава';
            delBtn.onclick = (e) => {
                e.stopPropagation();
                this.currentCompositionDraft.splice(index, 1);
                this.renderDraftTable();
                this.renderAvailablePartsTable();
            };
            tdAct.appendChild(delBtn);

            tr.append(tdName, tdQty, tdSum, tdAct);
            this.currentCompTableBody.appendChild(tr);
        });

        if (this.headerTotalCountLabel) this.headerTotalCountLabel.textContent = totalCount + ' шт.';
        if (this.summaryPriceLabel) this.summaryPriceLabel.textContent = totalSumMoney.toLocaleString('ru-RU', { style: 'currency', currency: 'RUB' });
        this.attachInnerRowSelection(this.currentCompTableBody);
    }

    renderAvailablePartsTable() {
        if (!this.availablePartsTableBody) return;
        this.availablePartsTableBody.innerHTML = '';

        const searchText = (this.partSearchInput?.value || '').toLowerCase().trim();
        const mode = document.querySelector('input[name="partSearchMode"]:checked')?.value || 'filter';
        const addedIds = new Set(this.currentCompositionDraft.map(x => x.partId));

        let partsToRender = this.cachedParts;
        if (mode === 'filter' && searchText) {
            partsToRender = this.cachedParts.filter(p => p.name.toLowerCase().includes(searchText));
        }

        const fragment = document.createDocumentFragment();

        partsToRender.forEach(part => {
            const isMatch = searchText && part.name.toLowerCase().includes(searchText);
            const isAdded = addedIds.has(part.id);
            const tr = document.createElement('tr');

            if (mode === 'highlight' && isMatch) tr.classList.add('highlight-match');
            if (isAdded) tr.style.backgroundColor = 'rgba(237, 217, 183, 0.3)';

            const tdName = document.createElement('td'); tdName.textContent = part.name;
            const tdPrice = document.createElement('td'); tdPrice.className = 'text-end small';
            const price = this.getLatestPrice(part.id);
            tdPrice.textContent = price.toLocaleString('ru-RU', { style: 'currency', currency: 'RUB' });

            const tdAction = document.createElement('td'); tdAction.className = 'col-fit text-center';
            if (isAdded) {
                const check = document.createElement('div');
                check.className = 'icon-added-check'; check.textContent = '✓'; check.title = 'Уже в составе';
                tdAction.appendChild(check);
            } else {
                const addBtn = document.createElement('button');
                addBtn.textContent = '+'; addBtn.className = 'btn btn-sm btn-success btn-square';
                addBtn.onclick = (e) => {
                    e.stopPropagation();
                    this.currentCompositionDraft.push({ partId: part.id, count: 1, name: part.name, price: price });
                    this.renderDraftTable();
                    this.renderAvailablePartsTable();
                };
                tdAction.appendChild(addBtn);
            }
            tr.append(tdName, tdPrice, tdAction);
            fragment.appendChild(tr);
        });

        this.availablePartsTableBody.appendChild(fragment);
        this.attachInnerRowSelection(this.availablePartsTableBody);
    }

    async handleCompositionSave() {
        if (!this.currentEditingFurnitureId) return;

        try {
            toggleLoader(true);
            const originalComps = this.cachedCompositions.filter(c => (c.idFurniture === this.currentEditingFurnitureId) || (c.entity1Id === this.currentEditingFurnitureId));
            const newComps = this.currentCompositionDraft;
            const toCreate = [], toUpdate = [], toDelete = [];

            originalComps.forEach(old => {
                const oldPartId = old.idPart ?? old.entity2Id;
                if (!newComps.find(n => n.partId === oldPartId)) {
                    toDelete.push({ IdFurniture: Number(this.currentEditingFurnitureId), IdPart: Number(oldPartId) });
                }
            });

            newComps.forEach(newItem => {
                const oldItem = originalComps.find(o => (o.idPart ?? o.entity2Id) === newItem.partId);
                const dto = { IdFurniture: Number(this.currentEditingFurnitureId), IdPart: Number(newItem.partId), Count: Number(newItem.count), UpdateDate: new Date().toISOString() };
                if (!oldItem) toCreate.push(dto);
                else if (oldItem.count !== newItem.count) toUpdate.push(dto);
            });

            const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
            const headers = { 'Content-Type': 'application/json', 'RequestVerificationToken': token };

            if (toCreate.length > 0) {
                const res = await fetch('?handler=CreateCompositionRange', { method: 'POST', headers: headers, body: JSON.stringify(toCreate) });
                const data = await res.json(); if (!data.success) throw new Error(data.message || 'Ошибка создания');
            }
            if (toUpdate.length > 0) {
                const res = await fetch('?handler=UpdateCompositionRange', { method: 'POST', headers: headers, body: JSON.stringify(toUpdate) });
                const data = await res.json(); if (!data.success) throw new Error(data.message || 'Ошибка обновления');
            }
            if (toDelete.length > 0) {
                const res = await fetch('?handler=DeleteCompositionRange', { method: 'POST', headers: headers, body: JSON.stringify(toDelete) });
                const data = await res.json();
                if (!data.success) throw new Error(data.message || 'Ошибка удаления');
                else await clearStore('FurnitureCompositions');
            }

            hideModal(this.editCompositionModal);
            await this.loadData();
            this.renderTable();
        } catch (e) {
            console.error(e); alert(e.message || 'Ошибка сохранения');
        } finally {
            toggleLoader(false);
        }
    }

    openEditModal(item) {
        if (this.editIdInput) this.editIdInput.value = item.id;
        document.getElementById('editName').value = item.name || '';
        document.getElementById('editMarkup').value = item.markup ?? 0;
        document.getElementById('editDescription').value = item.description || '';
        document.getElementById('editIsActive').checked = item.activity;

        if (this.editCategorySelect) {
            this.editCategorySelect.value = item.categoryId ?? '';
            this.editCategorySelect.dispatchEvent(new Event('change'));
        }
        initCustomSelects();
        showModal(this.editModal);
    }

    openDeleteModal(item) {
        if (this.deleteIdInput) this.deleteIdInput.value = item.id;
        showModal(this.deleteModal);
    }

    validateFurnitureForm(prefix) {
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
            if (errorList) {
                errorList.innerHTML = '';
                errors.forEach(msg => {
                    const li = document.createElement('li'); li.textContent = msg; errorList.appendChild(li);
                });
            }
            showModal(this.errorModal);
            return false;
        }
        return true;
    }

    async handleFormSubmit(e, handlerName, modalToClose) {
        e.preventDefault();
        const formPrefix = e.target.id.replace('Form', '');
        if (formPrefix !== 'delete' && !this.validateFurnitureForm(formPrefix)) return;

        try {
            toggleLoader(true);
            const formData = new FormData(e.target);
            if (formPrefix === 'edit') formData.set('Activity', document.getElementById('editIsActive').checked);

            const response = await fetch(`?handler=${handlerName}`, { method: 'POST', body: formData });
            let result = {};
            try { result = await response.json(); } catch { result = { success: response.ok }; }

            if (!response.ok || (result.hasOwnProperty('success') && !result.success)) {
                const errorList = document.getElementById('errorList');
                if (errorList) errorList.innerHTML = `<li>${result.message || 'Ошибка операции'}</li>`;
                showModal(this.errorModal);
            } else {
                hideModal(modalToClose);
                await this.loadData();
                this.renderTable();
            }
        } catch (err) { console.error(err); } finally { toggleLoader(false); }
    }
}

document.addEventListener('DOMContentLoaded', () => {
    const page = new FurniturePage();
    page.init();
});