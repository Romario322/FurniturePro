import { syncTable } from '../api/syncService.js';
import { getAll } from '../db/repository.js';
import { toggleLoader } from '../ui/loader.js';
import { showModal, hideModal } from '../ui/modal.js';
import { attachRowSelection, updateHeaderSortUI } from '../ui/tables.js';
import { getSearchMode, initSearchModeSwitcher, fillSelect } from '../utils/domHelper.js';
import { paginateData, updatePaginationUI } from '../utils/pagination.js';
import { initCustomSelects } from '../ui/customSelect.js';

class PartsPage {
    constructor() {
        this.cachedParts = [];
        this.cachedMaterials = [];
        this.cachedColors = [];
        this.cachedPrices = [];
        this.currentData = [];

        this.analyticsChart = null;
        this.currentAnalyticsPartId = null;
        this.currentAnalyticsYear = new Date().getFullYear();

        this.currentPage = 1;
        this.perPage = 20;
        this.sortField = null;
        this.sortDirection = 'asc';

        this.initDOM();
        this.attachEvents();
    }

    initDOM() {
        this.table = document.getElementById('partsTable');
        this.tbody = this.table?.querySelector('tbody');
        this.headers = this.table?.querySelectorAll('thead th');

        this.searchInput = document.getElementById('searchInput');
        this.searchButton = document.getElementById('searchButton');
        this.clearSearchTextBtn = document.getElementById('clearSearchText');

        this.activityFilter = document.getElementById('activityFilter');
        this.materialFilter = document.getElementById('materialFilter');
        this.colorFilter = document.getElementById('colorFilter');
        this.createPartButton = document.getElementById('createPartButton');
        this.partsCount = document.getElementById('partsCount');

        this.createModal = document.getElementById('createModal');
        this.editModal = document.getElementById('editModal');
        this.deleteModal = document.getElementById('deleteModal');
        this.descriptionModal = document.getElementById('descriptionModal');
        this.errorModal = document.getElementById('errorModal');
        this.priceModal = document.getElementById('priceModal');
        this.analyticsModal = document.getElementById('analyticsModal');

        this.analyticsPrevYearBtn = document.getElementById('analyticsPrevYear');
        this.analyticsNextYearBtn = document.getElementById('analyticsNextYear');
        this.analyticsYearDisplay = document.getElementById('analyticsYearDisplay');

        this.createForm = document.getElementById('createForm');
        this.editForm = document.getElementById('editForm');
        this.deleteForm = document.getElementById('deleteForm');
        this.priceForm = document.getElementById('priceForm');

        this.createMaterialSelect = document.getElementById('createMaterial');
        this.editMaterialSelect = document.getElementById('editMaterial');
        this.createColorSelect = document.getElementById('createColor');
        this.editColorSelect = document.getElementById('editColor');

        this.editIdInput = document.getElementById('editId');
        this.deleteIdInput = document.getElementById('deleteId');
        this.descriptionContent = document.getElementById('descriptionContent');

        this.pricePartIdInput = document.getElementById('pricePartId');
        this.pricePartNameDisplay = document.getElementById('pricePartName');
        this.priceValueInput = document.getElementById('priceValue');
        this.priceDateInput = document.getElementById('priceDate');
        this.priceHistoryList = document.getElementById('priceHistoryList');

        this.prevBtn = document.getElementById('prev');
        this.nextBtn = document.getElementById('next');
        this.pageInfo = document.getElementById('pageInfo');
    }

    async init() {
        try {
            toggleLoader(true);
            initSearchModeSwitcher(() => this.renderTable());
            attachRowSelection('#partsTable tbody');
            this.initAutoDecimalCorrection();

            await this.loadData();
            this.initFiltersFromData();
            initCustomSelects();
            this.renderTable();
        } finally {
            toggleLoader(false);
        }
        this.startAutoSync();
    }

