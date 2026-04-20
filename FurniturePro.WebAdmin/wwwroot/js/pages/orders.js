// wwwroot/js/pages/orders.js

// --- CONSTANTS ---
const STATUS_FLOW = [
    "Новый",
    "В обработке",
    "Ожидает оплаты",
    "Оплачен",
    "В производстве",
    "Готов к отгрузке",
    "Отгружен",
    "Доставлен"
];

// --- GLOBAL VARIABLES ---
let cached_orders = [];
let cached_clients = [];
let cached_statuses = [];
let cached_furniture = [];
let cached_parts = [];
let cached_prices = [];

let cached_statusChanges = [];
let cached_orderCompositions = [];
let cached_furnitureCompositions = [];

let cached_ordersViewData = [];
let orders = [];

let currentOrderCompositionDraft = [];
let currentEditingOrderId = null;

let currentPage = 1;
const perPage = 20;
let sortField = null;
let sortDirection = 'desc';

// Flatpickr Instances
let fpCreateDate = null;
let fpChangeStatusDate = null;

// --- DOM ELEMENTS: MAIN ---
const searchInput = document.getElementById('searchInput');
const searchButton = document.getElementById('searchButton');
const clearSearchTextBtn = document.getElementById('clearSearchText');

// Filters
const statusFilter = document.getElementById('statusFilter');
const clientFilter = document.getElementById('clientFilter');
const groupFilter = document.getElementById('groupFilter');

const createOrderButton = document.getElementById('createOrderButton');
const ordersCount = document.getElementById('ordersCount');
const pageInfo = document.getElementById('pageInfo');
const prevBtn = document.getElementById('prev');
const nextBtn = document.getElementById('next');

// Modals
const contactsModal = document.getElementById('contactsModal');
const compositionModal = document.getElementById('compositionModal');
const historyModal = document.getElementById('historyModal');
const changeStatusModal = document.getElementById('changeStatusModal');
const editOrderCompositionModal = document.getElementById('editOrderCompositionModal');

// CRUD Modals
const createModal = document.getElementById('createModal');
const editModal = document.getElementById('editModal');
const deleteModal = document.getElementById('deleteModal');
const errorModal = document.getElementById('errorModal');

// Forms & Inputs
const createForm = document.getElementById('createForm');
const createClientIdSelect = document.getElementById('createClientId');
const editForm = document.getElementById('editForm');
const deleteForm = document.getElementById('deleteForm');
const deleteIdInput = document.getElementById('deleteId');

// Modal Contents
const contactsContent = document.getElementById('contactsContent');
const compositionContent = document.getElementById('compositionContent');
const historyContent = document.getElementById('historyContent');

// Change Status Form
const changeStatusForm = document.getElementById('changeStatusForm');
const newStatusSelect = document.getElementById('newStatusSelect');
const currentStatusDisplay = document.getElementById('currentStatusDisplay');
const changeStatusOrderId = document.getElementById('changeStatusOrderId');
const changeStatusDate = document.getElementById('changeStatusDate');

// Composition Editor Elements
const furnitureSearchInput = document.getElementById('furnitureSearchInput');
const clearFurnitureSearchBtn = document.getElementById('clearFurnitureSearch');
const saveCompositionBtn = document.getElementById('saveCompositionBtn');
const currentOrderCompTableBody = document.querySelector('#currentOrderCompTable tbody');
const availableFurnitureTableBody = document.querySelector('#availableFurnitureTable tbody');
const headerOrderTotalCountLabel = document.getElementById('headerOrderTotalCount');
const summaryOrderPriceLabel = document.getElementById('summaryOrderPrice');
const fModeFilter = document.getElementById('fModeFilter');
const fModeHighlight = document.getElementById('fModeHighlight');

// Table
const table = document.getElementById('ordersTable');
const headers = table.querySelectorAll('thead th');

// ==========================================
// 1. INITIALIZATION & DATA FETCHING
// ==========================================

headers.forEach((th, index) => {
    // Indexes: 0:Client, 1:Date, 2:Status, 3:RawSum, 4:Discount, 5:TotalSum
    if ([0, 1, 2, 3, 4, 5].includes(index)) {
        th.style.cursor = 'pointer';
        th.addEventListener('click', () => onHeaderClick(index, th));
    }
});

document.addEventListener('DOMContentLoaded', async () => {
    try {
        toggleLoader(true);

        // Инициализация календарей
        initOrderPickers();

        initSearchModeSwitcher(() => renderTable());
        attachRowSelection('#ordersTable tbody');
        attachDiscountInputLimits(); // Подключаем валидацию ввода чисел

        await initData();

        initFiltersFromData();
        initCustomSelects();

        renderTable();
    } catch (e) {
        console.error("Init error:", e);
    } finally {
        toggleLoader(false);
    }
});

function initOrderPickers() {
    const commonConfig = {
        locale: "ru",
        enableTime: true,      // Включаем время
        time_24hr: true,       // 24 часа
        dateFormat: "Y-m-dTH:i", // Формат для сервера (ISO)
        altInput: true,
        altFormat: "d.m.Y H:i",  // Красивый формат
        disableMobile: "true",   // Стилизация на мобильных
        allowInput: true
    };

    // 1. Создание заказа
    fpCreateDate = flatpickr("#createDate", commonConfig);

    // 2. Смена статуса
    fpChangeStatusDate = flatpickr("#changeStatusDate", commonConfig);
}

async function initData() {
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

    cached_orders = dOrders || [];
    cached_clients = dClients || [];
    cached_statuses = dStatuses || [];
    cached_statusChanges = dStChanges || [];
    cached_orderCompositions = dOrdComps || [];
    cached_furniture = dFurn || [];
    cached_furnitureCompositions = dFurnComps || [];
    cached_parts = dParts || [];
    cached_prices = dPrices || [];

    // Заполнение селекта в модалке создания (здесь, чтобы данные были свежими)
    if (createClientIdSelect) {
        const clientList = cached_clients.map(c => ({
            id: c.id,
            name: c.fullName || 'Без имени'
        })).sort((a, b) => a.name.localeCompare(b.name));
        fillSelect(createClientIdSelect, 'Выберите клиента', clientList);
    }

    recalcOrdersViewData();
}

