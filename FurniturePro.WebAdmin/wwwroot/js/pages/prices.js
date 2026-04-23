import { syncTable } from '../api/syncService.js';
import { getAll } from '../db/repository.js';
import { toggleLoader } from '../ui/loader.js';
import { showModal, hideModal } from '../ui/modals.js';
import { attachRowSelection, updateHeaderSortUI } from '../ui/tables.js';
import { fillSelect } from '../utils/domHelper.js';
import { paginateData, updatePaginationUI } from '../utils/pagination.js';

class PricesPage {
    constructor() {
        this.cachedPrices = [];
        this.cachedParts = [];
        this.latestPriceIds = new Set();

        // Лимиты редактирования (7 дней)
        this.EDIT_LIMIT_MS = 7 * 24 * 60 * 60 * 1000;
        this.CURRENT_TIME = new Date().getTime();

        this.currentPage = 1;
        this.perPage = 20;
        this.sortField = 'date';
        this.sortDirection = 'desc';

        // Инстансы Flatpickr
        this.fpDateFrom = null;
        this.fpDateTo = null;
        this.fpEditDate = null;

        this.initDOM();
        this.attachEvents();
    }

    initDOM() {
        this.table = document.getElementById('pricesTable');
        this.tbody = this.table?.querySelector('tbody');
        this.headers = this.table?.querySelectorAll('thead th');

        this.searchInput = document.getElementById('searchInput');
        this.clearSearchTextBtn = document.getElementById('clearSearchText');
        this.clearSearchRangeBtn = document.getElementById('clearSearchRange');

        this.searchPriceMin = document.getElementById('searchPriceMin');
        this.searchPriceMax = document.getElementById('searchPriceMax');
        this.searchContainerSingle = document.getElementById('searchContainerSingle');
        this.searchContainerRange = document.getElementById('searchContainerRange');

        this.partFilter = document.getElementById('partFilter');
        this.actualityFilter = document.getElementById('actualityFilter');
        this.dateFromFilter = document.getElementById('dateFromFilter');
        this.dateToFilter = document.getElementById('dateToFilter');
        this.pricesCount = document.getElementById('pricesCount');

        this.pageInfo = document.getElementById('pageInfo');
        this.prevBtn = document.getElementById('prev');
        this.nextBtn = document.getElementById('next');

        this.editModal = document.getElementById('editModal');
        this.deleteModal = document.getElementById('deleteModal');
        this.errorModal = document.getElementById('errorModal');

        this.editForm = document.getElementById('editForm');
        this.deleteForm = document.getElementById('deleteForm');
        this.editIdInput = document.getElementById('editId');
        this.editPartIdInput = document.getElementById('editPartId');
        this.deleteIdInput = document.getElementById('deleteId');
        this.editPartNameDisplay = document.getElementById('editPartNameDisplay');
        this.editDateInput = document.getElementById('editDate');
        this.editValueInput = document.getElementById('editValue');
    }

    async init() {
        try {
            toggleLoader(true);

            this.initDatePickers();
            attachRowSelection('#pricesTable tbody');
            this.initAutoDecimalCorrection();
            this.initSearchMethodLogic();
            this.initViewModeLogic();

            await this.loadData();
            this.initFilters();

            this.recalculateLatestPermissions();
            this.renderTable();
        } finally {
            toggleLoader(false);
        }
        this.startAutoSync();
    }

    initDatePickers() {
        const commonConfig = {
            locale: "ru",
            dateFormat: "Y-m-d",
            altInput: true,
            altFormat: "d.m.Y",
            disableMobile: "true",
            allowInput: true
        };

        if (this.dateFromFilter) {
            this.fpDateFrom = flatpickr(this.dateFromFilter, {
                ...commonConfig,
                onChange: (selectedDates, dateStr) => {
                    if (this.fpDateTo) this.fpDateTo.set("minDate", dateStr);
                    this.currentPage = 1;
                    this.renderTable();
                }
            });
        }

        if (this.dateToFilter) {
            this.fpDateTo = flatpickr(this.dateToFilter, {
                ...commonConfig,
                onChange: (selectedDates, dateStr) => {
                    if (this.fpDateFrom) this.fpDateFrom.set("maxDate", dateStr);
                    this.currentPage = 1;
                    this.renderTable();
                }
            });
        }

        if (this.editDateInput) {
            this.fpEditDate = flatpickr(this.editDateInput, {
                locale: "ru",
                enableTime: true,
                time_24hr: true,
                dateFormat: "Y-m-dTH:i",
                altInput: true,
                altFormat: "d.m.Y H:i",
                disableMobile: "true",
                allowInput: true
            });
        }
    }

