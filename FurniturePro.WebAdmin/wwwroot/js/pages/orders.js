import { syncTable } from '../api/syncService.js';
import { getAll, clearStore } from '../db/repository.js';
import { toggleLoader } from '../ui/loader.js';
import { showModal, hideModal } from '../ui/modals.js';
import { attachRowSelection, updateHeaderSortUI } from '../ui/tables.js';
import { getSearchMode, initSearchModeSwitcher, fillSelect } from '../utils/domHelper.js';
import { paginateData, updatePaginationUI } from '../utils/pagination.js';
import { initCustomSelects } from '../ui/customSelect.js';
import { copyToClipboard } from '../utils/clipboard.js';

class OrdersPage {
    constructor() {
        this.STATUS_FLOW = [
            "Новый", "В обработке", "Ожидает оплаты", "Оплачен",
            "В производстве", "Готов к отгрузке", "Отгружен", "Доставлен"
        ];

        // Кэш данных
        this.cachedOrders = [];
        this.cachedClients = [];
        this.cachedStatuses = [];
        this.cachedFurniture = [];
        this.cachedParts = [];
        this.cachedPrices = [];
        this.cachedStatusChanges = [];
        this.cachedOrderCompositions = [];
        this.cachedFurnitureCompositions = [];

        // View Model
        this.cachedOrdersViewData = [];
        this.currentData = [];

        // Состояние редактора композиции
        this.currentOrderCompositionDraft = [];
        this.currentEditingOrderId = null;

        // Пагинация и сортировка
        this.currentPage = 1;
        this.perPage = 20;
        this.sortField = null;
        this.sortDirection = 'desc';

        // Flatpickr
        this.fpCreateDate = null;
        this.fpChangeStatusDate = null;

        this.initDOM();
        this.attachEvents();
    }

    initDOM() {
        this.table = document.getElementById('ordersTable');
        this.tbody = this.table?.querySelector('tbody');
        this.headers = this.table?.querySelectorAll('thead th');

        this.searchInput = document.getElementById('searchInput');
        this.searchButton = document.getElementById('searchButton');
        this.clearSearchTextBtn = document.getElementById('clearSearchText');

        this.statusFilter = document.getElementById('statusFilter');
        this.clientFilter = document.getElementById('clientFilter');
        this.groupFilter = document.getElementById('groupFilter');

        this.createOrderButton = document.getElementById('createOrderButton');
        this.ordersCount = document.getElementById('ordersCount');
        this.pageInfo = document.getElementById('pageInfo');
        this.prevBtn = document.getElementById('prev');
        this.nextBtn = document.getElementById('next');

        this.contactsModal = document.getElementById('contactsModal');
        this.compositionModal = document.getElementById('compositionModal');
        this.historyModal = document.getElementById('historyModal');
        this.changeStatusModal = document.getElementById('changeStatusModal');
        this.editOrderCompositionModal = document.getElementById('editOrderCompositionModal');

        this.createModal = document.getElementById('createModal');
        this.editModal = document.getElementById('editModal');
        this.deleteModal = document.getElementById('deleteModal');
        this.errorModal = document.getElementById('errorModal');

        this.createForm = document.getElementById('createForm');
        this.createClientIdSelect = document.getElementById('createClientId');
        this.editForm = document.getElementById('editForm');
        this.deleteForm = document.getElementById('deleteForm');
        this.deleteIdInput = document.getElementById('deleteId');

        this.contactsContent = document.getElementById('contactsContent');
        this.compositionContent = document.getElementById('compositionContent');
        this.historyContent = document.getElementById('historyContent');

        this.changeStatusForm = document.getElementById('changeStatusForm');
        this.newStatusSelect = document.getElementById('newStatusSelect');
        this.currentStatusDisplay = document.getElementById('currentStatusDisplay');
        this.changeStatusOrderId = document.getElementById('changeStatusOrderId');
        this.changeStatusDate = document.getElementById('changeStatusDate');

        this.furnitureSearchInput = document.getElementById('furnitureSearchInput');
        this.clearFurnitureSearchBtn = document.getElementById('clearFurnitureSearch');
        this.saveCompositionBtn = document.getElementById('saveCompositionBtn');
        this.currentOrderCompTableBody = document.querySelector('#currentOrderCompTable tbody');
        this.availableFurnitureTableBody = document.querySelector('#availableFurnitureTable tbody');
        this.headerOrderTotalCountLabel = document.getElementById('headerOrderTotalCount');
        this.summaryOrderPriceLabel = document.getElementById('summaryOrderPrice');
        this.fModeFilter = document.getElementById('fModeFilter');
        this.fModeHighlight = document.getElementById('fModeHighlight');
    }

    async init() {
        try {
            toggleLoader(true);
            this.initOrderPickers();
            initSearchModeSwitcher(() => this.renderTable());
            attachRowSelection('#ordersTable tbody');
            this.attachDiscountInputLimits();

            await this.loadData();
            this.initFiltersFromData();
            initCustomSelects();

            this.renderTable();
        } catch (e) {
            console.error("Init error:", e);
        } finally {
            toggleLoader(false);
        }
    }

    initOrderPickers() {
        const commonConfig = { locale: "ru", enableTime: true, time_24hr: true, dateFormat: "Y-m-dTH:i", altInput: true, altFormat: "d.m.Y H:i", disableMobile: "true", allowInput: true };
        if (document.getElementById('createDate')) this.fpCreateDate = flatpickr("#createDate", commonConfig);
        if (document.getElementById('changeStatusDate')) this.fpChangeStatusDate = flatpickr("#changeStatusDate", commonConfig);
    }

