// wwwroot/js/pages/prices.js

let cached_prices = [];
let cached_parts = [];
let prices = [];
let latestPriceIds = new Set();
// Константа для лимита редактирования (7 дней в миллисекундах)
const EDIT_LIMIT_MS = 7 * 24 * 60 * 60 * 1000;
const CURRENT_TIME = new Date().getTime(); // Фиксируем текущее время при загрузке

let currentPage = 1;
const perPage = 20;
let sortField = 'date';
let sortDirection = 'desc';

let fpDateFrom, fpDateTo, fpEditDate;
// --- UI Elements ---
const searchInput = document.getElementById('searchInput');
const searchButton = document.getElementById('searchButton');

const clearSearchTextBtn = document.getElementById('clearSearchText');
const clearSearchRangeBtn = document.getElementById('clearSearchRange');

const searchPriceMin = document.getElementById('searchPriceMin');
const searchPriceMax = document.getElementById('searchPriceMax');
const searchContainerSingle = document.getElementById('searchContainerSingle');
const searchContainerRange = document.getElementById('searchContainerRange');

const partFilter = document.getElementById('partFilter');
const actualityFilter = document.getElementById('actualityFilter');
const dateFromFilter = document.getElementById('dateFromFilter');
const dateToFilter = document.getElementById('dateToFilter');
const pricesCount = document.getElementById('pricesCount');

// --- Modals ---
const editModal = document.getElementById('editModal');
const deleteModal = document.getElementById('deleteModal');
const errorModal = document.getElementById('errorModal');

// --- Forms ---
const editForm = document.getElementById('editForm');
const deleteForm = document.getElementById('deleteForm');
const editIdInput = document.getElementById('editId');
const editPartIdInput = document.getElementById('editPartId');
const deleteIdInput = document.getElementById('deleteId');
const editPartNameDisplay = document.getElementById('editPartNameDisplay');
const editDateInput = document.getElementById('editDate');

// --- Pagination & Table ---
const prevBtn = document.getElementById('prev');
const nextBtn = document.getElementById('next');
const pageInfo = document.getElementById('pageInfo');
const table = document.getElementById('pricesTable');
const headers = table.querySelectorAll('thead th');

// === INITIALIZATION ===

headers.forEach((th, index) => {
    th.style.cursor = 'pointer';
    th.addEventListener('click', () => onHeaderClick(index));
});

document.addEventListener('DOMContentLoaded', async () => {
    try {
        toggleLoader(true);

        // 1. ИНИЦИАЛИЗАЦИЯ FLATPICKR
        initDatePickers();

        initCustomSelects();
        attachRowSelection('#pricesTable tbody');
        initAutoDecimalCorrection();
        initSearchMethodLogic();
        initViewModeLogic();

        await initData();
        initFilters();

        recalculateLatestPermissions();
        renderTable();
    } finally {
        toggleLoader(false);
    }
    startAutoSync();
});

function initDatePickers() {
    // Общая конфигурация для всех календарей
    const commonConfig = {
        locale: "ru",          // Русский язык
        dateFormat: "Y-m-d",   // Формат значения, которое пойдет в value (ISO)
        altInput: true,        // Включаем "альтернативный" инпут для отображения
        altFormat: "d.m.Y",    // Формат, который видит пользователь (25.10.2023)
        disableMobile: "true", // ВАЖНО: Принудительно используем кастомный вид везде (иначе на телефоне будет системный календарь)
        allowInput: true       // Разрешаем ручной ввод (опционально)
    };

    // 1. Фильтр "Дата С" (#dateFromFilter)
    fpDateFrom = flatpickr("#dateFromFilter", {
        ...commonConfig,
        onChange: function (selectedDates, dateStr, instance) {
            // При выборе даты начала, обновляем минимальную дату для "Дата По"
            if (fpDateTo) {
                fpDateTo.set("minDate", dateStr);
            }
            // Сбрасываем пагинацию и обновляем таблицу
            currentPage = 1;
            renderTable();
        }
    });

    // 2. Фильтр "Дата По" (#dateToFilter)
    fpDateTo = flatpickr("#dateToFilter", {
        ...commonConfig,
        onChange: function (selectedDates, dateStr, instance) {
            // При выборе даты конца, обновляем максимальную дату для "Дата С"
            if (fpDateFrom) {
                fpDateFrom.set("maxDate", dateStr);
            }
            currentPage = 1;
            renderTable();
        }
    });

    // 3. Поле редактирования в модалке (#editDate)
    fpEditDate = flatpickr("#editDate", {
        locale: "ru",
        enableTime: true,      // Включаем время
        time_24hr: true,       // 24-часовой формат
        dateFormat: "Y-m-dTH:i", // Формат для отправки на сервер (ISO с временем)
        altInput: true,
        altFormat: "d.m.Y H:i", // Формат для пользователя (31.12.2023 23:59)
        disableMobile: "true",  // Обязательно для стилизации
        allowInput: true,

        // Хук при закрытии, чтобы убедиться, что фокус ушел (иногда помогает с UI)
        onClose: function (selectedDates, dateStr, instance) {
            // Можно добавить валидацию здесь, если нужно
        }
    });
}