    async loadData() {
        await Promise.all([syncTable('Parts'), syncTable('Materials'), syncTable('Colors'), syncTable('Prices')]);
        const [partsData, materials, colors, prices] = await Promise.all([getAll('Parts'), getAll('Materials'), getAll('Colors'), getAll('Prices')]);
        this.cachedParts = partsData || [];
        this.cachedMaterials = materials || [];
        this.cachedColors = colors || [];
        this.cachedPrices = prices || [];
    }

    initFiltersFromData() {
        fillSelect(this.materialFilter, 'Материал: все записи', this.cachedMaterials);
        fillSelect(this.colorFilter, 'Цвет: все записи', this.cachedColors);
        fillSelect(this.createMaterialSelect, 'Выберите материал', this.cachedMaterials);
        fillSelect(this.editMaterialSelect, 'Выберите материал', this.cachedMaterials);
        fillSelect(this.createColorSelect, 'Выберите цвет', this.cachedColors);
        fillSelect(this.editColorSelect, 'Выберите цвет', this.cachedColors);

        if (this.activityFilter && this.activityFilter.options.length <= 1) {
            this.activityFilter.innerHTML = '';
            const allOpt = document.createElement('option'); allOpt.value = ''; allOpt.textContent = 'Активность: все детали';
            const activeOpt = document.createElement('option'); activeOpt.value = 'active'; activeOpt.textContent = 'Только активные детали';
            const inactiveOpt = document.createElement('option'); inactiveOpt.value = 'inactive'; inactiveOpt.textContent = 'Только неактивные детали';
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
                th.style.cursor = 'pointer';
                th.addEventListener('click', () => this.onHeaderClick(index));
            });
        }

        const triggerRender = () => { this.currentPage = 1; this.renderTable(); };

        if (this.searchButton) this.searchButton.addEventListener('click', triggerRender);
        if (this.searchInput) this.searchInput.addEventListener('keyup', (e) => { if (e.key === 'Enter') triggerRender(); });
        if (this.clearSearchTextBtn) this.clearSearchTextBtn.addEventListener('click', () => { this.searchInput.value = ''; triggerRender(); });

        if (this.activityFilter) this.activityFilter.addEventListener('change', triggerRender);
        if (this.materialFilter) this.materialFilter.addEventListener('change', triggerRender);
        if (this.colorFilter) this.colorFilter.addEventListener('change', triggerRender);

        if (this.createPartButton) {
            this.createPartButton.addEventListener('click', () => {
                if (this.createForm) { initCustomSelects(); this.createForm.reset(); }
                const activeCheck = document.getElementById('createIsActive');
                if (activeCheck) activeCheck.checked = true;
                showModal(this.createModal);
            });
        }

        if (this.prevBtn) this.prevBtn.addEventListener('click', () => { if (this.currentPage > 1) { this.currentPage--; this.renderTable(); } });
        if (this.nextBtn) this.nextBtn.addEventListener('click', () => {
            const totalPages = Math.ceil(this.currentData.length / this.perPage);
            if (this.currentPage < totalPages) { this.currentPage++; this.renderTable(); }
        });

        if (this.analyticsPrevYearBtn) this.analyticsPrevYearBtn.addEventListener('click', () => this.changeAnalyticsYear(-1));
        if (this.analyticsNextYearBtn) this.analyticsNextYearBtn.addEventListener('click', () => this.changeAnalyticsYear(1));

        if (this.createForm) this.createForm.addEventListener('submit', (e) => this.handleFormSubmit(e, 'CreatePart', this.createModal));
        if (this.editForm) this.editForm.addEventListener('submit', (e) => this.handleFormSubmit(e, 'UpdatePart', this.editModal));
        if (this.deleteForm) this.deleteForm.addEventListener('submit', (e) => this.handleFormSubmit(e, 'DeletePart', this.deleteModal));
        if (this.priceForm) this.priceForm.addEventListener('submit', (e) => this.handleFormSubmit(e, 'CreatePrice', this.priceModal));
    }

    onHeaderClick(index) {
        const fieldMap = ['name', 'material', 'color', 'size', 'weight', 'activity', 'price', 'description', 'actions'];
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

        const search = (this.searchInput?.value || '').toLowerCase().trim();
        const activity = this.activityFilter?.value;
        const material = this.materialFilter?.value;
        const color = this.colorFilter?.value;

        this.tbody.innerHTML = '';

        const checkStaticFilters = (p) => {
            if (activity === 'active' && !p.activity) return false;
            if (activity === 'inactive' && p.activity) return false;
            const materialId = material ? Number(material) : null;
            const colorId = color ? Number(color) : null;
            if (materialId !== null && p.materialId !== materialId) return false;
            if (colorId !== null && p.colorId !== colorId) return false;
            return true;
        };

        let finded_parts = this.cachedParts.filter(p => {
            if (search) {
                const haystack = [p.name, p.description, p.length, p.width, p.height, p.diameter, p.weight].map(v => (v ?? '').toString().toLowerCase()).join(' ');
                if (!haystack.includes(search)) return false;
            }
            return checkStaticFilters(p);
        });

        const mode = getSearchMode();
        let isHighlight = false;

        if (mode === 'filter') {
            this.currentData = finded_parts;
        } else if (mode === 'highlight') {
            this.currentData = this.cachedParts.filter(checkStaticFilters);
            isHighlight = true;
        }

        if (this.partsCount) {
            const total = this.cachedParts.length;
            const filtered = finded_parts.length;
            this.partsCount.textContent = mode === 'filter'
                ? `Деталей: ${filtered} из ${total}`
                : `Деталей: ${this.currentData.length} (совпадений: ${filtered}) из ${total}`;
        }

        const priceMap = new Map();
        const sortedPrices = [...this.cachedPrices].sort((a, b) => new Date(b.date) - new Date(a.date));
        sortedPrices.forEach(p => { if (!priceMap.has(p.partId)) priceMap.set(p.partId, p.value); });

        const materialById = new Map(this.cachedMaterials.map(m => [m.id, m]));
        const colorById = new Map(this.cachedColors.map(c => [c.id, c]));

        if (this.currentData.length > 0 && this.sortField) {
            this.currentData.sort((a, b) => {
                const dir = this.sortDirection === 'asc' ? 1 : -1;
                const getValue = (item) => {
                    switch (this.sortField) {
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

        this.currentPage = updatePaginationUI(this.currentPage, this.currentData.length, this.perPage, this.pageInfo, this.prevBtn, this.nextBtn);

        if (this.currentData.length === 0) {
            this.tbody.innerHTML = '<tr><td colspan="9" class="text-center">Записей нет</td></tr>';
            return;
        }

        const pageParts = paginateData(this.currentData, this.currentPage, this.perPage);
        const fragment = document.createDocumentFragment();

        for (const item of pageParts) {
            const tr = document.createElement('tr');
            if (isHighlight && finded_parts.includes(item)) tr.classList.add('highlight-match');

            const tdName = document.createElement('td'); tdName.textContent = item.name;
            const tdMaterial = document.createElement('td'); tdMaterial.textContent = materialById.get(item.materialId)?.name || '—';
            const tdColor = document.createElement('td'); tdColor.textContent = colorById.get(item.colorId)?.name || '—';

            const tdSize = document.createElement('td');
            tdSize.textContent = (item.diameter && item.diameter > 0) ? `${item.length} мм Ø${item.diameter} мм` : `${item.length}x${item.width}x${item.height} мм`;
            const tdWeight = document.createElement('td'); tdWeight.textContent = item.weight + ' кг';
            const tdActivity = document.createElement('td'); tdActivity.textContent = item.activity ? 'Активна' : 'Деактивирована';

            const currentPrice = priceMap.get(item.id) || 0;
            const tdPrice = document.createElement('td'); tdPrice.classList.add('col-fit');
            const wrapper = document.createElement('div'); wrapper.className = 'price-cell-wrapper justify-content-end';

            const priceText = document.createElement('span'); priceText.className = 'price-value-text me-2';
            priceText.textContent = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' }).format(currentPrice);

            const analyticsBtn = document.createElement('button');
            analyticsBtn.textContent = '📈'; analyticsBtn.title = 'Динамика цен'; analyticsBtn.className = 'btn btn-icon-mini btn-square me-2';
            analyticsBtn.onclick = (e) => { e.stopPropagation(); this.openAnalyticsModal(item); };

            const addPriceBtn = document.createElement('button');
            addPriceBtn.className = 'btn btn-sm btn-success btn-add-price btn-square'; addPriceBtn.title = 'Обновить цену'; addPriceBtn.innerHTML = '+';
            addPriceBtn.onclick = (e) => {
                e.stopPropagation();
                const history = this.cachedPrices.filter(p => p.partId === item.id).sort((a, b) => new Date(b.date) - new Date(a.date));
                this.openPriceModal(item, history);
            };

            wrapper.append(priceText, analyticsBtn, addPriceBtn);
            tdPrice.appendChild(wrapper);

            const tdDescription = document.createElement('td'); tdDescription.classList.add('col-fit');
            const descBtn = document.createElement('button');
            descBtn.textContent = '📄'; descBtn.title = 'Описание'; descBtn.className = 'btn btn-sm btn-description btn-square';
            descBtn.onclick = () => { if (this.descriptionContent) this.descriptionContent.textContent = item.description || 'Описания нет'; showModal(this.descriptionModal); };
            tdDescription.appendChild(descBtn);

            const tdActions = document.createElement('td'); tdActions.classList.add('col-fit');
            const actionsDiv = document.createElement('div'); actionsDiv.className = 'actions-group justify-content-end';

            const editBtn = document.createElement('button');
            editBtn.textContent = '✎'; editBtn.title = 'Изменить'; editBtn.className = 'btn btn-sm btn-primary btn-square';
            editBtn.onclick = () => this.openEditModal(item);

            const deleteBtn = document.createElement('button');
            deleteBtn.textContent = '🗑'; deleteBtn.title = 'Удалить'; deleteBtn.className = 'btn btn-sm btn-danger btn-square';
            deleteBtn.onclick = () => this.openDeleteModal(item);

            actionsDiv.append(editBtn, deleteBtn);
            tdActions.appendChild(actionsDiv);

            tr.append(tdName, tdMaterial, tdColor, tdSize, tdWeight, tdActivity, tdPrice, tdDescription, tdActions);
            fragment.appendChild(tr);
        }
        this.tbody.appendChild(fragment);
    }

    openEditModal(item) {
        if (this.editIdInput) this.editIdInput.value = item.id;
        document.getElementById('editName').value = item.name || '';
        document.getElementById('editLength').value = item.length ?? '';
        document.getElementById('editWidth').value = item.width ?? '';
        document.getElementById('editHeight').value = item.height ?? '';
        document.getElementById('editDiameter').value = item.diameter ?? '';
        document.getElementById('editWeight').value = item.weight ?? '';
        document.getElementById('editIsActive').checked = item.activity;
        document.getElementById('editDescription').value = item.description || '';
        if (this.editMaterialSelect) this.editMaterialSelect.value = item.materialId ?? '';
        if (this.editColorSelect) this.editColorSelect.value = item.colorId ?? '';
        initCustomSelects();
        showModal(this.editModal);
    }

    openDeleteModal(item) {
        if (this.deleteIdInput) this.deleteIdInput.value = item.id;
        showModal(this.deleteModal);
    }

    openPriceModal(part, history) {
        const historyContainer = document.getElementById('priceHistoryContainer');
        if (this.pricePartIdInput) this.pricePartIdInput.value = part.id;
        if (this.pricePartNameDisplay) this.pricePartNameDisplay.textContent = part.name;
        if (this.priceForm) this.priceForm.reset();

        const now = new Date();
        now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
        if (this.priceDateInput) {
            this.priceDateInput.value = now.toISOString().slice(0, 16);
            if (history && history.length > 0) {
                const prevDate = new Date(history[0].date);
                prevDate.setMinutes(prevDate.getMinutes() - prevDate.getTimezoneOffset());
                this.priceDateInput.min = prevDate.toISOString().slice(0, 16);
            } else {
                this.priceDateInput.removeAttribute('min');
            }
        }

        if (historyContainer) {
            historyContainer.innerHTML = '';
            if (history && history.length > 0) {
                history.slice(0, 10).forEach(h => {
                    const card = document.createElement('div'); card.className = 'price-history-card';
                    card.innerHTML = `<div class="price-history-date">${new Date(h.date).toLocaleDateString()}</div><div class="price-history-val">${h.value} ₽</div>`;
                    historyContainer.appendChild(card);
                });
            } else {
                historyContainer.innerHTML = '<div class="price-history-empty">История цен пуста</div>';
            }
        }
        showModal(this.priceModal);
    }

    openAnalyticsModal(part) {
        this.currentAnalyticsPartId = part.id;
        this.currentAnalyticsYear = new Date().getFullYear();
        this.renderAnalytics(part);
        showModal(this.analyticsModal);
    }

    changeAnalyticsYear(delta) {
        const part = this.cachedParts.find(p => p.id === this.currentAnalyticsPartId);
        if (!part) return;

        const allHistory = this.cachedPrices.filter(p => p.partId === part.id).sort((a, b) => new Date(a.date) - new Date(b.date));
        const minYear = allHistory.length > 0 ? new Date(allHistory[0].date).getFullYear() : new Date().getFullYear();
        const maxYear = new Date().getFullYear();

        const newYear = this.currentAnalyticsYear + delta;
        if (newYear < minYear || newYear > maxYear) return;

        this.currentAnalyticsYear = newYear;
        this.renderAnalytics(part);
    }

    renderAnalytics(part) {
        const nameDisplay = document.getElementById('analyticsPartName');
        const priceDisplay = document.getElementById('analyticsCurrentPrice');
        const minDisplay = document.getElementById('analyticsMinPrice');
        const maxDisplay = document.getElementById('analyticsMaxPrice');

        if (nameDisplay) nameDisplay.textContent = part.name;
        if (this.analyticsYearDisplay) this.analyticsYearDisplay.textContent = this.currentAnalyticsYear;

        const allHistory = this.cachedPrices.filter(p => p.partId === part.id).sort((a, b) => new Date(a.date) - new Date(b.date));
        const currentPrice = allHistory.length > 0 ? allHistory[allHistory.length - 1].value : 0;

        const fmt = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' });
        if (priceDisplay) priceDisplay.textContent = fmt.format(currentPrice);

        if (allHistory.length > 0) {
            const vals = allHistory.map(h => h.value);
            if (minDisplay) minDisplay.textContent = fmt.format(Math.min(...vals));
            if (maxDisplay) maxDisplay.textContent = fmt.format(Math.max(...vals));
        } else {
            if (minDisplay) minDisplay.textContent = "0 ₽";
            if (maxDisplay) maxDisplay.textContent = "0 ₽";
        }

        const minYear = allHistory.length > 0 ? new Date(allHistory[0].date).getFullYear() : new Date().getFullYear();
        const realCurrentYear = new Date().getFullYear();
        if (this.analyticsPrevYearBtn) this.analyticsPrevYearBtn.disabled = this.currentAnalyticsYear <= minYear;
        if (this.analyticsNextYearBtn) this.analyticsNextYearBtn.disabled = this.currentAnalyticsYear >= realCurrentYear;

        const startOfYearDate = new Date(this.currentAnalyticsYear, 0, 1);
        const endOfYearDate = new Date(this.currentAnalyticsYear, 11, 31, 23, 59, 59);
        const now = new Date();
        const lineEndDate = this.currentAnalyticsYear === now.getFullYear() ? now : endOfYearDate;

        const previousPrices = allHistory.filter(h => new Date(h.date) < startOfYearDate);
        let startPrice = previousPrices.length > 0 ? previousPrices[previousPrices.length - 1].value : null;

        const seriesData = [];
        if (startPrice !== null) seriesData.push([startOfYearDate.getTime(), startPrice]);

        allHistory.filter(h => new Date(h.date).getFullYear() === this.currentAnalyticsYear).forEach(h => {
            seriesData.push([new Date(h.date).getTime(), h.value]);
        });

        if (seriesData.length > 0) {
            seriesData.push([lineEndDate.getTime(), seriesData[seriesData.length - 1][1]]);
        }

        const getCssVar = (name) => getComputedStyle(document.documentElement).getPropertyValue(name).trim();
        const colAccent = getCssVar('--accent-secondary') || '#e27b32';
        const colMuted = getCssVar('--text-muted') || '#8a6540';
        const colGrid = getCssVar('--warm-divider') || 'rgba(125, 94, 60, 0.4)';
        const colMin = getCssVar('--action-create') || '#1f5a35';
        const colMax = getCssVar('--danger-main') || '#500805';

        let annotationsY = [];
        if (allHistory.length > 0) {
            const vals = allHistory.map(h => h.value);
            annotationsY = [
                { y: Math.min(...vals), borderColor: colMin, layer: 'back', strokeDashArray: 4 },
                { y: Math.max(...vals), borderColor: colMax, layer: 'back', strokeDashArray: 4 }
            ];
        }

        if (this.analyticsChart) this.analyticsChart.destroy();

        const options = {
            colors: [colAccent],
            series: [{ name: "Цена", data: seriesData }],
            annotations: { yaxis: annotationsY },
            chart: {
                type: 'line', height: 300, toolbar: { show: false }, zoom: { enabled: false }, fontFamily: 'inherit',
                locales: [{ "name": "ru", "options": { "months": ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], "shortMonths": ["Янв", "Фев", "Мар", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек"] } }],
                defaultLocale: "ru"
            },
            stroke: { curve: 'stepline', width: 3 },
            grid: { borderColor: colGrid, strokeDashArray: 4, padding: { top: 25, bottom: 25, left: 10, right: 10 } },
            dataLabels: { enabled: false },
            xaxis: { type: 'datetime', min: startOfYearDate.getTime(), max: endOfYearDate.getTime(), tooltip: { enabled: false }, labels: { datetimeFormatter: { year: 'yyyy', month: 'MMM', day: 'dd MMM' }, style: { colors: colMuted } }, axisBorder: { show: false }, axisTicks: { show: false } },
            yaxis: { labels: { formatter: (val) => val.toFixed(0) + " ₽", style: { colors: colMuted } } },
            fill: { type: 'gradient', gradient: { shade: 'dark', gradientToColors: [colAccent], shadeIntensity: 1, type: 'horizontal', opacityFrom: 0.9, opacityTo: 0.9, stops: [0, 100] } },
            markers: { size: 4, colors: ['#fff'], strokeColors: colAccent, strokeWidth: 2, hover: { size: 6 } },
            tooltip: { x: { format: 'dd MMM yyyy HH:mm' }, y: { formatter: (val) => val + " ₽" }, marker: { show: true, fillColors: [colAccent] }, theme: 'false', cssClass: 'apexcharts-tooltip-custom' },
            noData: { text: 'Нет данных за этот период', align: 'center', verticalAlign: 'middle', style: { color: colMuted, fontSize: '16px' } }
        };

        this.analyticsChart = new ApexCharts(document.querySelector("#priceChart"), options);
        this.analyticsChart.render();
    }

    initAutoDecimalCorrection() {
        const ids = ['createLength', 'createWidth', 'createHeight', 'createDiameter', 'createWeight', 'editLength', 'editWidth', 'editHeight', 'editDiameter', 'editWeight', 'priceValue'];
        ids.forEach(id => {
            const el = document.getElementById(id);
            if (el) el.addEventListener('input', function () { if (this.value.includes(',')) this.value = this.value.replace(/,/g, '.'); });
        });
    }

    validatePartForm(prefix) {
        const errors = [];
        const name = document.getElementById(prefix + 'Name');
        const material = document.getElementById(prefix + 'Material');
        const color = document.getElementById(prefix + 'Color');
        const length = document.getElementById(prefix + 'Length');
        const width = document.getElementById(prefix + 'Width');
        const height = document.getElementById(prefix + 'Height');
        const diameter = document.getElementById(prefix + 'Diameter');
        const weight = document.getElementById(prefix + 'Weight');

        if (!name?.value.trim()) errors.push('Поле "Название" обязательно.');
        if (!material?.value) errors.push('Поле "Материал" обязательно.');
        if (!color?.value) errors.push('Поле "Цвет" обязательно.');
        if (!length?.value || Number(length.value) <= 0) errors.push('Поле "Длина" должно быть больше 0.');
        if (!weight?.value || Number(weight.value) <= 0) errors.push('Поле "Вес" должно быть больше 0.');

        const w = Number(width?.value), h = Number(height?.value), d = Number(diameter?.value);
        if ((!w || w <= 0 || !h || h <= 0) && (!d || d <= 0)) {
            errors.push('Необходимо указать корректные габариты (Диаметр или Ширина+Высота).');
        }

        if (errors.length > 0) {
            const errorList = document.getElementById('errorList');
            if (errorList) { errorList.innerHTML = ''; errors.forEach(msg => { const li = document.createElement('li'); li.textContent = msg; errorList.appendChild(li); }); }
            showModal(this.errorModal); return false;
        }
        return true;
    }

    validatePriceAddForm() {
        const errors = [];
        const valInput = document.getElementById('priceValue');
        const dateInput = document.getElementById('priceDate');

        if (!valInput?.value || Number(valInput.value) <= 0) errors.push('Поле "Стоимость" должно быть больше 0.');
        if (!dateInput?.value) errors.push('Поле "Дата установки" обязательно.');
        else if (dateInput.min && dateInput.value < dateInput.min) errors.push(`Дата не может быть раньше предыдущей записи.`);

        if (errors.length > 0) {
            const errorList = document.getElementById('errorList');
            if (errorList) { errorList.innerHTML = ''; errors.forEach(msg => { const li = document.createElement('li'); li.textContent = msg; errorList.appendChild(li); }); }
            showModal(this.errorModal); return false;
        }
        return true;
    }

    async handleFormSubmit(e, handlerName, modalToClose) {
        e.preventDefault();
        if (e.target.id === 'priceForm') { if (!this.validatePriceAddForm()) return; }
        else if (e.target.id !== 'deleteForm') { const formPrefix = e.target.id.replace('Form', ''); if (!this.validatePartForm(formPrefix)) return; }

        try {
            toggleLoader(true);
            const formData = new FormData(e.target);
            const response = await fetch(`?handler=${handlerName}`, { method: 'POST', body: formData });
            let result = {};
            try { result = await response.json(); } catch { result = { success: response.ok }; }

            if (!response.ok || (result.hasOwnProperty('success') && !result.success)) {
                const errorList = document.getElementById('errorList');
                if (errorList) errorList.innerHTML = `<li>${result.message || 'Ошибка операции'}</li>`;
                showModal(this.errorModal);
            } else {
                hideModal(modalToClose);
                if (handlerName === 'CreatePrice') {
                    await syncTable('Prices');
                    this.cachedPrices = await getAll('Prices') || [];
                } else {
                    await this.loadData();
                }
                this.renderTable();
            }
        } finally {
            toggleLoader(false);
        }
    }
}

document.addEventListener('DOMContentLoaded', () => {
    const page = new PartsPage();
    page.init();
});