    async loadData() {
        await Promise.all([
            syncTable('Orders'), syncTable('Clients'), syncTable('Statuses'),
            syncTable('StatusChanges'), syncTable('OrderCompositions'),
            syncTable('Furniture'), syncTable('FurnitureCompositions'),
            syncTable('Parts'), syncTable('Prices')
        ]);

        const [dOrders, dClients, dStatuses, dStChanges, dOrdComps, dFurn, dFurnComps, dParts, dPrices] = await Promise.all([
            getAll('Orders'), getAll('Clients'), getAll('Statuses'),
            getAll('StatusChanges'), getAll('OrderCompositions'),
            getAll('Furniture'), getAll('FurnitureCompositions'),
            getAll('Parts'), getAll('Prices')
        ]);

        this.cachedOrders = dOrders || [];
        this.cachedClients = dClients || [];
        this.cachedStatuses = dStatuses || [];
        this.cachedStatusChanges = dStChanges || [];
        this.cachedOrderCompositions = dOrdComps || [];
        this.cachedFurniture = dFurn || [];
        this.cachedFurnitureCompositions = dFurnComps || [];
        this.cachedParts = dParts || [];
        this.cachedPrices = dPrices || [];

        if (this.createClientIdSelect) {
            const clientList = this.cachedClients.map(c => ({ id: c.id, name: c.fullName || 'Без имени' })).sort((a, b) => a.name.localeCompare(b.name));
            fillSelect(this.createClientIdSelect, 'Выберите клиента', clientList);
        }

        this.recalcOrdersViewData();
    }

    initFiltersFromData() {
        fillSelect(this.statusFilter, 'Статус: все', this.cachedStatuses);
        const clientList = this.cachedClients.map(c => ({ id: c.id, name: c.fullName || 'Без имени' })).sort((a, b) => a.name.localeCompare(b.name));
        fillSelect(this.clientFilter, 'Клиент: все', clientList);
    }

    recalcOrdersViewData() {
        const statusMap = new Map();
        this.cachedStatusChanges.forEach(sc => {
            if (!statusMap.has(sc.idOrder)) statusMap.set(sc.idOrder, []);
            statusMap.get(sc.idOrder).push(sc);
        });

        const compositionMap = new Map();
        this.cachedOrderCompositions.forEach(oc => {
            if (!compositionMap.has(oc.idOrder)) compositionMap.set(oc.idOrder, []);
            compositionMap.get(oc.idOrder).push(oc);
        });

        const clientMap = new Map(this.cachedClients.map(c => [c.id, c]));

        this.cachedOrdersViewData = this.cachedOrders.map(order => {
            const client = clientMap.get(order.clientId);
            const changes = statusMap.get(order.id) || [];
            const currentSt = this.getCurrentStatus(changes);
            const comps = compositionMap.get(order.id) || [];

            let rawSum = 0;
            comps.forEach(oc => { rawSum += this.calculateFurniturePrice(oc.idFurniture) * oc.count; });
            const discount = order.discount || 0;
            const totalSum = rawSum * (1 - discount / 100);

            return {
                ...order,
                _clientName: client ? client.fullName : 'Неизвестный клиент',
                _statusObj: currentSt,
                _statusName: currentSt.name,
                _creationDate: this.getCreationDate(changes) || new Date(0),
                _rawSum: rawSum,
                _totalSum: totalSum,
                _compositions: comps,
                _changes: changes
            };
        });
    }

    getStatusStyle(statusName) {
        if (!statusName) return { bg: 'var(--status-default-bg)', color: 'var(--status-default-text)' };
        const lower = statusName.toLowerCase();
        if (lower.includes('отменен')) return { bg: 'var(--status-danger-bg)', color: 'var(--status-danger-text)' };
        if (lower.includes('пауз') || lower.includes('ожидает оплаты')) return { bg: 'var(--status-warning-bg)', color: 'var(--status-warning-text)' };
        if (lower.includes('доставлен') || lower.includes('оплачен') || lower.includes('готов')) return { bg: 'var(--status-success-bg)', color: 'var(--status-success-text)' };
        if (lower === 'новый') return { bg: 'var(--status-new-bg)', color: 'var(--status-new-text)' };
        return { bg: 'var(--status-process-bg)', color: 'var(--status-process-text)' };
    }

    isOrderLocked(statusName) {
        if (!statusName) return false;
        const lower = statusName.toLowerCase();
        const lockedKeywords = ['оплачен', 'производств', 'готов', 'отгружен', 'доставлен', 'отменен', 'пауз', 'ожидает оплаты'];
        return lockedKeywords.some(keyword => lower.includes(keyword));
    }

    getSortedStatusChanges(changesList) {
        if (!changesList || !Array.isArray(changesList) || changesList.length === 0) return [];
        return [...changesList].sort((a, b) => new Date(a.date) - new Date(b.date));
    }

    getCurrentStatus(changesList) {
        const sorted = this.getSortedStatusChanges(changesList);
        if (sorted.length === 0) return { name: 'Не определен', id: 0, date: null };
        const last = sorted[sorted.length - 1];
        const status = this.cachedStatuses.find(s => s.id === last.idStatus);
        return { name: status ? status.name : '?', id: last.idStatus, date: last.date };
    }

    getEffectiveStatusName(changesList) {
        const sorted = this.getSortedStatusChanges(changesList);
        for (let i = sorted.length - 1; i >= 0; i--) {
            const st = this.cachedStatuses.find(s => s.id === sorted[i].idStatus);
            if (st && !st.name.toLowerCase().includes('пауз')) return st.name;
        }
        return "Новый";
    }

    getAllowedNextStatuses(changesList) {
        const currentSt = this.getCurrentStatus(changesList);
        if (!currentSt.name) return [];

        const currentName = currentSt.name.trim();
        const currentLower = currentName.toLowerCase();

        if (currentLower.includes('отменен') || currentLower.includes('доставлен')) return [];

        const allowedNames = [];
        let baseName = currentName;
        if (currentLower.includes('пауз')) baseName = this.getEffectiveStatusName(changesList);

        const flowIndex = this.STATUS_FLOW.findIndex(s => s.toLowerCase() === baseName.toLowerCase());
        const paidIndex = this.STATUS_FLOW.findIndex(s => s.toLowerCase() === 'оплачен');

        if (flowIndex !== -1 && flowIndex < this.STATUS_FLOW.length - 1) allowedNames.push(this.STATUS_FLOW[flowIndex + 1]);
        if (currentLower.includes('пауз') && flowIndex !== -1) allowedNames.push(this.STATUS_FLOW[flowIndex]);
        if (!currentLower.includes('пауз')) allowedNames.push("На паузе");
        if (paidIndex !== -1 && flowIndex < paidIndex) allowedNames.push("Отменен");

        const uniqueNames = [...new Set(allowedNames)];
        const result = [];
        uniqueNames.forEach(targetName => {
            const found = this.cachedStatuses.find(s => s.name.trim().toLowerCase() === targetName.trim().toLowerCase());
            if (found) result.push(found);
        });
        return result;
    }