    initAutoDecimalCorrection() {
        const decimalInputs = [this.searchInput, this.searchPriceMin, this.searchPriceMax, this.editValueInput];
        decimalInputs.forEach(el => {
            if (el) {
                el.addEventListener('input', function () {
                    if (this.value.includes(',')) this.value = this.value.replace(/,/g, '.');
                });
            }
        });
    }

    initSearchMethodLogic() {
        const radios = document.querySelectorAll('input[name="searchMethod"]');
        radios.forEach(radio => {
            radio.addEventListener('change', (e) => {
                const method = e.target.value;
                if (method === 'range') {
                    if (this.searchContainerSingle) this.searchContainerSingle.style.display = 'none';
                    if (this.searchContainerRange) this.searchContainerRange.style.display = 'flex';
                } else {
                    if (this.searchContainerSingle) this.searchContainerSingle.style.display = 'flex';
                    if (this.searchContainerRange) this.searchContainerRange.style.display = 'none';
                }
                this.currentPage = 1;
                this.renderTable();
            });
        });
    }

    initViewModeLogic() {
        const radios = document.querySelectorAll('input[name="viewMode"]');
        radios.forEach(radio => {
            radio.addEventListener('change', () => this.renderTable());
        });
    }

    getSearchMethod() {
        const checked = document.querySelector('input[name="searchMethod"]:checked');
        return checked ? checked.value : 'start';
    }

    getViewMode() {
        const checked = document.querySelector('input[name="viewMode"]:checked');
        return checked ? checked.value : 'filter';
    }

    async loadData() {
        await Promise.all([syncTable('Prices'), syncTable('Parts')]);
        const [pricesData, partsData] = await Promise.all([getAll('Prices'), getAll('Parts')]);
        this.cachedPrices = pricesData || [];
        this.cachedParts = partsData || [];
    }

    initFilters() {
        const sortedParts = [...this.cachedParts].sort((a, b) => (a.name || '').localeCompare(b.name || ''));
        if (this.partFilter) fillSelect(this.partFilter, 'Деталь: все записи', sortedParts);
    }

    startAutoSync() {
        const loop = async () => {
            await this.loadData();
            this.recalculateLatestPermissions();
            this.renderTable();
            setTimeout(loop, 5 * 60 * 1000);
        };
        loop();
    }

    recalculateLatestPermissions() {
        this.latestPriceIds.clear();
        const map = new Map();

        this.cachedPrices.forEach(p => {
            const pDate = new Date(p.date).getTime();
            if (!map.has(p.partId) || pDate > map.get(p.partId).date) {
                map.set(p.partId, { id: p.id, date: pDate });
            }
        });

        for (const val of map.values()) {
            this.latestPriceIds.add(val.id);
        }
    }

    attachEvents() {
        if (this.headers) {
            this.headers.forEach((th, index) => {
                th.style.cursor = 'pointer';
                th.addEventListener('click', () => this.onHeaderClick(index));
            });
        }

        const triggerRender = () => { this.currentPage = 1; this.renderTable(); };

        if (this.dateFromFilter) this.dateFromFilter.addEventListener('change', triggerRender);
        if (this.dateToFilter) this.dateToFilter.addEventListener('change', triggerRender);
        if (this.partFilter) this.partFilter.addEventListener('change', triggerRender);
        if (this.actualityFilter) this.actualityFilter.addEventListener('change', triggerRender);

        if (this.searchPriceMin) {
            this.searchPriceMin.addEventListener('change', () => {
                let min = parseFloat(this.searchPriceMin.value);
                let max = parseFloat(this.searchPriceMax.value);
                if (min < 0) { min = 0; this.searchPriceMin.value = 0; }
                if (!isNaN(min) && !isNaN(max) && min > max) { this.searchPriceMax.value = min; }
                triggerRender();
            });
            this.searchPriceMin.addEventListener('keyup', (e) => { if (e.key === 'Enter') triggerRender(); });
        }

        if (this.searchPriceMax) {
            this.searchPriceMax.addEventListener('change', () => {
                let min = parseFloat(this.searchPriceMin.value);
                let max = parseFloat(this.searchPriceMax.value);
                if (max < 0) { max = 0; this.searchPriceMax.value = 0; }
                if (!isNaN(min) && !isNaN(max) && max < min) { this.searchPriceMin.value = max; }
                triggerRender();
            });
            this.searchPriceMax.addEventListener('keyup', (e) => { if (e.key === 'Enter') triggerRender(); });
        }

        if (this.clearSearchTextBtn) {
            this.clearSearchTextBtn.addEventListener('click', () => {
                if (this.searchInput) this.searchInput.value = '';
                triggerRender();
            });
        }

        if (this.clearSearchRangeBtn) {
            this.clearSearchRangeBtn.addEventListener('click', () => {
                if (this.searchPriceMin) this.searchPriceMin.value = '';
                if (this.searchPriceMax) this.searchPriceMax.value = '';
                triggerRender();
            });
        }

        if (this.searchInput) this.searchInput.addEventListener('keyup', (e) => { if (e.key === 'Enter') triggerRender(); });
        if (this.prevBtn) this.prevBtn.addEventListener('click', () => { if (this.currentPage > 1) { this.currentPage--; this.renderTable(); } });
        if (this.nextBtn) {
            this.nextBtn.addEventListener('click', () => {
                const totalPages = Math.ceil(this.currentData.length / this.perPage);
                if (this.currentPage < totalPages) { this.currentPage++; this.renderTable(); }
            });
        }

        if (this.editForm) this.editForm.addEventListener('submit', (e) => this.handleFormSubmit(e, 'UpdatePrice', this.editModal));
        if (this.deleteForm) this.deleteForm.addEventListener('submit', (e) => this.handleFormSubmit(e, 'DeletePrice', this.deleteModal));
    }