function initFiltersFromData() {
    fillSelect(statusFilter, 'Статус: все', cached_statuses);

    const clientList = cached_clients.map(c => ({
        id: c.id,
        name: c.fullName || 'Без имени'
    })).sort((a, b) => a.name.localeCompare(b.name));

    fillSelect(clientFilter, 'Клиент: все', clientList);
}

// --- VIEW MODEL CALCULATION ---
function recalcOrdersViewData() {
    const statusMap = new Map();
    cached_statusChanges.forEach(sc => {
        if (!statusMap.has(sc.idOrder)) statusMap.set(sc.idOrder, []);
        statusMap.get(sc.idOrder).push(sc);
    });

    const compositionMap = new Map();
    cached_orderCompositions.forEach(oc => {
        if (!compositionMap.has(oc.idOrder)) compositionMap.set(oc.idOrder, []);
        compositionMap.get(oc.idOrder).push(oc);
    });

    const clientMap = new Map(cached_clients.map(c => [c.id, c]));

    cached_ordersViewData = cached_orders.map(order => {
        const client = clientMap.get(order.clientId);
        const changes = statusMap.get(order.id) || [];
        const currentSt = getCurrentStatus(changes);
        const comps = compositionMap.get(order.id) || [];

        let rawSum = 0;
        comps.forEach(oc => {
            rawSum += calculateFurniturePrice(oc.idFurniture) * oc.count;
        });
        const discount = order.discount || 0;
        const totalSum = rawSum * (1 - discount / 100);

        return {
            ...order,
            _clientName: client ? client.fullName : 'Неизвестный клиент',
            _statusObj: currentSt,
            _statusName: currentSt.name,
            _creationDate: getCreationDate(changes) || new Date(0),
            _rawSum: rawSum,
            _totalSum: totalSum,
            _compositions: comps,
            _changes: changes
        };
    });
}

// ==========================================
// 2. HELPER LOGIC
// ==========================================

function getStatusStyle(statusName) {
    if (!statusName) return { bg: 'var(--status-default-bg)', color: 'var(--status-default-text)' };
    const lower = statusName.toLowerCase();
    if (lower.includes('отменен')) return { bg: 'var(--status-danger-bg)', color: 'var(--status-danger-text)' };
    if (lower.includes('пауз') || lower.includes('ожидает оплаты')) return { bg: 'var(--status-warning-bg)', color: 'var(--status-warning-text)' };
    if (lower.includes('доставлен') || lower.includes('оплачен') || lower.includes('готов')) return { bg: 'var(--status-success-bg)', color: 'var(--status-success-text)' };
    if (lower === 'новый') return { bg: 'var(--status-new-bg)', color: 'var(--status-new-text)' };
    return { bg: 'var(--status-process-bg)', color: 'var(--status-process-text)' };
}

function isOrderLocked(statusName) {
    if (!statusName) return false;
    const lower = statusName.toLowerCase();
    const lockedKeywords = ['оплачен', 'производств', 'готов', 'отгружен', 'доставлен', 'отменен', 'пауз', 'ожидает оплаты'];
    return lockedKeywords.some(keyword => lower.includes(keyword));
}

function getSortedStatusChanges(changesList) {
    if (!changesList || !Array.isArray(changesList) || changesList.length === 0) return [];
    return [...changesList].sort((a, b) => new Date(a.date) - new Date(b.date));
}

function getCurrentStatus(changesList) {
    const sorted = getSortedStatusChanges(changesList);
    if (sorted.length === 0) return { name: 'Не определен', id: 0, date: null };
    const last = sorted[sorted.length - 1];
    const status = cached_statuses.find(s => s.id === last.idStatus);
    return {
        name: status ? status.name : '?',
        id: last.idStatus,
        date: last.date
    };
}

function getEffectiveStatusName(changesList) {
    const sorted = getSortedStatusChanges(changesList);
    for (let i = sorted.length - 1; i >= 0; i--) {
        const st = cached_statuses.find(s => s.id === sorted[i].idStatus);
        if (st && !st.name.toLowerCase().includes('пауз')) {
            return st.name;
        }
    }
    return "Новый";
}

function getAllowedNextStatuses(changesList) {
    const currentSt = getCurrentStatus(changesList);
    if (!currentSt.name) return [];

    const currentName = currentSt.name.trim();
    const currentLower = currentName.toLowerCase();

    if (currentLower.includes('отменен') || currentLower.includes('доставлен')) return [];

    const allowedNames = [];
    let baseName = currentName;
    if (currentLower.includes('пауз')) {
        baseName = getEffectiveStatusName(changesList);
    }

    const flowIndex = STATUS_FLOW.findIndex(s => s.toLowerCase() === baseName.toLowerCase());
    const paidIndex = STATUS_FLOW.findIndex(s => s.toLowerCase() === 'оплачен');

    if (flowIndex !== -1 && flowIndex < STATUS_FLOW.length - 1) {
        allowedNames.push(STATUS_FLOW[flowIndex + 1]);
    }

    if (currentLower.includes('пауз') && flowIndex !== -1) {
        allowedNames.push(STATUS_FLOW[flowIndex]);
    }

    if (!currentLower.includes('пауз')) {
        allowedNames.push("На паузе");
    }

    if (paidIndex !== -1 && flowIndex < paidIndex) {
        allowedNames.push("Отменен");
    }

    const uniqueNames = [...new Set(allowedNames)];
    const result = [];

    uniqueNames.forEach(targetName => {
        const found = cached_statuses.find(s => s.name.trim().toLowerCase() === targetName.trim().toLowerCase());
        if (found) {
            result.push(found);
        }
    });

    return result;
}

function getCreationDate(changesList) {
    const sorted = getSortedStatusChanges(changesList);
    if (sorted.length === 0) return null;
    return new Date(sorted[0].date);
}