    getCreationDate(changesList) {
        const sorted = this.getSortedStatusChanges(changesList);
        if (sorted.length === 0) return null;
        return new Date(sorted[0].date);
    }

    calculateFurniturePrice(furnitureId) {
        const furniture = this.cachedFurniture.find(f => f.id === furnitureId);
        if (!furniture) return 0;
        const comps = this.cachedFurnitureCompositions.filter(c => (c.idFurniture === furnitureId) || (c.entity1Id === furnitureId));
        let selfPrice = 0;
        const sortedPrices = [...this.cachedPrices].sort((a, b) => new Date(b.date) - new Date(a.date));
        comps.forEach(comp => {
            const pId = comp.idPart ?? comp.entity2Id;
            const priceObj = sortedPrices.find(p => p.partId === pId);
            selfPrice += (priceObj ? priceObj.value : 0) * (comp.count ?? 0);
        });
        return selfPrice + (selfPrice * (furniture.markup ?? 0) / 100);
    }

    attachEvents() {
        if (this.headers) {
            this.headers.forEach((th, index) => {
                if ([0, 1, 2, 3, 4, 5].includes(index)) {
                    th.style.cursor = 'pointer';
                    th.addEventListener('click', () => this.onHeaderClick(index));
                }
            });
        }

        const triggerRender = () => { this.currentPage = 1; this.renderTable(); };
        if (this.searchButton) this.searchButton.addEventListener('click', triggerRender);
        if (this.searchInput) this.searchInput.addEventListener('keyup', (e) => { if (e.key === 'Enter') triggerRender(); });
        if (this.clearSearchTextBtn) this.clearSearchTextBtn.addEventListener('click', () => { this.searchInput.value = ''; triggerRender(); });
        if (this.statusFilter) this.statusFilter.addEventListener('change', triggerRender);
        if (this.clientFilter) this.clientFilter.addEventListener('change', triggerRender);
        if (this.groupFilter) this.groupFilter.addEventListener('change', triggerRender);

        if (this.prevBtn) this.prevBtn.addEventListener('click', () => { if (this.currentPage > 1) { this.currentPage--; this.renderTable(); } });
        if (this.nextBtn) this.nextBtn.addEventListener('click', () => {
            const totalPages = Math.ceil(this.currentData.length / this.perPage);
            if (this.currentPage < totalPages) { this.currentPage++; this.renderTable(); }
        });

        if (this.createOrderButton) {
            this.createOrderButton.addEventListener('click', () => {
                if (this.createForm) this.createForm.reset();
                if (this.fpCreateDate) this.fpCreateDate.setDate(new Date());
                const newStatus = this.cachedStatuses.find(s => s.name.trim().toLowerCase() === 'новый');
                const statusIdInput = document.getElementById('createStatusId');
                if (statusIdInput) statusIdInput.value = newStatus ? newStatus.id : 1;
                initCustomSelects();
                showModal(this.createModal);
            });
        }

        if (this.createForm) this.createForm.addEventListener('submit', (e) => this.handleOrderFormSubmit(e, 'CreateOrder', this.createModal));
        if (this.editForm) this.editForm.addEventListener('submit', (e) => this.handleOrderFormSubmit(e, 'UpdateOrder', this.editModal));
        if (this.deleteForm) this.deleteForm.addEventListener('submit', (e) => this.handleOrderFormSubmit(e, 'DeleteOrder', this.deleteModal));
        if (this.changeStatusForm) this.changeStatusForm.addEventListener('submit', (e) => this.handleStatusChangeSubmit(e));

        if (this.furnitureSearchInput) this.furnitureSearchInput.addEventListener('input', () => this.renderAvailableFurnitureTable());
        if (this.fModeFilter) this.fModeFilter.addEventListener('change', () => this.renderAvailableFurnitureTable());
        if (this.fModeHighlight) this.fModeHighlight.addEventListener('change', () => this.renderAvailableFurnitureTable());
        if (this.clearFurnitureSearchBtn) this.clearFurnitureSearchBtn.addEventListener('click', () => {
            this.furnitureSearchInput.value = ''; this.furnitureSearchInput.focus(); this.renderAvailableFurnitureTable();
        });
        if (this.saveCompositionBtn) this.saveCompositionBtn.addEventListener('click', () => this.handleCompositionSave());
    }

    onHeaderClick(index) {
        const fieldMap = { 0: 'client', 1: 'date', 2: 'status', 3: 'raw', 4: 'discount', 5: 'total' };
        const field = fieldMap[index];
        if (!field) return;

        if (this.sortField === field) {
            this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
        } else {
            this.sortField = field;
            this.sortDirection = (field === 'date' || field === 'total' || field === 'raw' || field === 'discount') ? 'desc' : 'asc';
        }

        updateHeaderSortUI(this.headers, index, this.sortDirection);
        this.currentPage = 1;
        this.renderTable();
    }