    onHeaderClick(index) {
        const fieldMap = ['part', 'value', 'date', 'actions'];
        const field = fieldMap[index];
        if (!field || field === 'actions') return;

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

        let searchVal = (this.searchInput?.value || '').trim();
        if (searchVal.includes(',')) searchVal = searchVal.replace(',', '.');

        const method = this.getSearchMethod();
        const viewMode = this.getViewMode();

        let minPrice = parseFloat(this.searchPriceMin?.value);
        let maxPrice = parseFloat(this.searchPriceMax?.value);
        if (isNaN(minPrice)) minPrice = null;
        if (isNaN(maxPrice)) maxPrice = null;

        const filterPartId = this.partFilter?.value ? Number(this.partFilter.value) : null;
        const filterActuality = this.actualityFilter?.value;
        const dateFrom = this.dateFromFilter?.value ? new Date(this.dateFromFilter.value) : null;
        const dateTo = this.dateToFilter?.value ? new Date(this.dateToFilter.value) : null;
        if (dateTo) dateTo.setHours(23, 59, 59, 999);

        this.tbody.innerHTML = '';

        const checkStaticFilters = (p) => {
            if (filterPartId !== null && p.partId !== filterPartId) return false;
            if (p.date) {
                const pDate = new Date(p.date);
                if (dateFrom && pDate < dateFrom) return false;
                if (dateTo && pDate > dateTo) return false;
            }
            if (filterActuality === 'actual' && !this.latestPriceIds.has(p.id)) return false;
            if (filterActuality === 'archived' && this.latestPriceIds.has(p.id)) return false;
            return true;
        };

        const checkSearch = (p) => {
            if (method !== 'range' && !searchVal) return true;
            if (method === 'range' && minPrice === null && maxPrice === null) return true;

            const val = Number(p.value);
            if (method === 'range') {
                if (minPrice !== null && val < minPrice) return false;
                if (maxPrice !== null && val > maxPrice) return false;
                return true;
            } else {
                const strVal = p.value.toString();
                return method === 'exact' ? val === Number(searchVal) : strVal.startsWith(searchVal);
            }
        };

        let matches = [];
        let displayList = [];

        const staticallyFiltered = this.cachedPrices.filter(checkStaticFilters);

        if (viewMode === 'filter') {
            displayList = staticallyFiltered.filter(p => {
                const isMatch = checkSearch(p);
                if (isMatch) matches.push(p);
                return isMatch;
            });
        } else {
            displayList = staticallyFiltered;
            matches = displayList.filter(checkSearch);
        }

        this.currentData = displayList;

        if (this.pricesCount) {
            const total = this.cachedPrices.length;
            const filtered = matches.length;
            this.pricesCount.textContent = viewMode === 'filter'
                ? `Цен: ${filtered} из ${total}`
                : `Цен: ${this.currentData.length} (совпадений: ${filtered}) из ${total}`;
        }

        if (this.currentData.length > 0) {
            const partsMap = new Map(this.cachedParts.map(pt => [pt.id, pt]));

            this.currentData.sort((a, b) => {
                const dir = this.sortDirection === 'asc' ? 1 : -1;
                let va, vb;
                switch (this.sortField) {
                    case 'part':
                        va = (partsMap.get(a.partId)?.name || '').toLowerCase();
                        vb = (partsMap.get(b.partId)?.name || '').toLowerCase();
                        break;
                    case 'value':
                        va = Number(a.value);
                        vb = Number(b.value);
                        break;
                    case 'date':
                        va = new Date(a.date).getTime();
                        vb = new Date(b.date).getTime();
                        break;
                    default: return 0;
                }
                return va < vb ? -1 * dir : (va > vb ? 1 * dir : 0);
            });
        }

        this.currentPage = updatePaginationUI(this.currentPage, this.currentData.length, this.perPage, this.pageInfo, this.prevBtn, this.nextBtn);

        if (this.currentData.length === 0) {
            this.tbody.innerHTML = '<tr><td colspan="4" class="text-center" style="font-style:italic; color:var(--text-muted);">Записей не найдено</td></tr>';
            return;
        }

        const pageItems = paginateData(this.currentData, this.currentPage, this.perPage);
        const partsMap = new Map(this.cachedParts.map(pt => [pt.id, pt]));
        const fragment = document.createDocumentFragment();
        const currencyFmt = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' });
        const dateFmt = new Intl.DateTimeFormat('ru-RU', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' });

        const matchesSet = new Set(matches.map(m => m.id));

        for (const item of pageItems) {
            const tr = document.createElement('tr');

            if (viewMode === 'highlight' && matchesSet.has(item.id)) {
                tr.classList.add('highlight-match');
            }

            const tdPart = document.createElement('td');
            const part = partsMap.get(item.partId);
            tdPart.textContent = part ? part.name : `ID: ${item.partId} (Удалена)`;
            if (!part) tdPart.style.color = 'var(--danger-main)';

            const tdValue = document.createElement('td');
            tdValue.textContent = currencyFmt.format(item.value);
            tdValue.style.fontWeight = '600';

            const tdDate = document.createElement('td');
            const d = new Date(item.date);
            tdDate.textContent = dateFmt.format(d);

            const tdActions = document.createElement('td');
            tdActions.classList.add('col-fit');

            const isLatest = this.latestPriceIds.has(item.id);
            const priceDateMs = new Date(item.date).getTime();
            const isEditable = (this.CURRENT_TIME - priceDateMs) < this.EDIT_LIMIT_MS;

            if (isLatest && isEditable) {
                const actionsDiv = document.createElement('div');
                actionsDiv.className = 'actions-group justify-content-end';

                const editBtn = document.createElement('button');
                editBtn.textContent = '✎';
                editBtn.title = 'Изменить';
                editBtn.className = 'btn btn-sm btn-primary btn-square';
                editBtn.addEventListener('click', () => this.openEditModal(item));

                const deleteBtn = document.createElement('button');
                deleteBtn.textContent = '🗑';
                deleteBtn.title = 'Удалить';
                deleteBtn.className = 'btn btn-sm btn-danger btn-square';
                deleteBtn.addEventListener('click', () => this.openDeleteModal(item));

                actionsDiv.appendChild(editBtn);
                actionsDiv.appendChild(deleteBtn);
                tdActions.appendChild(actionsDiv);
            } else {
                const lockSpan = document.createElement('span');
                lockSpan.textContent = '🔒';
                lockSpan.title = !isLatest ? 'Архивная запись (есть более свежая цена)' : 'Лимит редактирования (7 дней) истек';
                lockSpan.style.fontSize = '1.2rem';
                lockSpan.style.opacity = '0.6';
                lockSpan.style.cursor = 'help';

                const centerDiv = document.createElement('div');
                centerDiv.className = 'd-flex justify-content-center w-100';
                centerDiv.appendChild(lockSpan);
                tdActions.appendChild(centerDiv);
            }

            tr.appendChild(tdPart);
            tr.appendChild(tdValue);
            tr.appendChild(tdDate);
            tr.appendChild(tdActions);
            fragment.appendChild(tr);
        }
        this.tbody.appendChild(fragment);
    }

    openEditModal(item) {
        const priceDateMs = new Date(item.date).getTime();
        if ((this.CURRENT_TIME - priceDateMs) >= this.EDIT_LIMIT_MS) {
            alert('Ошибка: Редактирование этой записи невозможно, так как прошло более 7 дней с даты установки цены.');
            return;
        }

        if (this.editIdInput) this.editIdInput.value = item.id;
        if (this.editPartIdInput) this.editPartIdInput.value = item.partId;

        const part = this.cachedParts.find(p => p.id === item.partId);
        if (this.editPartNameDisplay) this.editPartNameDisplay.textContent = part ? part.name : 'Неизвестная деталь';
        if (this.editValueInput) this.editValueInput.value = item.value;

        const d = new Date(item.date);
        d.setMinutes(d.getMinutes() - d.getTimezoneOffset());
        const isoDate = d.toISOString().slice(0, 16);

        if (this.fpEditDate) {
            this.fpEditDate.setDate(isoDate);

            const history = this.cachedPrices
                .filter(p => p.partId === item.partId && p.id !== item.id)
                .sort((a, b) => new Date(b.date) - new Date(a.date));

            if (history.length > 0) {
                this.fpEditDate.set("minDate", new Date(history[0].date));
            } else {
                this.fpEditDate.set("minDate", null);
            }
        }

        showModal(this.editModal);
    }

    openDeleteModal(item) {
        const priceDateMs = new Date(item.date).getTime();
        if ((this.CURRENT_TIME - priceDateMs) >= this.EDIT_LIMIT_MS) {
            alert('Ошибка: Удаление этой записи невозможно, так как прошло более 7 дней с даты установки цены.');
            return;
        }

        if (this.deleteIdInput) this.deleteIdInput.value = item.id;
        showModal(this.deleteModal);
    }

    validatePriceForm() {
        const errors = [];

        if (!this.editValueInput?.value) {
            errors.push('Поле "Стоимость" обязательно.');
        } else if (Number(this.editValueInput.value) <= 0) {
            errors.push('Поле "Стоимость" должно быть больше 0.');
        }

        if (!this.editDateInput?.value) {
            errors.push('Поле "Дата установки" обязательно.');
        } else if (this.editDateInput.min && this.editDateInput.value < this.editDateInput.min) {
            const minDatePretty = new Date(this.editDateInput.min).toLocaleString('ru-RU');
            errors.push(`Дата не может быть раньше предыдущей записи (${minDatePretty}).`);
        }

        const editedPriceId = this.editIdInput?.value;
        const originalPrice = this.cachedPrices.find(p => p.id == editedPriceId);
        if (originalPrice) {
            const originalPriceDateMs = new Date(originalPrice.date).getTime();
            if ((this.CURRENT_TIME - originalPriceDateMs) >= this.EDIT_LIMIT_MS) {
                errors.push('Лимит редактирования (7 дней) истек. Изменение цены невозможно.');
            }
        }

        if (errors.length > 0) {
            const errorList = document.getElementById('errorList');
            if (errorList) {
                errorList.innerHTML = '';
                errors.forEach(msg => {
                    const li = document.createElement('li');
                    li.textContent = msg;
                    errorList.appendChild(li);
                });
            }
            showModal(this.errorModal);
            return false;
        }
        return true;
    }

    async handleFormSubmit(e, handlerName, modalToClose) {
        e.preventDefault();

        if (e.target.id === 'editForm' && !this.validatePriceForm()) return;

        if (e.target.id === 'deleteForm') {
            const deletePriceId = this.deleteIdInput?.value;
            const priceToDelete = this.cachedPrices.find(p => p.id == deletePriceId);
            if (priceToDelete) {
                const priceDateMs = new Date(priceToDelete.date).getTime();
                if ((this.CURRENT_TIME - priceDateMs) >= this.EDIT_LIMIT_MS) {
                    alert('Ошибка: Удаление этой записи невозможно, лимит редактирования (7 дней) истек.');
                    return;
                }
            }
        }

        try {
            toggleLoader(true);
            const formData = new FormData(e.target);
            const response = await fetch(`?handler=${handlerName}`, { method: 'POST', body: formData });
            const result = await response.json();

            if (!result.success) {
                const errorList = document.getElementById('errorList');
                if (errorList) errorList.innerHTML = `<li>${result.message || 'Ошибка операции'}</li>`;
                showModal(this.errorModal);
            } else {
                hideModal(modalToClose);
                await this.loadData();
                this.recalculateLatestPermissions();
                this.renderTable();
            }
        } catch (ex) {
            console.error(ex);
            const errorList = document.getElementById('errorList');
            if (errorList) errorList.innerHTML = `<li>Произошла системная ошибка</li>`;
            showModal(this.errorModal);
        } finally {
            toggleLoader(false);
        }
    }
}

document.addEventListener('DOMContentLoaded', () => {
    const page = new PricesPage();
    page.init();
});