function getClientName(id) {
    const c = cached_clients.find(x => x.id === id);
    return c ? c.fullName : 'Неизвестный клиент';
}

function calculateFurniturePrice(furnitureId) {
    const furniture = cached_furniture.find(f => f.id === furnitureId);
    if (!furniture) return 0;
    const comps = cached_furnitureCompositions.filter(c => (c.idFurniture === furnitureId) || (c.entity1Id === furnitureId));
    let selfPrice = 0;
    const sortedPrices = [...cached_prices].sort((a, b) => new Date(b.date) - new Date(a.date));
    comps.forEach(comp => {
        const pId = comp.idPart ?? comp.entity2Id;
        const count = comp.count ?? 0;
        const priceObj = sortedPrices.find(p => p.partId === pId);
        selfPrice += (priceObj ? priceObj.value : 0) * count;
    });
    return selfPrice + (selfPrice * (furniture.markup ?? 0) / 100);
}

// ==========================================
// 3. TABLE RENDERING
// ==========================================

function renderTable() {
    const tbody = document.querySelector('#ordersTable tbody');
    if (!tbody) return;

    const search = (searchInput.value || '').toLowerCase().trim();
    const fStatus = statusFilter.value ? Number(statusFilter.value) : null;
    const fClient = clientFilter.value ? Number(clientFilter.value) : null;
    const fGroup = groupFilter.value;

    tbody.innerHTML = '';

    const checkFilters = (item) => {
        if (search && !item._clientName.toLowerCase().includes(search)) return false;
        if (fClient !== null && item.clientId !== fClient) return false;
        if (fStatus !== null && item._statusObj.id !== fStatus) return false;

        const stNameLower = item._statusName.toLowerCase();
        if (fGroup === 'cancelled' && !stNameLower.includes('отменен')) return false;
        if (fGroup === 'success' && !stNameLower.includes('доставлен')) return false;
        if (fGroup === 'action' && (!stNameLower.includes('новый') && !stNameLower.includes('обработке'))) return false;
        if (fGroup === 'process') {
            const isCancelled = stNameLower.includes('отменен');
            const isSuccess = stNameLower.includes('доставлен');
            const isAction = stNameLower.includes('новый') || stNameLower.includes('обработке');
            if (isCancelled || isSuccess || isAction) return false;
        }
        return true;
    };

    let itemsToRender = [];
    const mode = getSearchMode();
    const matches = cached_ordersViewData.filter(checkFilters);
    let isHighlight = false;

    if (mode === 'filter') {
        itemsToRender = matches;
        orders = matches;
    } else {
        const baseList = cached_ordersViewData.filter(item => {
            if (fClient !== null && item.clientId !== fClient) return false;
            if (fStatus !== null && item._statusObj.id !== fStatus) return false;
            return true;
        });
        itemsToRender = baseList;
        orders = baseList;
        isHighlight = true;
    }

    if (ordersCount) {
        ordersCount.textContent = mode === 'filter'
            ? `Записей: ${matches.length}`
            : `Записей: ${itemsToRender.length} (совпадений: ${matches.length})`;
    }

    if (sortField) {
        itemsToRender.sort((a, b) => {
            const dir = sortDirection === 'asc' ? 1 : -1;
            let va, vb;

            switch (sortField) {
                case 'client': va = a._clientName.toLowerCase(); vb = b._clientName.toLowerCase(); break;
                case 'date': va = a._creationDate; vb = b._creationDate; break;
                case 'status': va = a._statusName; vb = b._statusName; break;
                case 'total': va = a._totalSum; vb = b._totalSum; break;
                case 'raw': va = a._rawSum; vb = b._rawSum; break;
                case 'discount': va = a.discount || 0; vb = b.discount || 0; break;

                default: return 0;
            }
            if (va < vb) return -1 * dir;
            if (va > vb) return 1 * dir;
            return 0;
        });
    }

    currentPage = updatePaginationUI(currentPage, orders.length, perPage, pageInfo, prevBtn, nextBtn);

    if (itemsToRender.length === 0) {
        tbody.innerHTML = '<tr><td colspan="8" class="text-center" style="color: var(--app-text); opacity: 0.6; font-style:italic; padding: 20px;">Заказов нет</td></tr>';
        return;
    }

    const pageItems = paginateData(itemsToRender, currentPage, perPage);
    const fragment = document.createDocumentFragment();
    const currencyFmt = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' });
    const dateFmt = new Intl.DateTimeFormat('ru-RU', { day: '2-digit', month: '2-digit', year: 'numeric' });

    for (const item of pageItems) {
        const tr = document.createElement('tr');
        if (isHighlight && matches.includes(item)) tr.classList.add('highlight-match');

        // 1. Client
        const tdClient = document.createElement('td');
        const clientDiv = document.createElement('div');
        clientDiv.className = 'd-flex align-items-center justify-content-between gap-2';
        const nameSpan = document.createElement('span');
        nameSpan.textContent = item._clientName;
        nameSpan.style.fontWeight = '500';
        const contactBtn = document.createElement('button');
        contactBtn.innerHTML = '📄';
        contactBtn.title = 'Контакты клиента';
        contactBtn.className = 'btn btn-sm btn-contacts btn-square';
        contactBtn.onclick = (e) => { e.stopPropagation(); openContactsModal(item.clientId); };
        clientDiv.append(nameSpan, contactBtn);
        tdClient.appendChild(clientDiv);

        // 2. Date
        const tdDate = document.createElement('td');
        tdDate.textContent = item._creationDate.getTime() > 0 ? dateFmt.format(item._creationDate) : '—';

        // 3. Status
        const tdStatus = document.createElement('td');
        const statusDiv = document.createElement('div');
        statusDiv.className = 'd-flex align-items-center justify-content-between gap-1';
        const stName = item._statusName;
        const badge = document.createElement('span');
        badge.className = 'badge';
        const style = getStatusStyle(stName);
        badge.style.backgroundColor = style.bg;
        badge.style.color = style.color;
        badge.textContent = stName;

        const btnGroup = document.createElement('div');
        btnGroup.className = 'd-flex gap-1';
        const historyBtn = document.createElement('button');
        historyBtn.innerHTML = '🕒';
        historyBtn.title = 'Просмотр истории статусов';
        historyBtn.className = 'btn btn-sm btn-history btn-square';
        historyBtn.onclick = (e) => { e.stopPropagation(); openHistoryModal(item._changes); };

        const changeStatusBtn = document.createElement('button');
        changeStatusBtn.innerHTML = '⇄';
        changeStatusBtn.title = 'Изменить статус';
        changeStatusBtn.className = 'btn btn-sm btn-change-status btn-square';

        const allowedNext = getAllowedNextStatuses(item._changes);
        if (allowedNext.length === 0) {
            changeStatusBtn.disabled = true;
        } else {
            changeStatusBtn.onclick = (e) => { e.stopPropagation(); openChangeStatusModal(item, item._changes); };
        }

        btnGroup.append(historyBtn, changeStatusBtn);
        statusDiv.append(badge, btnGroup);
        tdStatus.appendChild(statusDiv);

        // Prices
        const tdRaw = document.createElement('td');
        tdRaw.textContent = currencyFmt.format(item._rawSum);

        const tdDisc = document.createElement('td');
        const discount = item.discount || 0;
        tdDisc.textContent = discount > 0 ? `-${discount}%` : '—';
        if (discount > 0) tdDisc.classList.add('text-success', 'fw-bold');

        const tdTotal = document.createElement('td');
        tdTotal.textContent = currencyFmt.format(item._totalSum);
        tdTotal.classList.add('fw-bold');

        // Comp
        const tdComp = document.createElement('td');
        tdComp.className = 'col-fit';

        // Получаем список композиций
        const comps = item._compositions || [];

        if (comps.length > 0) {
            const compBtn = document.createElement('button');
            compBtn.innerHTML = '<span>📦</span>';
            // Добавим подсказку с количеством позиций
            compBtn.title = `Просмотр состава (${comps.length} поз.)`;
            compBtn.className = 'btn btn-sm btn-composition btn-square';
            compBtn.onclick = (e) => {
                e.stopPropagation();
                openOrderCompositionModal(comps, item);
            };
            tdComp.appendChild(compBtn);
        } else {
            // Если пусто — рисуем прочерк в стилистике темы (без text-muted)
            tdComp.innerHTML = '<span style="color: var(--warm-border); opacity: 0.6; font-weight: 500;">—</span>';
        }

        // Actions
        const tdActions = document.createElement('td');
        tdActions.className = 'col-fit';

        if (isOrderLocked(stName)) {
            const lockSpan = document.createElement('span');
            lockSpan.innerHTML = '🔒';
            lockSpan.title = `Изменение запрещено (Статус: ${stName})`;
            lockSpan.style.fontSize = '1.2rem';
            lockSpan.style.opacity = '0.6';
            const centerDiv = document.createElement('div');
            centerDiv.className = 'd-flex justify-content-center w-100';
            centerDiv.appendChild(lockSpan);
            tdActions.appendChild(centerDiv);
        } else {
            const actionsDiv = document.createElement('div');
            actionsDiv.className = 'actions-group justify-content-end';

            const configBtn = document.createElement('button');
            configBtn.textContent = '⚙';
            configBtn.title = 'Редактировать состав';
            configBtn.className = 'btn btn-sm btn-outline-secondary btn-square';
            configBtn.addEventListener('click', () => openEditOrderCompositionModal(item));

            const editBtn = document.createElement('button');
            editBtn.textContent = '✎';
            editBtn.title = 'Редактировать заказ';
            editBtn.className = 'btn btn-sm btn-primary btn-square';
            // Вызов модалки редактирования
            editBtn.onclick = () => openEditModal(item);

            const delBtn = document.createElement('button');
            delBtn.textContent = '🗑';
            delBtn.title = 'Удалить';
            delBtn.className = 'btn btn-sm btn-danger btn-square';
            // Вызов модалки удаления
            delBtn.onclick = () => openDeleteModal(item);

            actionsDiv.append(configBtn, editBtn, delBtn);
            tdActions.appendChild(actionsDiv);
        }

        tr.append(tdClient, tdDate, tdStatus, tdRaw, tdDisc, tdTotal, tdComp, tdActions);
        fragment.appendChild(tr);
    }
    tbody.appendChild(fragment);
}