    renderTable() {
        if (!this.tbody) return;

        const search = (this.searchInput?.value || '').toLowerCase().trim();
        const fStatus = this.statusFilter?.value ? Number(this.statusFilter.value) : null;
        const fClient = this.clientFilter?.value ? Number(this.clientFilter.value) : null;
        const fGroup = this.groupFilter?.value;

        this.tbody.innerHTML = '';

        const checkFilters = (item) => {
            if (search && !item._clientName.toLowerCase().includes(search)) return false;
            if (fClient !== null && item.clientId !== fClient) return false;
            if (fStatus !== null && item._statusObj.id !== fStatus) return false;

            const stNameLower = item._statusName.toLowerCase();
            if (fGroup === 'cancelled' && !stNameLower.includes('отменен')) return false;
            if (fGroup === 'success' && !stNameLower.includes('доставлен')) return false;
            if (fGroup === 'action' && (!stNameLower.includes('новый') && !stNameLower.includes('обработке'))) return false;
            if (fGroup === 'process') {
                if (stNameLower.includes('отменен') || stNameLower.includes('доставлен') || stNameLower.includes('новый') || stNameLower.includes('обработке')) return false;
            }
            return true;
        };

        const mode = getSearchMode();
        const matches = this.cachedOrdersViewData.filter(checkFilters);
        let itemsToRender = [];
        let isHighlight = false;

        if (mode === 'filter') {
            itemsToRender = matches;
        } else {
            itemsToRender = this.cachedOrdersViewData.filter(item => {
                if (fClient !== null && item.clientId !== fClient) return false;
                if (fStatus !== null && item._statusObj.id !== fStatus) return false;
                return true;
            });
            isHighlight = true;
        }

        this.currentData = itemsToRender;

        if (this.ordersCount) {
            this.ordersCount.textContent = mode === 'filter' ? `Записей: ${matches.length}` : `Записей: ${itemsToRender.length} (совпадений: ${matches.length})`;
        }

        if (this.sortField) {
            itemsToRender.sort((a, b) => {
                const dir = this.sortDirection === 'asc' ? 1 : -1;
                let va, vb;
                switch (this.sortField) {
                    case 'client': va = a._clientName.toLowerCase(); vb = b._clientName.toLowerCase(); break;
                    case 'date': va = a._creationDate; vb = b._creationDate; break;
                    case 'status': va = a._statusName; vb = b._statusName; break;
                    case 'total': va = a._totalSum; vb = b._totalSum; break;
                    case 'raw': va = a._rawSum; vb = b._rawSum; break;
                    case 'discount': va = a.discount || 0; vb = b.discount || 0; break;
                    default: return 0;
                }
                return va < vb ? -1 * dir : (va > vb ? 1 * dir : 0);
            });
        }

        this.currentPage = updatePaginationUI(this.currentPage, this.currentData.length, this.perPage, this.pageInfo, this.prevBtn, this.nextBtn);

        if (itemsToRender.length === 0) {
            this.tbody.innerHTML = '<tr><td colspan="8" class="text-center" style="color: var(--app-text); opacity: 0.6; font-style:italic; padding: 20px;">Заказов нет</td></tr>';
            return;
        }

        const pageItems = paginateData(itemsToRender, this.currentPage, this.perPage);
        const fragment = document.createDocumentFragment();
        const currencyFmt = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' });
        const dateFmt = new Intl.DateTimeFormat('ru-RU', { day: '2-digit', month: '2-digit', year: 'numeric' });

        for (const item of pageItems) {
            const tr = document.createElement('tr');
            if (isHighlight && matches.includes(item)) tr.classList.add('highlight-match');

            const tdClient = document.createElement('td');
            const clientDiv = document.createElement('div');
            clientDiv.className = 'd-flex align-items-center justify-content-between gap-2';
            const nameSpan = document.createElement('span');
            nameSpan.textContent = item._clientName; nameSpan.style.fontWeight = '500';

            const contactBtn = document.createElement('button');
            contactBtn.innerHTML = '📄'; contactBtn.title = 'Контакты клиента'; contactBtn.className = 'btn btn-sm btn-contacts btn-square';
            contactBtn.onclick = (e) => { e.stopPropagation(); this.openContactsModal(item.clientId); };

            clientDiv.append(nameSpan, contactBtn);
            tdClient.appendChild(clientDiv);

            const tdDate = document.createElement('td');
            tdDate.textContent = item._creationDate.getTime() > 0 ? dateFmt.format(item._creationDate) : '—';

            const tdStatus = document.createElement('td');
            const statusDiv = document.createElement('div'); statusDiv.className = 'd-flex align-items-center justify-content-between gap-1';
            const stName = item._statusName;
            const badge = document.createElement('span'); badge.className = 'badge';
            const style = this.getStatusStyle(stName);
            badge.style.backgroundColor = style.bg; badge.style.color = style.color; badge.textContent = stName;

            const btnGroup = document.createElement('div'); btnGroup.className = 'd-flex gap-1';
            const historyBtn = document.createElement('button');
            historyBtn.innerHTML = '🕒'; historyBtn.title = 'Просмотр истории статусов'; historyBtn.className = 'btn btn-sm btn-history btn-square';
            historyBtn.onclick = (e) => { e.stopPropagation(); this.openHistoryModal(item._changes); };

            const changeStatusBtn = document.createElement('button');
            changeStatusBtn.innerHTML = '⇄'; changeStatusBtn.title = 'Изменить статус'; changeStatusBtn.className = 'btn btn-sm btn-change-status btn-square';

            const allowedNext = this.getAllowedNextStatuses(item._changes);
            if (allowedNext.length === 0) changeStatusBtn.disabled = true;
            else changeStatusBtn.onclick = (e) => { e.stopPropagation(); this.openChangeStatusModal(item, item._changes); };

            btnGroup.append(historyBtn, changeStatusBtn);
            statusDiv.append(badge, btnGroup);
            tdStatus.appendChild(statusDiv);

            const tdRaw = document.createElement('td'); tdRaw.textContent = currencyFmt.format(item._rawSum);
            const tdDisc = document.createElement('td');
            const discount = item.discount || 0;
            tdDisc.textContent = discount > 0 ? `-${discount}%` : '—';
            if (discount > 0) tdDisc.classList.add('text-success', 'fw-bold');

            const tdTotal = document.createElement('td'); tdTotal.textContent = currencyFmt.format(item._totalSum); tdTotal.classList.add('fw-bold');

            const tdComp = document.createElement('td'); tdComp.className = 'col-fit';
            const comps = item._compositions || [];
            if (comps.length > 0) {
                const compBtn = document.createElement('button');
                compBtn.innerHTML = '<span>📦</span>'; compBtn.title = `Просмотр состава (${comps.length} поз.)`; compBtn.className = 'btn btn-sm btn-composition btn-square';
                compBtn.onclick = (e) => { e.stopPropagation(); this.openOrderCompositionModal(comps, item); };
                tdComp.appendChild(compBtn);
            } else {
                tdComp.innerHTML = '<span style="color: var(--warm-border); opacity: 0.6; font-weight: 500;">—</span>';
            }

            const tdActions = document.createElement('td'); tdActions.className = 'col-fit';
            if (this.isOrderLocked(stName)) {
                const lockSpan = document.createElement('span'); lockSpan.innerHTML = '🔒'; lockSpan.title = `Изменение запрещено (Статус: ${stName})`;
                lockSpan.style.fontSize = '1.2rem'; lockSpan.style.opacity = '0.6';
                const centerDiv = document.createElement('div'); centerDiv.className = 'd-flex justify-content-center w-100'; centerDiv.appendChild(lockSpan);
                tdActions.appendChild(centerDiv);
            } else {
                const actionsDiv = document.createElement('div'); actionsDiv.className = 'actions-group justify-content-end';

                const configBtn = document.createElement('button'); configBtn.textContent = '⚙'; configBtn.title = 'Редактировать состав'; configBtn.className = 'btn btn-sm btn-outline-secondary btn-square';
                configBtn.onclick = () => this.openEditOrderCompositionModal(item);

                const editBtn = document.createElement('button'); editBtn.textContent = '✎'; editBtn.title = 'Редактировать заказ'; editBtn.className = 'btn btn-sm btn-primary btn-square';
                editBtn.onclick = () => this.openEditModal(item);

                const delBtn = document.createElement('button'); delBtn.textContent = '🗑'; delBtn.title = 'Удалить'; delBtn.className = 'btn btn-sm btn-danger btn-square';
                delBtn.onclick = () => this.openDeleteModal(item);

                actionsDiv.append(configBtn, editBtn, delBtn);
                tdActions.appendChild(actionsDiv);
            }

            tr.append(tdClient, tdDate, tdStatus, tdRaw, tdDisc, tdTotal, tdComp, tdActions);
            fragment.appendChild(tr);
        }
        this.tbody.appendChild(fragment);
    }