function initSearchMethodLogic() {
    const radios = document.querySelectorAll('input[name="searchMethod"]');
    radios.forEach(radio => {
        radio.addEventListener('change', (e) => {
            const method = e.target.value;
            if (method === 'range') {
                searchContainerSingle.style.display = 'none';
                searchContainerRange.style.display = 'flex';
            } else {
                searchContainerSingle.style.display = 'flex';
                searchContainerRange.style.display = 'none';
            }
            currentPage = 1;
            renderTable();
        });
    });
}

function initViewModeLogic() {
    const radios = document.querySelectorAll('input[name="viewMode"]');
    radios.forEach(radio => {
        radio.addEventListener('change', () => {
            renderTable();
        });
    });
}

function getSearchMethod() {
    const checked = document.querySelector('input[name="searchMethod"]:checked');
    return checked ? checked.value : 'start';
}

function getViewMode() {
    const checked = document.querySelector('input[name="viewMode"]:checked');
    return checked ? checked.value : 'filter';
}

// === DATA FETCHING ===

async function initData() {
    await Promise.all([
        syncTable('Prices'),
        syncTable('Parts')
    ]);

    const [pricesData, partsData] = await Promise.all([
        getAll('Prices'),
        getAll('Parts')
    ]);

    cached_prices = pricesData || [];
    cached_parts = partsData || [];
}

function initFilters() {
    const sortedParts = [...cached_parts].sort((a, b) => (a.name || '').localeCompare(b.name || ''));
    fillSelect(partFilter, 'Деталь: все записи', sortedParts);
    initCustomSelects();
}

function startAutoSync() {
    async function loop() {
        await initData();
        recalculateLatestPermissions();
        renderTable();
        setTimeout(loop, 5 * 60 * 1000);
    }
    loop();
}

function recalculateLatestPermissions() {
    latestPriceIds.clear();
    const map = new Map();

    cached_prices.forEach(p => {
        const pDate = new Date(p.date).getTime();
        if (!map.has(p.partId)) {
            map.set(p.partId, { id: p.id, date: pDate });
        } else {
            const currentMax = map.get(p.partId);
            if (pDate > currentMax.date) {
                map.set(p.partId, { id: p.id, date: pDate });
            }
        }
    });

    for (const val of map.values()) {
        latestPriceIds.add(val.id);
    }
}

// === SORTING ===