// ==========================================
// 4. CRUD LOGIC (Validation & Forms)
// ==========================================

createOrderButton.addEventListener('click', () => {
    if (createForm) {
        createForm.reset();

        // 1. Установка даты через Flatpickr
        if (fpCreateDate) {
            fpCreateDate.setDate(new Date());
        }

        // 2. Поиск и установка ID статуса "Новый"
        const newStatus = cached_statuses.find(s => s.name.trim().toLowerCase() === 'новый');
        const statusIdInput = document.getElementById('createStatusId');

        if (newStatus && statusIdInput) {
            statusIdInput.value = newStatus.id;
        } else if (statusIdInput) {
            console.warn('Статус "Новый" не найден в кэше');
            statusIdInput.value = 1;
        }

        initCustomSelects();
    }
    showModal(createModal);
});

function openEditModal(order) {
    document.getElementById('editId').value = order.id;
    document.getElementById('editClientIdHidden').value = order.clientId;
    document.getElementById('editDateHidden').value = order._creationDate ? order._creationDate.toISOString() : '';

    // Read-only fields
    document.getElementById('editClientNameDisplay').value = order._clientName;
    const dateFmt = new Intl.DateTimeFormat('ru-RU', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' });
    document.getElementById('editDateDisplay').value = order._creationDate.getTime() > 0 ? dateFmt.format(order._creationDate) : '—';

    // Editable field
    document.getElementById('editDiscount').value = order.discount || 0;

    showModal(editModal);
}

function openDeleteModal(order) {
    deleteIdInput.value = order.id;
    showModal(deleteModal);
}

function validateOrderForm(prefix) {
    const errors = [];

    // 1. Проверка обязательных полей (только для Create)
    if (prefix === 'create') {
        const client = document.getElementById('createClientId');
        if (!client.value) {
            errors.push('Необходимо выбрать клиента.');
        }

        const date = document.getElementById('createDate');
        if (!date.value) {
            errors.push('Необходимо указать дату начала.');
        }
    }

    // 2. Проверка скидки
    const discountInput = document.getElementById(prefix + 'Discount');
    const discountVal = parseFloat(discountInput.value);

    if (discountInput.value !== '') {
        if (isNaN(discountVal)) {
            errors.push('Скидка должна быть числом.');
        } else if (discountVal < 0) {
            errors.push('Скидка не может быть отрицательной.');
        } else if (discountVal > 100) {
            errors.push('Скидка не может превышать 100%.');
        }
    }

    if (errors.length > 0) {
        const errorList = document.getElementById('errorList');
        errorList.innerHTML = '';
        errors.forEach(msg => {
            const li = document.createElement('li');
            li.textContent = msg;
            li.style.marginBottom = '0.25rem';
            errorList.appendChild(li);
        });
        showModal(errorModal);
        return false;
    }

    return true;
}

// Ограничитель ввода чисел для скидки (0-100)
function attachDiscountInputLimits() {
    ['createDiscount', 'editDiscount'].forEach(id => {
        const el = document.getElementById(id);
        if (!el) return;

        el.addEventListener('input', function () {
            let val = parseFloat(this.value);
            if (val > 100) this.value = 100;
            if (val < 0) this.value = 0;
        });

        el.addEventListener('keydown', function (e) {
            if (e.key === '-' || e.key === 'Subtract') {
                e.preventDefault();
            }
        });
    });
}

// Универсальный обработчик отправки форм
async function handleOrderFormSubmit(e, handlerName, modalToClose) {
    e.preventDefault();

    const formId = e.target.id;
    let prefix = '';
    if (formId === 'createForm') prefix = 'create';
    if (formId === 'editForm') prefix = 'edit';

    // Валидация (пропускаем для удаления)
    if (prefix && !validateOrderForm(prefix)) {
        return;
    }

    try {
        toggleLoader(true);
        const formData = new FormData(e.target);
        const response = await fetch(`?handler=${handlerName}`, { method: 'POST', body: formData });

        let result = {};
        try { result = await response.json(); } catch { result = { success: response.ok }; }

        if (!response.ok || (result.hasOwnProperty('success') && !result.success)) {
            const errorList = document.getElementById('errorList');
            if (errorList) {
                errorList.innerHTML = `<li>${result.message || 'Ошибка операции'}</li>`;
                showModal(errorModal);
            } else {
                alert(result.message || 'Ошибка операции');
            }
        } else {
            hideModal(modalToClose);
            if (prefix === 'create')
                clearStore('StatusChanges');
            await initData();
            renderTable();
        }
    } catch (err) {
        console.error(err);
        const errorList = document.getElementById('errorList');
        if (errorList) {
            errorList.innerHTML = `<li>Ошибка соединения: ${err.message}</li>`;
            showModal(errorModal);
        }
    } finally {
        toggleLoader(false);
    }
}

createForm.addEventListener('submit', (e) => handleOrderFormSubmit(e, 'CreateOrder', createModal));
editForm.addEventListener('submit', (e) => handleOrderFormSubmit(e, 'UpdateOrder', editModal));
deleteForm.addEventListener('submit', (e) => handleOrderFormSubmit(e, 'DeleteOrder', deleteModal));

// ==========================================
// 5. READ-ONLY & COMPOSITION LOGIC
// ==========================================

function openContactsModal(clientId) {
    const client = cached_clients.find(c => c.id === clientId);
    contactsContent.innerHTML = '';

    if (!client) {
        contactsContent.innerHTML = '<div class="text-center p-4 text-muted">Клиент не найден в базе данных</div>';
        showModal(contactsModal);
        return;
    }

    // Подготовка данных
    const phone = client.phone || '';
    const email = client.email || '';
    const address = client.address || '';

    // HTML без аватара, ID и ссылок
    const html = `
        <div class="contact-card-wrapper">
            <div class="contact-header" style="border-bottom: none; padding-bottom: 0.5rem; justify-content: center;">
                <div class="contact-name text-center" style="font-size: 1.4rem;">${client.fullName}</div>
            </div>

            <div class="contact-details">
                <div class="contact-row">
                    <div class="contact-icon-box">📞</div>
                    <div class="contact-info">
                        <span class="contact-label">Телефон</span>
                        <span class="contact-value" style="user-select: all;">
                            ${phone || '<span class="contact-empty">Не указан</span>'}
                        </span>
                    </div>
                    ${phone ? `<button class="btn-copy-action btn-square" onclick="copyToClipboard('${phone}', this)" title="Скопировать">📋</button>` : ''}
                </div>

                <div class="contact-row">
                    <div class="contact-icon-box">✉️</div>
                    <div class="contact-info">
                        <span class="contact-label">Email</span>
                        <span class="contact-value" style="user-select: all;">
                             ${email || '<span class="contact-empty">Не указан</span>'}
                        </span>
                    </div>
                    ${email ? `<button class="btn-copy-action btn-square" onclick="copyToClipboard('${email}', this)" title="Скопировать">📋</button>` : ''}
                </div>
                
                ${address ? `
                <div class="contact-row">
                    <div class="contact-icon-box">📍</div>
                    <div class="contact-info">
                        <span class="contact-label">Адрес</span>
                        <span class="contact-value" style="white-space: normal;">${address}</span>
                    </div>
                    <button class="btn-copy-action btn-square" onclick="copyToClipboard('${address}', this)" title="Скопировать">📋</button>
                </div>` : ''}
            </div>
        </div>
    `;

    contactsContent.innerHTML = html;
    showModal(contactsModal);
}

// Обновленная функция копирования (поддерживает передачу кнопки для визуального эффекта)
function copyToClipboard(text, btnElement) {
    if (!text) return;
    navigator.clipboard.writeText(text).then(() => {
        if (btnElement) {
            const originalHTML = btnElement.innerHTML;
            btnElement.innerHTML = '✓';
            btnElement.style.color = 'var(--action-create)';

            // Если это кнопка в таблице, убираем стиль через время
            setTimeout(() => {
                btnElement.innerHTML = originalHTML;
                btnElement.style.color = '';
            }, 1000);
        }
    }).catch(err => console.error('Ошибка копирования:', err));
}

function openHistoryModal(changesList) {
    const container = historyContent;
    container.innerHTML = '';
    container.style.maxHeight = '50vh';
    container.style.overflowY = 'auto';

    const sorted = getSortedStatusChanges(changesList);
    const historyView = [...sorted].reverse();

    if (historyView.length === 0) {
        container.innerHTML = '<div class="p-3 text-center" style="color:var(--app-text); opacity:0.6;">История пуста</div>';
        showModal(historyModal);
        return;
    }
    const table = document.createElement('table');
    table.className = 'table table-sm table-hover mb-0 admin-table';
    table.innerHTML = `<thead class="table-light"><tr><th style="width: 140px; position: sticky; top: 0; background: var(--table-header-bg); z-index: 2;">Дата</th><th style="position: sticky; top: 0; background: var(--table-header-bg); z-index: 2;">Статус</th></tr></thead><tbody></tbody>`;
    const tbody = table.querySelector('tbody');
    const dateTimeFmt = new Intl.DateTimeFormat('ru-RU', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' });

    historyView.forEach((h, idx) => {
        const stObj = cached_statuses.find(s => s.id === h.idStatus);
        const name = stObj ? stObj.name : `ID: ${h.idStatus}`;
        const isLatest = idx === 0;
        const tr = document.createElement('tr');
        if (isLatest) tr.style.backgroundColor = 'rgba(var(--accent-rgb), 0.05)';
        const style = getStatusStyle(name);
        const badge = `<span class="badge" style="background-color: ${style.bg}; color: ${style.color};">${name}</span>`;
        tr.innerHTML = `<td style="color: var(--app-text); font-variant-numeric: tabular-nums;">${dateTimeFmt.format(new Date(h.date))}</td><td>${badge}</td>`;
        tbody.appendChild(tr);
    });
    container.appendChild(table);
    showModal(historyModal);
}

function openOrderCompositionModal(compsList, order) {
    const container = compositionContent;

    container.className = '';
    container.innerHTML = '';
    container.style.padding = '0';
    container.style.border = 'none';
    container.style.background = 'transparent';
    container.style.maxHeight = 'none';
    container.style.overflow = 'visible';

    const clientName = getClientName(order.clientId);

    const header = document.createElement('div');
    header.className = 'd-flex justify-content-between align-items-center mb-2 px-1';
    header.innerHTML = `
        <div class="d-flex flex-column">
            <span class="small text-uppercase fw-bold" style="color: var(--text-muted); letter-spacing: 0.05em; font-size: 0.75rem;">Заказчик:</span>
            <span class="fw-bold" style="color: var(--app-text); font-size: 1.1rem; line-height: 1.2;">${clientName}</span>
        </div>
        <span class="badge badge-theme" style="font-size: 0.85rem;">${compsList.length} позиций</span>`;
    container.appendChild(header);

    if (compsList.length === 0) {
        container.innerHTML += `<div class="d-flex flex-column align-items-center justify-content-center p-4" style="color: var(--app-text);">
            <span class="fs-4 mb-2">∅</span>
            <span>В заказе нет позиций</span>
        </div>`;
    } else {
        const wrapper = document.createElement('div');
        wrapper.className = 'composition-table-wrapper';
        wrapper.style.height = '400px';
        wrapper.style.marginBottom = '0';

        const scroll = document.createElement('div');
        scroll.className = 'composition-table-scroll';

        const tbl = document.createElement('table');
        tbl.className = 'table table-sm table-hover align-middle mb-0 admin-table';

        tbl.innerHTML = `
            <thead class="table-light">
                <tr>
                    <th>Мебель</th>
                    <th class="text-end" style="width: 100px;">Цена шт.</th>
                    <th class="text-center" style="width: 80px;">Кол-во</th>
                    <th class="text-end" style="width: 110px;">Сумма</th>
                </tr>
            </thead>`;

        const tbody = document.createElement('tbody');
        const currencyFmt = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' });
        let totalSum = 0;

        compsList.forEach(c => {
            const furniture = cached_furniture.find(f => f.id === c.idFurniture);
            const price = calculateFurniturePrice(c.idFurniture);
            const sum = price * c.count;
            totalSum += sum;

            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${furniture ? furniture.name : '<span class="text-danger fst-italic">Удалено</span>'}</td>
                <td class="text-end small">${currencyFmt.format(price)}</td>
                <td class="text-center fw-bold" style="color: var(--app-text);">${c.count}</td>
                <td class="text-end fw-bold" style="color: var(--app-text);">${currencyFmt.format(sum)}</td>`;
            tbody.appendChild(tr);
        });

        tbl.appendChild(tbody);
        scroll.appendChild(tbl);
        wrapper.appendChild(scroll);
        container.appendChild(wrapper);

        const discount = order.discount || 0;
        const finalTotal = totalSum * (1 - discount / 100);

        const footer = document.createElement('div');
        footer.className = 'd-flex justify-content-end align-items-center mt-2 px-1 small';
        footer.style.color = 'var(--app-text)';

        let footerHtml = `<span style="font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; margin-right: 10px; color: var(--text-muted);">Итого:</span>`;

        if (discount > 0) {
            footerHtml += `
                <span class="text-decoration-line-through me-2">${currencyFmt.format(totalSum)}</span>
                <span class="text-success me-2">(-${discount}%)</span>
            `;
        }

        footerHtml += `<b class="fs-5">${currencyFmt.format(finalTotal)}</b>`;

        footer.innerHTML = footerHtml;
        container.appendChild(footer);
    }
    showModal(compositionModal);
}

// ==========================================
// 6. STATUS CHANGE LOGIC
// ==========================================

function openChangeStatusModal(order, changesList) {
    changeStatusOrderId.value = order.id;
    const currentSt = getCurrentStatus(changesList);

    const style = getStatusStyle(currentSt.name);
    currentStatusDisplay.innerHTML = `<span class="badge" style="background-color: ${style.bg}; color: ${style.color}; font-size: 1rem;">${currentSt.name}</span>`;

    newStatusSelect.innerHTML = '';
    const allowed = getAllowedNextStatuses(changesList);

    if (allowed.length === 0) {
        newStatusSelect.innerHTML = '<option disabled selected>Нет доступных переходов</option>';
    } else {
        const placeholder = document.createElement('option');
        placeholder.value = ""; placeholder.text = "Выберите новый статус..."; placeholder.disabled = true; placeholder.selected = true;
        newStatusSelect.appendChild(placeholder);
        allowed.forEach(st => {
            const opt = document.createElement('option'); opt.value = st.id; opt.textContent = st.name; newStatusSelect.appendChild(opt);
        });
    }

    new CustomSelect(newStatusSelect);

    // Установка даты через Flatpickr
    if (fpChangeStatusDate) {
        // Установка текущей даты
        fpChangeStatusDate.setDate(new Date());

        // Установка минимальной даты (не раньше предыдущего статуса)
        const prevDate = currentSt.date ? new Date(currentSt.date) : null;
        if (prevDate) {
            fpChangeStatusDate.set('minDate', prevDate);
        } else {
            fpChangeStatusDate.set('minDate', null);
        }
    }

    showModal(changeStatusModal);
}

changeStatusForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    const orderId = document.getElementById('changeStatusOrderId').value;
    const statusId = document.getElementById('newStatusSelect').value;

    if (!orderId || !statusId) {
        alert("Ошибка: Не все поля заполнены корректно.");
        return;
    }

    try {
        toggleLoader(true);
        const formData = new FormData(e.target);
        const res = await fetch('?handler=CreateStatusChange', {
            method: 'POST',
            body: formData
        });

        let data = {};
        try { data = await res.json(); } catch { }

        if (res.ok && (data.success === undefined || data.success)) {
            hideModal(changeStatusModal);
            clearStore('StatusChanges');
            await initData();
            renderTable();
        } else {
            alert(data.message || 'Ошибка при смене статуса');
        }
    } catch (err) {
        console.error(err);
        alert('Ошибка соединения');
    } finally {
        toggleLoader(false);
    }
});

// ==========================================
// 7. ORDER COMPOSITION EDITOR (WRITE)
// ==========================================

function openEditOrderCompositionModal(order) {
    currentEditingOrderId = order.id;

    const existingComps = cached_orderCompositions.filter(c => c.idOrder === order.id);

    currentOrderCompositionDraft = existingComps.map(c => {
        const furn = cached_furniture.find(f => f.id === c.idFurniture);
        return {
            furnitureId: c.idFurniture,
            count: c.count,
            name: furn ? furn.name : 'Удаленная мебель',
            price: calculateFurniturePrice(c.idFurniture)
        };
    });

    furnitureSearchInput.value = '';
    if (fModeFilter) fModeFilter.checked = true;

    renderOrderDraftTable();
    renderAvailableFurnitureTable();
    showModal(editOrderCompositionModal);
}

function renderOrderDraftTable() {
    currentOrderCompTableBody.innerHTML = '';
    let totalCount = 0;
    let totalSumMoney = 0;
    const currencyFmt = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' });

    currentOrderCompositionDraft.forEach((item, index) => {
        totalCount += item.count;
        const itemTotalCost = item.price * item.count;
        totalSumMoney += itemTotalCost;

        const tr = document.createElement('tr');

        const tdName = document.createElement('td');
        tdName.textContent = item.name;

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
            renderOrderDraftTable();
        });
        tdQty.appendChild(input);

        const tdSum = document.createElement('td');
        tdSum.className = 'text-end small fw-bold';
        tdSum.textContent = currencyFmt.format(itemTotalCost);

        const tdAct = document.createElement('td');
        tdAct.className = 'col-fit';
        const delBtn = document.createElement('button');
        delBtn.textContent = '×';
        delBtn.className = 'btn btn-sm btn-danger btn-square';
        delBtn.onclick = (e) => {
            e.stopPropagation();
            currentOrderCompositionDraft.splice(index, 1);
            renderOrderDraftTable();
            renderAvailableFurnitureTable();
        };
        tdAct.appendChild(delBtn);

        tr.append(tdName, tdQty, tdSum, tdAct);
        currentOrderCompTableBody.appendChild(tr);
    });

    if (headerOrderTotalCountLabel) headerOrderTotalCountLabel.textContent = totalCount + ' шт.';
    if (summaryOrderPriceLabel) summaryOrderPriceLabel.textContent = currencyFmt.format(totalSumMoney);

    attachRowSelection('#currentOrderCompTable tbody');
}

function renderAvailableFurnitureTable() {
    availableFurnitureTableBody.innerHTML = '';
    const searchText = (furnitureSearchInput.value || '').toLowerCase().trim();
    const mode = document.querySelector('input[name="furnSearchMode"]:checked')?.value || 'filter';

    const addedIds = new Set(currentOrderCompositionDraft.map(x => x.furnitureId));
    let furnitureToRender = cached_furniture.filter(f => f.activity === true);

    if (mode === 'filter' && searchText) {
        furnitureToRender = furnitureToRender.filter(f => f.name.toLowerCase().includes(searchText));
    }

    const fragment = document.createDocumentFragment();
    const currencyFmt = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' });

    furnitureToRender.forEach(furn => {
        const isMatch = searchText && furn.name.toLowerCase().includes(searchText);
        const isAdded = addedIds.has(furn.id);

        const tr = document.createElement('tr');
        if (mode === 'highlight' && isMatch) tr.classList.add('highlight-match');
        if (isAdded) tr.style.backgroundColor = 'rgba(237, 217, 183, 0.3)';

        const tdName = document.createElement('td');
        tdName.textContent = furn.name;

        const tdPrice = document.createElement('td');
        tdPrice.className = 'text-end small';
        const price = calculateFurniturePrice(furn.id);
        tdPrice.textContent = currencyFmt.format(price);

        const tdAction = document.createElement('td');
        tdAction.className = 'col-fit text-center';

        if (isAdded) {
            const check = document.createElement('div');
            check.className = 'icon-added-check';
            check.textContent = '✓';
            check.title = 'Уже в заказе';
            tdAction.appendChild(check);
        } else {
            const addBtn = document.createElement('button');
            addBtn.textContent = '+';
            addBtn.className = 'btn btn-sm btn-success btn-square';
            addBtn.onclick = (e) => {
                e.stopPropagation();
                currentOrderCompositionDraft.push({
                    furnitureId: furn.id,
                    count: 1,
                    name: furn.name,
                    price: price
                });
                renderOrderDraftTable();
                renderAvailableFurnitureTable();
            };
            tdAction.appendChild(addBtn);
        }

        tr.append(tdName, tdPrice, tdAction);
        fragment.appendChild(tr);
    });

    availableFurnitureTableBody.appendChild(fragment);
    attachRowSelection('#availableFurnitureTable tbody');
}

furnitureSearchInput.addEventListener('input', renderAvailableFurnitureTable);
if (fModeFilter) fModeFilter.addEventListener('change', renderAvailableFurnitureTable);
if (fModeHighlight) fModeHighlight.addEventListener('change', renderAvailableFurnitureTable);

if (clearFurnitureSearchBtn) {
    clearFurnitureSearchBtn.addEventListener('click', () => {
        furnitureSearchInput.value = '';
        furnitureSearchInput.focus();
        renderAvailableFurnitureTable();
    });
}

saveCompositionBtn.addEventListener('click', async () => {
    if (!currentEditingOrderId) return;

    try {
        toggleLoader(true);

        const originalComps = cached_orderCompositions.filter(c => c.idOrder === currentEditingOrderId);
        const newComps = currentOrderCompositionDraft;

        const toCreate = [];
        const toUpdate = [];
        const toDelete = [];

        originalComps.forEach(old => {
            const exists = newComps.find(n => n.furnitureId === old.idFurniture);
            if (!exists) {
                toDelete.push({
                    IdOrder: Number(currentEditingOrderId),
                    IdFurniture: Number(old.idFurniture)
                });
            }
        });

        newComps.forEach(newItem => {
            const oldItem = originalComps.find(o => o.idFurniture === newItem.furnitureId);
            const dto = {
                IdOrder: Number(currentEditingOrderId),
                IdFurniture: Number(newItem.furnitureId),
                Count: Number(newItem.count),
                UpdateDate: new Date().toISOString()
            };

            if (!oldItem) {
                toCreate.push(dto);
            } else if (oldItem.count !== newItem.count) {
                toUpdate.push(dto);
            }
        });

        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
        const headers = {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        };

        if (toCreate.length > 0) {
            const res = await fetch('?handler=CreateCompositionRange', {
                method: 'POST', headers: headers, body: JSON.stringify(toCreate)
            });
            const data = await res.json();
            if (!data.success) throw new Error(data.message || 'Ошибка добавления позиций');
        }

        if (toUpdate.length > 0) {
            const res = await fetch('?handler=UpdateCompositionRange', {
                method: 'POST', headers: headers, body: JSON.stringify(toUpdate)
            });
            const data = await res.json();
            if (!data.success) throw new Error(data.message || 'Ошибка обновления позиций');
        }

        if (toDelete.length > 0) {
            const res = await fetch('?handler=DeleteCompositionRange', {
                method: 'POST', headers: headers, body: JSON.stringify(toDelete)
            });
            const data = await res.json();
            if (!data.success) throw new Error(data.message || 'Ошибка удаления позиций');
        }

        clearStore('OrderCompositions');
        hideModal(editOrderCompositionModal);
        await initData();
        renderTable();

    } catch (e) {
        console.error(e);
        alert(e.message || 'Ошибка сохранения состава заказа');
    } finally {
        toggleLoader(false);
    }
});

// ==========================================
// 8. GLOBAL LISTENERS
// ==========================================

function onHeaderClick(index, th) {
    // Indexes: 0:Client, 1:Date, 2:Status, 3:RawSum, 4:Discount, 5:TotalSum
    const fieldMap = { 0: 'client', 1: 'date', 2: 'status', 3: 'raw', 4: 'discount', 5: 'total' };

    const field = fieldMap[index];
    if (!field) return;

    if (sortField === field) {
        sortDirection = sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
        sortField = field;
        // Скидку логично сортировать сначала по убыванию (кто больше получил),
        // но стандартно числа часто сортируют по возрастанию. 
        // Оставим desc для важных чисел (дата, итог, скидка).
        sortDirection = (field === 'date' || field === 'total' || field === 'raw' || field === 'discount') ? 'desc' : 'asc';
    }

    updateHeaderSortUI(headers, index, sortDirection);
    currentPage = 1;
    renderTable();
}

searchButton.addEventListener('click', () => { currentPage = 1; renderTable(); });
searchInput.addEventListener('keyup', (e) => { if (e.key === 'Enter') { currentPage = 1; renderTable(); } });
clearSearchTextBtn.addEventListener('click', () => { searchInput.value = ''; currentPage = 1; renderTable(); });
statusFilter.addEventListener('change', () => { currentPage = 1; renderTable(); });
clientFilter.addEventListener('change', () => { currentPage = 1; renderTable(); });
groupFilter.addEventListener('change', () => { currentPage = 1; renderTable(); });
prevBtn.addEventListener('click', () => { if (currentPage > 1) { currentPage--; renderTable(); } });
nextBtn.addEventListener('click', () => { const totalPages = Math.ceil(orders.length / perPage); if (currentPage < totalPages) { currentPage++; renderTable(); } });