    attachDiscountInputLimits() {
        ['createDiscount', 'editDiscount'].forEach(id => {
            const el = document.getElementById(id);
            if (!el) return;
            el.addEventListener('input', function () {
                let val = parseFloat(this.value);
                if (val > 100) this.value = 100;
                if (val < 0) this.value = 0;
            });
            el.addEventListener('keydown', function (e) {
                if (e.key === '-' || e.key === 'Subtract') e.preventDefault();
            });
        });
    }

    validateOrderForm(prefix) {
        const errors = [];
        if (prefix === 'create') {
            const client = document.getElementById('createClientId');
            if (!client?.value) errors.push('Необходимо выбрать клиента.');
            const date = document.getElementById('createDate');
            if (!date?.value) errors.push('Необходимо указать дату начала.');
        }

        const discountInput = document.getElementById(prefix + 'Discount');
        if (discountInput?.value !== '') {
            const discountVal = parseFloat(discountInput.value);
            if (isNaN(discountVal)) errors.push('Скидка должна быть числом.');
            else if (discountVal < 0) errors.push('Скидка не может быть отрицательной.');
            else if (discountVal > 100) errors.push('Скидка не может превышать 100%.');
        }

        if (errors.length > 0) {
            const errorList = document.getElementById('errorList');
            if (errorList) {
                errorList.innerHTML = '';
                errors.forEach(msg => { const li = document.createElement('li'); li.textContent = msg; li.style.marginBottom = '0.25rem'; errorList.appendChild(li); });
            }
            showModal(this.errorModal);
            return false;
        }
        return true;
    }

    async handleOrderFormSubmit(e, handlerName, modalToClose) {
        e.preventDefault();
        const formId = e.target.id;
        let prefix = formId === 'createForm' ? 'create' : formId === 'editForm' ? 'edit' : '';
        if (prefix && !this.validateOrderForm(prefix)) return;

        try {
            toggleLoader(true);
            const formData = new FormData(e.target);
            const response = await fetch(`?handler=${handlerName}`, { method: 'POST', body: formData });
            let result = {};
            try { result = await response.json(); } catch { result = { success: response.ok }; }

            if (!response.ok || (result.hasOwnProperty('success') && !result.success)) {
                const errorList = document.getElementById('errorList');
                if (errorList) { errorList.innerHTML = `<li>${result.message || 'Ошибка операции'}</li>`; showModal(this.errorModal); }
                else alert(result.message || 'Ошибка операции');
            } else {
                hideModal(modalToClose);
                if (prefix === 'create') await clearStore('StatusChanges');
                await this.loadData();
                this.renderTable();
            }
        } catch (err) {
            console.error(err);
            const errorList = document.getElementById('errorList');
            if (errorList) { errorList.innerHTML = `<li>Ошибка соединения: ${err.message}</li>`; showModal(this.errorModal); }
        } finally { toggleLoader(false); }
    }