function onHeaderClick(index) {
    const fieldMap = ['part', 'value', 'date', 'actions'];
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

// === LISTENERS ===

dateFromFilter.addEventListener('change', () => {
    const fromVal = dateFromFilter.value;
    const toVal = dateToFilter.value;
    if (fromVal && toVal && fromVal > toVal) dateToFilter.value = fromVal;
    currentPage = 1;
    renderTable();
});

dateToFilter.addEventListener('change', () => {
    const fromVal = dateFromFilter.value;
    const toVal = dateToFilter.value;
    if (fromVal && toVal && toVal < fromVal) dateFromFilter.value = toVal;
    currentPage = 1;
    renderTable();
});

searchPriceMin.addEventListener('change', () => {
    let min = parseFloat(searchPriceMin.value);
    let max = parseFloat(searchPriceMax.value);
    if (min < 0) { min = 0; searchPriceMin.value = 0; }
    if (!isNaN(min) && !isNaN(max) && min > max) { searchPriceMax.value = min; }
    currentPage = 1;
    renderTable();
});

searchPriceMax.addEventListener('change', () => {
    let min = parseFloat(searchPriceMin.value);
    let max = parseFloat(searchPriceMax.value);
    if (max < 0) { max = 0; searchPriceMax.value = 0; }
    if (!isNaN(min) && !isNaN(max) && max < min) { searchPriceMin.value = max; }
    currentPage = 1;
    renderTable();
});

partFilter.addEventListener('change', () => { currentPage = 1; renderTable(); });
actualityFilter.addEventListener('change', () => { currentPage = 1; renderTable(); });

if (clearSearchTextBtn) {
    clearSearchTextBtn.addEventListener('click', () => {
        searchInput.value = '';
        currentPage = 1;
        renderTable();
    });
}

if (clearSearchRangeBtn) {
    clearSearchRangeBtn.addEventListener('click', () => {
        searchPriceMin.value = '';
        searchPriceMax.value = '';
        currentPage = 1;
        renderTable();
    });
}

// === RENDERING ===

function renderTable() {
    const tbody = document.querySelector('#pricesTable tbody');
    if (!tbody) return;

    // Сбор параметров
    let searchVal = (searchInput.value || '').trim();
    if (searchVal.includes(',')) searchVal = searchVal.replace(',', '.');

    const method = getSearchMethod();
    const viewMode = getViewMode();

    let minPrice = parseFloat(searchPriceMin.value);
    let maxPrice = parseFloat(searchPriceMax.value);
    if (isNaN(minPrice)) minPrice = null;
    if (isNaN(maxPrice)) maxPrice = null;

    const filterPartId = partFilter.value ? Number(partFilter.value) : null;
    const filterActuality = actualityFilter.value;
    const dateFrom = dateFromFilter.value ? new Date(dateFromFilter.value) : null;
    const dateTo = dateToFilter.value ? new Date(dateToFilter.value) : null;
    if (dateTo) dateTo.setHours(23, 59, 59, 999);

    tbody.innerHTML = '';

    // Логика фильтрации
    const checkStaticFilters = (p) => {
        if (filterPartId !== null && p.partId !== filterPartId) return false;

        if (p.date) {
            const pDate = new Date(p.date);
            if (dateFrom && pDate < dateFrom) return false;
            if (dateTo && pDate > dateTo) return false;
        }

        if (filterActuality === 'actual') {
            if (!latestPriceIds.has(p.id)) return false;
        } else if (filterActuality === 'archived') {
            if (latestPriceIds.has(p.id)) return false;
        }
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
            if (method === 'exact') {
                return val === Number(searchVal);
            } else {
                return strVal.startsWith(searchVal);
            }
        }
    };

    let matches = [];
    let displayList = [];

    const staticallyFiltered = cached_prices.filter(checkStaticFilters);

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

    prices = displayList;

    if (pricesCount) {
        const total = cached_prices.length;
        const filtered = matches.length;

        if (viewMode === 'filter') {
            pricesCount.textContent = `Цен: ${filtered} из ${total}`;
        } else {
            pricesCount.textContent = `Цен: ${prices.length} (совпадений: ${filtered}) из ${total}`;
        }
    }

    if (prices.length > 0) {
        const partsMap = new Map(cached_parts.map(pt => [pt.id, pt]));

        prices.sort((a, b) => {
            const dir = sortDirection === 'asc' ? 1 : -1;
            let va, vb;
            switch (sortField) {
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

    currentPage = updatePaginationUI(currentPage, prices.length, perPage, pageInfo, prevBtn, nextBtn);

    if (prices.length === 0) {
        tbody.innerHTML = '<tr><td colspan="4" class="text-center" style="font-style:italic; color:var(--text-muted);">Записей не найдено</td></tr>';
        return;
    }

    const pageItems = paginateData(prices, currentPage, perPage);
    const partsMap = new Map(cached_parts.map(pt => [pt.id, pt]));
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

        // Actions with col-fit
        const tdActions = document.createElement('td');
        tdActions.classList.add('col-fit');

        const isLatest = latestPriceIds.has(item.id);
        const priceDateMs = new Date(item.date).getTime();

        // **ЛОГИКА ПРОВЕРКИ ЛИМИТА РЕДАКТИРОВАНИЯ (7 дней)**
        const isEditable = (CURRENT_TIME - priceDateMs) < EDIT_LIMIT_MS;

        if (isLatest && isEditable) {
            const actionsDiv = document.createElement('div');
            actionsDiv.className = 'actions-group justify-content-end';

            const editBtn = document.createElement('button');
            editBtn.textContent = '✎';
            editBtn.title = 'Изменить';
            editBtn.className = 'btn btn-sm btn-primary btn-square';
            editBtn.addEventListener('click', () => openEditModal(item));

            const deleteBtn = document.createElement('button');
            deleteBtn.textContent = '🗑';
            deleteBtn.title = 'Удалить';
            deleteBtn.className = 'btn btn-sm btn-danger btn-square';
            deleteBtn.addEventListener('click', () => openDeleteModal(item));

            actionsDiv.appendChild(editBtn);
            actionsDiv.appendChild(deleteBtn);
            tdActions.appendChild(actionsDiv);
        } else {
            // ОБНОВЛЕННЫЙ СТИЛЬ ЗАМКА (Как у оплаченных заказов)
            const lockSpan = document.createElement('span');
            lockSpan.textContent = '🔒';

            // Текст подсказки зависит от причины блокировки
            lockSpan.title = !isLatest
                ? 'Архивная запись (есть более свежая цена)'
                : 'Лимит редактирования (7 дней) истек';

            lockSpan.style.fontSize = '1.2rem'; // Крупный размер
            lockSpan.style.opacity = '0.6';     // Полупрозрачность
            lockSpan.style.cursor = 'help';     // Курсор вопроса при наведении

            // Контейнер для центрирования
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
    tbody.appendChild(fragment);
}

// === MODALS & ACTIONS ===

function openEditModal(item) {
    // ВАЛИДАЦИЯ ПЕРЕД ОТКРЫТИЕМ: Двойная проверка на всякий случай
    const priceDateMs = new Date(item.date).getTime();
    if ((CURRENT_TIME - priceDateMs) >= EDIT_LIMIT_MS) {
        alert('Ошибка: Редактирование этой записи невозможно, так как прошло более 7 дней с даты установки цены.');
        return;
    }

    editIdInput.value = item.id;
    editPartIdInput.value = item.partId;

    const part = cached_parts.find(p => p.id === item.partId);
    editPartNameDisplay.textContent = part ? part.name : 'Неизвестная деталь';

    document.getElementById('editValue').value = item.value;

    const d = new Date(item.date);
    d.setMinutes(d.getMinutes() - d.getTimezoneOffset()); // Коррекция для ISO
    const isoDate = d.toISOString().slice(0, 16);

    // ОБНОВЛЕНИЕ ПИКЕРА
    if (fpEditDate) {
        fpEditDate.setDate(isoDate);

        // Логика minDate для модалки
        const history = cached_prices
            .filter(p => p.partId === item.partId && p.id !== item.id)
            .sort((a, b) => new Date(b.date) - new Date(a.date));

        if (history.length > 0) {
            const prevDate = new Date(history[0].date);
            fpEditDate.set("minDate", prevDate);
        } else {
            fpEditDate.set("minDate", null);
        }
    }

    showModal(editModal);
}

function openDeleteModal(item) {
    // ВАЛИДАЦИЯ ПЕРЕД ОТКРЫТИЕМ: Двойная проверка на всякий случай
    const priceDateMs = new Date(item.date).getTime();
    if ((CURRENT_TIME - priceDateMs) >= EDIT_LIMIT_MS) {
        alert('Ошибка: Удаление этой записи невозможно, так как прошло более 7 дней с даты установки цены.');
        return;
    }

    deleteIdInput.value = item.id;
    showModal(deleteModal);
}

// === VALIDATION & FORM HANDLING ===

function validatePriceForm() {
    const errors = [];
    const valueInput = document.getElementById('editValue');
    const dateInput = document.getElementById('editDate');

    if (!valueInput.value) {
        errors.push('Поле "Стоимость" обязательно.');
    } else if (Number(valueInput.value) <= 0) {
        errors.push('Поле "Стоимость" должно быть больше 0.');
    }

    if (!dateInput.value) {
        errors.push('Поле "Дата установки" обязательно.');
    } else if (dateInput.min && dateInput.value < dateInput.min) {
        const minDatePretty = new Date(dateInput.min).toLocaleString('ru-RU');
        errors.push(`Дата не может быть раньше предыдущей записи (${minDatePretty}).`);
    }

    // **ВАЛИДАЦИЯ ЛИМИТА (7 дней)**
    // Проверяем, что дата, которую пытаются изменить, не выходит за лимит
    const editedPriceId = document.getElementById('editId').value;
    const originalPrice = cached_prices.find(p => p.id == editedPriceId);
    if (originalPrice) {
        const originalPriceDateMs = new Date(originalPrice.date).getTime();
        if ((CURRENT_TIME - originalPriceDateMs) >= EDIT_LIMIT_MS) {
            errors.push('Лимит редактирования (7 дней) истек. Изменение цены невозможно.');
        }
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

    if (e.target.id === 'editForm' && !validatePriceForm()) return;

    // Дополнительная проверка лимита перед отправкой запроса (если это удаление)
    if (e.target.id === 'deleteForm') {
        const deletePriceId = document.getElementById('deleteId').value;
        const priceToDelete = cached_prices.find(p => p.id == deletePriceId);
        if (priceToDelete) {
            const priceDateMs = new Date(priceToDelete.date).getTime();
            if ((CURRENT_TIME - priceDateMs) >= EDIT_LIMIT_MS) {
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
            errorList.innerHTML = `<li>${result.message || 'Ошибка операции'}</li>`;
            showModal(errorModal);
        } else {
            hideModal(modalToClose);
            await initData();
            recalculateLatestPermissions();
            renderTable();
        }
    } catch (ex) {
        console.error(ex);
        const errorList = document.getElementById('errorList');
        errorList.innerHTML = `<li>Произошла системная ошибка</li>`;
        showModal(errorModal);
    } finally {
        toggleLoader(false);
    }
}

editForm.addEventListener('submit', (e) => handleFormSubmit(e, 'UpdatePrice', editModal));
deleteForm.addEventListener('submit', (e) => handleFormSubmit(e, 'DeletePrice', deleteModal));

// === AUTO DECIMAL CORRECTION ===

function initAutoDecimalCorrection() {
    const decimalInputIds = [
        'searchInput',      // Поиск
        'searchPriceMin',   // Фильтр мин. цены
        'searchPriceMax',   // Фильтр макс. цены
        'editValue'         // Редактирование цены
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

// === CONTROLS ===
prevBtn.addEventListener('click', () => { if (currentPage > 1) { currentPage--; renderTable(); } });
nextBtn.addEventListener('click', () => {
    const totalPages = Math.ceil(prices.length / perPage);
    if (currentPage < totalPages) { currentPage++; renderTable(); }
});

searchButton.addEventListener('click', () => { currentPage = 1; renderTable(); });

searchInput.addEventListener('keyup', (e) => { if (e.key === 'Enter') { currentPage = 1; renderTable(); } });
searchPriceMin.addEventListener('keyup', (e) => { if (e.key === 'Enter') { currentPage = 1; renderTable(); } });
searchPriceMax.addEventListener('keyup', (e) => { if (e.key === 'Enter') { currentPage = 1; renderTable(); } });