    openEditModal(order) {
        document.getElementById('editId').value = order.id;
        document.getElementById('editClientIdHidden').value = order.clientId;
        document.getElementById('editDateHidden').value = order._creationDate ? order._creationDate.toISOString() : '';
        document.getElementById('editClientNameDisplay').value = order._clientName;

        const dateFmt = new Intl.DateTimeFormat('ru-RU', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' });
        document.getElementById('editDateDisplay').value = order._creationDate.getTime() > 0 ? dateFmt.format(order._creationDate) : '—';
        document.getElementById('editDiscount').value = order.discount || 0;
        showModal(this.editModal);
    }

    openDeleteModal(order) {
        this.deleteIdInput.value = order.id;
        showModal(this.deleteModal);
    }

    openContactsModal(clientId) {
        const client = this.cachedClients.find(c => c.id === clientId);
        if (!this.contactsContent) return;
        this.contactsContent.innerHTML = '';

        if (!client) {
            this.contactsContent.innerHTML = '<div class="text-center p-4 text-muted">Клиент не найден в базе данных</div>';
            showModal(this.contactsModal);
            return;
        }

        const phone = client.phone || '';
        const email = client.email || '';
        const address = client.address || '';

        this.contactsContent.innerHTML = `
            <div class="contact-card-wrapper">
                <div class="contact-header" style="border-bottom: none; padding-bottom: 0.5rem; justify-content: center;">
                    <div class="contact-name text-center" style="font-size: 1.4rem;">${client.fullName}</div>
                </div>
                <div class="contact-details">
                    <div class="contact-row">
                        <div class="contact-icon-box">📞</div>
                        <div class="contact-info">
                            <span class="contact-label">Телефон</span><span class="contact-value" style="user-select: all;">${phone || '<span class="contact-empty">Не указан</span>'}</span>
                        </div>
                        ${phone ? `<button class="btn-copy-action btn-square" data-copy="${phone}" title="Скопировать">📋</button>` : ''}
                    </div>
                    <div class="contact-row">
                        <div class="contact-icon-box">✉️</div>
                        <div class="contact-info">
                            <span class="contact-label">Email</span><span class="contact-value" style="user-select: all;">${email || '<span class="contact-empty">Не указан</span>'}</span>
                        </div>
                        ${email ? `<button class="btn-copy-action btn-square" data-copy="${email}" title="Скопировать">📋</button>` : ''}
                    </div>
                    ${address ? `
                    <div class="contact-row">
                        <div class="contact-icon-box">📍</div>
                        <div class="contact-info"><span class="contact-label">Адрес</span><span class="contact-value" style="white-space: normal;">${address}</span></div>
                        <button class="btn-copy-action btn-square" data-copy="${address}" title="Скопировать">📋</button>
                    </div>` : ''}
                </div>
            </div>`;

        this.contactsContent.querySelectorAll('.btn-copy-action').forEach(btn => {
            btn.addEventListener('click', () => copyToClipboard(btn.dataset.copy, btn));
        });

        showModal(this.contactsModal);
    }

    openHistoryModal(changesList) {
        if (!this.historyContent) return;
        this.historyContent.innerHTML = '';
        this.historyContent.style.maxHeight = '50vh';
        this.historyContent.style.overflowY = 'auto';

        const sorted = this.getSortedStatusChanges(changesList);
        const historyView = [...sorted].reverse();

        if (historyView.length === 0) {
            this.historyContent.innerHTML = '<div class="p-3 text-center" style="color:var(--app-text); opacity:0.6;">История пуста</div>';
            showModal(this.historyModal);
            return;
        }

        const table = document.createElement('table');
        table.className = 'table table-sm table-hover mb-0 admin-table';
        table.innerHTML = `<thead class="table-light"><tr><th style="width: 140px; position: sticky; top: 0; background: var(--table-header-bg); z-index: 2;">Дата</th><th style="position: sticky; top: 0; background: var(--table-header-bg); z-index: 2;">Статус</th></tr></thead><tbody></tbody>`;
        const tbody = table.querySelector('tbody');
        const dateTimeFmt = new Intl.DateTimeFormat('ru-RU', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' });

        historyView.forEach((h, idx) => {
            const stObj = this.cachedStatuses.find(s => s.id === h.idStatus);
            const name = stObj ? stObj.name : `ID: ${h.idStatus}`;
            const tr = document.createElement('tr');
            if (idx === 0) tr.style.backgroundColor = 'rgba(var(--accent-rgb), 0.05)';
            const style = this.getStatusStyle(name);
            const badge = `<span class="badge" style="background-color: ${style.bg}; color: ${style.color};">${name}</span>`;
            tr.innerHTML = `<td style="color: var(--app-text); font-variant-numeric: tabular-nums;">${dateTimeFmt.format(new Date(h.date))}</td><td>${badge}</td>`;
            tbody.appendChild(tr);
        });

        this.historyContent.appendChild(table);
        showModal(this.historyModal);
    }

    openOrderCompositionModal(compsList, order) {
        if (!this.compositionContent) return;
        this.compositionContent.className = ''; this.compositionContent.innerHTML = '';
        this.compositionContent.style.padding = '0'; this.compositionContent.style.border = 'none';
        this.compositionContent.style.background = 'transparent'; this.compositionContent.style.maxHeight = 'none';
        this.compositionContent.style.overflow = 'visible';

        const clientName = this.cachedClients.find(x => x.id === order.clientId)?.fullName || 'Неизвестный клиент';

        const header = document.createElement('div');
        header.className = 'd-flex justify-content-between align-items-center mb-2 px-1';
        header.innerHTML = `<div class="d-flex flex-column"><span class="small text-uppercase fw-bold" style="color: var(--text-muted); letter-spacing: 0.05em; font-size: 0.75rem;">Заказчик:</span><span class="fw-bold" style="color: var(--app-text); font-size: 1.1rem; line-height: 1.2;">${clientName}</span></div><span class="badge badge-theme" style="font-size: 0.85rem;">${compsList.length} позиций</span>`;
        this.compositionContent.appendChild(header);

        if (compsList.length === 0) {
            this.compositionContent.innerHTML += `<div class="d-flex flex-column align-items-center justify-content-center p-4" style="color: var(--app-text);"><span class="fs-4 mb-2">∅</span><span>В заказе нет позиций</span></div>`;
        } else {
            const wrapper = document.createElement('div'); wrapper.className = 'composition-table-wrapper'; wrapper.style.height = '400px'; wrapper.style.marginBottom = '0';
            const scroll = document.createElement('div'); scroll.className = 'composition-table-scroll';
            const tbl = document.createElement('table'); tbl.className = 'table table-sm table-hover align-middle mb-0 admin-table';
            tbl.innerHTML = `<thead class="table-light"><tr><th>Мебель</th><th class="text-end" style="width: 100px;">Цена шт.</th><th class="text-center" style="width: 80px;">Кол-во</th><th class="text-end" style="width: 110px;">Сумма</th></tr></thead>`;

            const tbody = document.createElement('tbody');
            const currencyFmt = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' });
            let totalSum = 0;

            compsList.forEach(c => {
                const furniture = this.cachedFurniture.find(f => f.id === c.idFurniture);
                const price = this.calculateFurniturePrice(c.idFurniture);
                const sum = price * c.count; totalSum += sum;
                const tr = document.createElement('tr');
                tr.innerHTML = `<td>${furniture ? furniture.name : '<span class="text-danger fst-italic">Удалено</span>'}</td><td class="text-end small">${currencyFmt.format(price)}</td><td class="text-center fw-bold" style="color: var(--app-text);">${c.count}</td><td class="text-end fw-bold" style="color: var(--app-text);">${currencyFmt.format(sum)}</td>`;
                tbody.appendChild(tr);
            });

            tbl.appendChild(tbody); scroll.appendChild(tbl); wrapper.appendChild(scroll); this.compositionContent.appendChild(wrapper);

            const discount = order.discount || 0;
            const finalTotal = totalSum * (1 - discount / 100);
            const footer = document.createElement('div'); footer.className = 'd-flex justify-content-end align-items-center mt-2 px-1 small'; footer.style.color = 'var(--app-text)';
            let footerHtml = `<span style="font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; margin-right: 10px; color: var(--text-muted);">Итого:</span>`;
            if (discount > 0) footerHtml += `<span class="text-decoration-line-through me-2">${currencyFmt.format(totalSum)}</span><span class="text-success me-2">(-${discount}%)</span>`;
            footerHtml += `<b class="fs-5">${currencyFmt.format(finalTotal)}</b>`;
            footer.innerHTML = footerHtml; this.compositionContent.appendChild(footer);
        }
        showModal(this.compositionModal);
    }

    openChangeStatusModal(order, changesList) {
        if (this.changeStatusOrderId) this.changeStatusOrderId.value = order.id;
        const currentSt = this.getCurrentStatus(changesList);
        const style = this.getStatusStyle(currentSt.name);

        if (this.currentStatusDisplay) this.currentStatusDisplay.innerHTML = `<span class="badge" style="background-color: ${style.bg}; color: ${style.color}; font-size: 1rem;">${currentSt.name}</span>`;
        if (this.newStatusSelect) {
            this.newStatusSelect.innerHTML = '';
            const allowed = this.getAllowedNextStatuses(changesList);
            if (allowed.length === 0) {
                this.newStatusSelect.innerHTML = '<option disabled selected>Нет доступных переходов</option>';
            } else {
                const placeholder = document.createElement('option'); placeholder.value = ""; placeholder.text = "Выберите новый статус..."; placeholder.disabled = true; placeholder.selected = true;
                this.newStatusSelect.appendChild(placeholder);
                allowed.forEach(st => { const opt = document.createElement('option'); opt.value = st.id; opt.textContent = st.name; this.newStatusSelect.appendChild(opt); });
            }
            initCustomSelects();
        }

        if (this.fpChangeStatusDate) {
            this.fpChangeStatusDate.setDate(new Date());
            const prevDate = currentSt.date ? new Date(currentSt.date) : null;
            this.fpChangeStatusDate.set('minDate', prevDate);
        }

        showModal(this.changeStatusModal);
    }

    async handleStatusChangeSubmit(e) {
        e.preventDefault();
        const orderId = document.getElementById('changeStatusOrderId')?.value;
        const statusId = document.getElementById('newStatusSelect')?.value;

        if (!orderId || !statusId) { alert("Ошибка: Не все поля заполнены корректно."); return; }

        try {
            toggleLoader(true);
            const formData = new FormData(e.target);
            const res = await fetch('?handler=CreateStatusChange', { method: 'POST', body: formData });
            let data = {}; try { data = await res.json(); } catch { }

            if (res.ok && (data.success === undefined || data.success)) {
                hideModal(this.changeStatusModal);
                await clearStore('StatusChanges');
                await this.loadData();
                this.renderTable();
            } else {
                alert(data.message || 'Ошибка при смене статуса');
            }
        } catch (err) { console.error(err); alert('Ошибка соединения'); } finally { toggleLoader(false); }
    }

    openEditOrderCompositionModal(order) {
        this.currentEditingOrderId = order.id;
        const existingComps = this.cachedOrderCompositions.filter(c => c.idOrder === order.id);

        this.currentOrderCompositionDraft = existingComps.map(c => {
            const furn = this.cachedFurniture.find(f => f.id === c.idFurniture);
            return { furnitureId: c.idFurniture, count: c.count, name: furn ? furn.name : 'Удаленная мебель', price: this.calculateFurniturePrice(c.idFurniture) };
        });

        if (this.furnitureSearchInput) this.furnitureSearchInput.value = '';
        if (this.fModeFilter) this.fModeFilter.checked = true;

        this.renderOrderDraftTable();
        this.renderAvailableFurnitureTable();
        showModal(this.editOrderCompositionModal);
    }

    renderOrderDraftTable() {
        if (!this.currentOrderCompTableBody) return;
        this.currentOrderCompTableBody.innerHTML = '';
        let totalCount = 0; let totalSumMoney = 0;
        const currencyFmt = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' });

        this.currentOrderCompositionDraft.forEach((item, index) => {
            totalCount += item.count;
            const itemTotalCost = item.price * item.count;
            totalSumMoney += itemTotalCost;

            const tr = document.createElement('tr');
            const tdName = document.createElement('td'); tdName.textContent = item.name;
            const tdQty = document.createElement('td'); tdQty.className = 'text-center';

            const input = document.createElement('input'); input.type = 'number'; input.value = item.count; input.min = 1; input.className = 'qty-input';
            input.onkeydown = (e) => { if (e.key === '-' || e.key === 'Subtract' || e.key === 'e') e.preventDefault(); };
            input.addEventListener('change', (e) => { let val = parseInt(e.target.value); if (val < 1 || isNaN(val)) { val = 1; e.target.value = 1; } item.count = val; this.renderOrderDraftTable(); });
            tdQty.appendChild(input);

            const tdSum = document.createElement('td'); tdSum.className = 'text-end small fw-bold'; tdSum.textContent = currencyFmt.format(itemTotalCost);
            const tdAct = document.createElement('td'); tdAct.className = 'col-fit';

            const delBtn = document.createElement('button'); delBtn.textContent = '×'; delBtn.className = 'btn btn-sm btn-danger btn-square';
            delBtn.onclick = (e) => { e.stopPropagation(); this.currentOrderCompositionDraft.splice(index, 1); this.renderOrderDraftTable(); this.renderAvailableFurnitureTable(); };
            tdAct.appendChild(delBtn);

            tr.append(tdName, tdQty, tdSum, tdAct);
            this.currentOrderCompTableBody.appendChild(tr);
        });

        if (this.headerOrderTotalCountLabel) this.headerOrderTotalCountLabel.textContent = totalCount + ' шт.';
        if (this.summaryOrderPriceLabel) this.summaryOrderPriceLabel.textContent = currencyFmt.format(totalSumMoney);
        attachRowSelection('#currentOrderCompTable tbody');
    }

    renderAvailableFurnitureTable() {
        if (!this.availableFurnitureTableBody) return;
        this.availableFurnitureTableBody.innerHTML = '';
        const searchText = (this.furnitureSearchInput?.value || '').toLowerCase().trim();
        const mode = document.querySelector('input[name="furnSearchMode"]:checked')?.value || 'filter';
        const addedIds = new Set(this.currentOrderCompositionDraft.map(x => x.furnitureId));

        let furnitureToRender = this.cachedFurniture.filter(f => f.activity === true);
        if (mode === 'filter' && searchText) furnitureToRender = furnitureToRender.filter(f => f.name.toLowerCase().includes(searchText));

        const fragment = document.createDocumentFragment();
        const currencyFmt = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' });

        furnitureToRender.forEach(furn => {
            const isMatch = searchText && furn.name.toLowerCase().includes(searchText);
            const isAdded = addedIds.has(furn.id);
            const tr = document.createElement('tr');
            if (mode === 'highlight' && isMatch) tr.classList.add('highlight-match');
            if (isAdded) tr.style.backgroundColor = 'rgba(237, 217, 183, 0.3)';

            const tdName = document.createElement('td'); tdName.textContent = furn.name;
            const tdPrice = document.createElement('td'); tdPrice.className = 'text-end small';
            const price = this.calculateFurniturePrice(furn.id); tdPrice.textContent = currencyFmt.format(price);

            const tdAction = document.createElement('td'); tdAction.className = 'col-fit text-center';
            if (isAdded) {
                const check = document.createElement('div'); check.className = 'icon-added-check'; check.textContent = '✓'; check.title = 'Уже в заказе';
                tdAction.appendChild(check);
            } else {
                const addBtn = document.createElement('button'); addBtn.textContent = '+'; addBtn.className = 'btn btn-sm btn-success btn-square';
                addBtn.onclick = (e) => { e.stopPropagation(); this.currentOrderCompositionDraft.push({ furnitureId: furn.id, count: 1, name: furn.name, price: price }); this.renderOrderDraftTable(); this.renderAvailableFurnitureTable(); };
                tdAction.appendChild(addBtn);
            }
            tr.append(tdName, tdPrice, tdAction);
            fragment.appendChild(tr);
        });

        this.availableFurnitureTableBody.appendChild(fragment);
        attachRowSelection('#availableFurnitureTable tbody');
    }

    async handleCompositionSave() {
        if (!this.currentEditingOrderId) return;

        try {
            toggleLoader(true);
            const originalComps = this.cachedOrderCompositions.filter(c => c.idOrder === this.currentEditingOrderId);
            const newComps = this.currentOrderCompositionDraft;
            const toCreate = [], toUpdate = [], toDelete = [];

            originalComps.forEach(old => {
                if (!newComps.find(n => n.furnitureId === old.idFurniture)) {
                    toDelete.push({ IdOrder: Number(this.currentEditingOrderId), IdFurniture: Number(old.idFurniture) });
                }
            });

            newComps.forEach(newItem => {
                const oldItem = originalComps.find(o => o.idFurniture === newItem.furnitureId);
                const dto = { IdOrder: Number(this.currentEditingOrderId), IdFurniture: Number(newItem.furnitureId), Count: Number(newItem.count), UpdateDate: new Date().toISOString() };
                if (!oldItem) toCreate.push(dto);
                else if (oldItem.count !== newItem.count) toUpdate.push(dto);
            });

            const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
            const headers = { 'Content-Type': 'application/json', 'RequestVerificationToken': token };

            if (toCreate.length > 0) {
                const res = await fetch('?handler=CreateCompositionRange', { method: 'POST', headers: headers, body: JSON.stringify(toCreate) });
                const data = await res.json(); if (!data.success) throw new Error(data.message || 'Ошибка добавления позиций');
            }
            if (toUpdate.length > 0) {
                const res = await fetch('?handler=UpdateCompositionRange', { method: 'POST', headers: headers, body: JSON.stringify(toUpdate) });
                const data = await res.json(); if (!data.success) throw new Error(data.message || 'Ошибка обновления позиций');
            }
            if (toDelete.length > 0) {
                const res = await fetch('?handler=DeleteCompositionRange', { method: 'POST', headers: headers, body: JSON.stringify(toDelete) });
                const data = await res.json(); if (!data.success) throw new Error(data.message || 'Ошибка удаления позиций');
            }

            await clearStore('OrderCompositions');
            hideModal(this.editOrderCompositionModal);
            await this.loadData();
            this.renderTable();
        } catch (e) {
            console.error(e); alert(e.message || 'Ошибка сохранения состава заказа');
        } finally {
            toggleLoader(false);
        }
    }
}

document.addEventListener('DOMContentLoaded', () => {
    const page = new OrdersPage();
    page.init();
});