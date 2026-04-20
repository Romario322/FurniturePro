// wwwroot/js/pages/clients.js

let cached_clients = [];
let clients = [];

let currentPage = 1;
const perPage = 20;
let sortField = null;
let sortDirection = 'asc';

// --- UI Elements ---
const searchInput = document.getElementById('searchInput');
const searchButton = document.getElementById('searchButton');
const clearSearchTextBtn = document.getElementById('clearSearchText');
const emailDomainFilter = document.getElementById('emailDomainFilter'); // New
const createClientButton = document.getElementById('createClientButton');
const clientsCount = document.getElementById('clientsCount');

// --- Modals ---
const createModal = document.getElementById('createModal');
const editModal = document.getElementById('editModal');
const deleteModal = document.getElementById('deleteModal');
const errorModal = document.getElementById('errorModal');

// --- Forms ---
const createForm = document.getElementById('createForm');
const editForm = document.getElementById('editForm');
const deleteForm = document.getElementById('deleteForm');

// --- Specific Inputs ---
const editIdInput = document.getElementById('editId');
const deleteIdInput = document.getElementById('deleteId');
// Phone inputs
const createPhoneInput = document.getElementById('createPhone');
const editPhoneInput = document.getElementById('editPhone');

// --- Pagination & Table ---
const prevBtn = document.getElementById('prev');
const nextBtn = document.getElementById('next');
const pageInfo = document.getElementById('pageInfo');
const table = document.getElementById('clientsTable');
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
        attachRowSelection('#clientsTable tbody');

        // Инициализация масок
        initPhoneMask(createPhoneInput);
        initPhoneMask(editPhoneInput);

        await initClients();
        initCustomSelects(); // Если используете кастомные селекты
        renderTable();
    } finally {
        toggleLoader(false);
    }
    startAutoSync();
});

// === PHONE MASK LOGIC ===
function initPhoneMask(input) {
    if (!input) return;

    const setCursorPosition = (pos, elem) => {
        elem.focus();
        if (elem.setSelectionRange) elem.setSelectionRange(pos, pos);
        else if (elem.createTextRange) {
            const range = elem.createTextRange();
            range.collapse(true);
            range.moveEnd("character", pos);
            range.moveStart("character", pos);
            range.select();
        }
    };

    const mask = (event) => {
        const matrix = "+7 (___) ___-__-__";
        let i = 0;
        let def = matrix.replace(/\D/g, "");
        let val = input.value.replace(/\D/g, "");

        // Если пользователь начинает вводить с 8 или 9, меняем на 7 для корректности РФ
        if (def.length >= val.length) val = def;
        if (val[0] === '8') val = '7' + val.substring(1);

        input.value = matrix.replace(/./g, function (a) {
            return /[_\d]/.test(a) && i < val.length ? val.charAt(i++) : i >= val.length ? "" : a;
        });

        if (event.type === "blur") {
            if (input.value.length == 2) input.value = "";
        } else {
            setCursorPosition(input.value.length, input);
        }
    };

    input.addEventListener("input", mask, false);
    input.addEventListener("focus", mask, false);
    input.addEventListener("blur", mask, false);
}

// === DATA FETCHING ===

async function initClients() {
    await syncTable('Clients');
    const clientsData = await getAll('Clients');
    cached_clients = clientsData || [];

    updateEmailFilterOptions();
}

function updateEmailFilterOptions() {
    if (!emailDomainFilter) return;

    // Сохраняем текущее значение, если есть
    const currentVal = emailDomainFilter.value;

    // Получаем уникальные домены
    const domains = new Set();
    cached_clients.forEach(c => {
        if (c.email && c.email.includes('@')) {
            const domain = c.email.split('@')[1].toLowerCase();
            domains.add(domain);
        }
    });

    // Очищаем и заполняем заново (кроме первой опции "Все")
    while (emailDomainFilter.options.length > 1) {
        emailDomainFilter.remove(1);
    }

    Array.from(domains).sort().forEach(domain => {
        const opt = document.createElement('option');
        opt.value = '@' + domain;
        opt.textContent = '@' + domain;
        emailDomainFilter.appendChild(opt);
    });

    emailDomainFilter.value = currentVal;

    // Обновляем кастомный селект, если он используется
    if (emailDomainFilter.parentNode.classList.contains('custom-select-wrapper')) {
        // Простой хак: пересоздаем обертку
        new CustomSelect(emailDomainFilter);
    }
}

function startAutoSync() {
    async function loop() {
        await initClients();
        renderTable();
        setTimeout(loop, 5 * 60 * 1000);
    }
    loop();
}

// === SORTING ===

function onHeaderClick(index, th) {
    const fieldMap = ['fullName', 'phone', 'email', 'actions'];
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
    const tbody = document.querySelector('#clientsTable tbody');
    if (!tbody) return;

    const search = (searchInput.value || '').toLowerCase().trim();
    const domainFilter = emailDomainFilter ? emailDomainFilter.value : '';

    tbody.innerHTML = '';

    // 1. Фильтрация
    let finded_clients = cached_clients.filter(c => {
        if (search) {
            const haystack = [c.fullName, c.phone, c.email].map(v => (v ?? '').toString().toLowerCase()).join(' ');
            if (!haystack.includes(search)) return false;
        }
        if (domainFilter) {
            if (!c.email || !c.email.toLowerCase().endsWith(domainFilter)) return false;
        }
        return true;
    });

    // 2. Режим отображения (Фильтр / Выделение)
    const mode = getSearchMode();
    let isHighlight = false;

    if (mode === 'filter') {
        clients = finded_clients;
    } else if (mode === 'highlight') {
        clients = cached_clients;
        if (domainFilter) {
            clients = clients.filter(c => c.email && c.email.toLowerCase().endsWith(domainFilter));
        }
        isHighlight = true;
    }

    // Обновление счетчика
    if (clientsCount) {
        const total = cached_clients.length;
        const filtered = finded_clients.length;
        clientsCount.textContent = mode === 'filter'
            ? `Клиентов: ${filtered} из ${total}`
            : `Клиентов: ${clients.length} (совпадений: ${filtered}) из ${total}`;
    }

    // 3. Сортировка
    if (clients.length > 0 && sortField) {
        clients.sort((a, b) => {
            const dir = sortDirection === 'asc' ? 1 : -1;
            const getValue = (item) => {
                switch (sortField) {
                    case 'fullName': return (item.fullName || '').toLowerCase();
                    case 'phone': return (item.phone || '').toLowerCase();
                    case 'email': return (item.email || '').toLowerCase();
                    default: return '';
                }
            };
            const va = getValue(a);
            const vb = getValue(b);
            return va < vb ? -1 * dir : (va > vb ? 1 * dir : 0);
        });
    }

    // Пагинация UI
    currentPage = updatePaginationUI(currentPage, clients.length, perPage, pageInfo, prevBtn, nextBtn);

    if (clients.length === 0) {
        tbody.innerHTML = '<tr><td colspan="4" class="text-center" style="opacity:0.6; padding:1rem;">Записей нет</td></tr>';
        return;
    }

    // 4. Рендер строк
    const pageClients = paginateData(clients, currentPage, perPage);
    const fragment = document.createDocumentFragment();

    for (const item of pageClients) {
        const tr = document.createElement('tr');
        if (isHighlight && finded_clients.includes(item)) {
            tr.classList.add('highlight-match');
        }

        // Full Name
        const tdName = document.createElement('td');
        tdName.textContent = item.fullName;
        tdName.style.fontWeight = '500';

        // Phone (с кнопкой копирования)
        const tdPhone = document.createElement('td');
        tdPhone.appendChild(createCopyableContent(item.phone));

        // Email (с кнопкой копирования)
        const tdEmail = document.createElement('td');
        tdEmail.appendChild(createCopyableContent(item.email));

        // Actions
        const tdActions = document.createElement('td');
        tdActions.classList.add('col-fit');
        const actionsDiv = document.createElement('div');
        actionsDiv.className = 'actions-group justify-content-end';

        const editBtn = document.createElement('button');
        editBtn.textContent = '✎';
        editBtn.title = 'Изменить';
        editBtn.className = 'btn btn-sm btn-primary btn-square';
        editBtn.onclick = (e) => { e.stopPropagation(); openEditModal(item); };

        const deleteBtn = document.createElement('button');
        deleteBtn.textContent = '🗑';
        deleteBtn.title = 'Удалить';
        deleteBtn.className = 'btn btn-sm btn-danger btn-square';
        deleteBtn.onclick = (e) => { e.stopPropagation(); openDeleteModal(item); };

        actionsDiv.appendChild(editBtn);
        actionsDiv.appendChild(deleteBtn);
        tdActions.appendChild(actionsDiv);

        tr.appendChild(tdName);
        tr.appendChild(tdPhone);
        tr.appendChild(tdEmail);
        tr.appendChild(tdActions);

        fragment.appendChild(tr);
    }
    tbody.appendChild(fragment);
}

// === FORM HANDLING ===

createClientButton.addEventListener('click', () => {
    if (createForm) {
        createForm.reset();
        // Сброс маски (визуальный)
        // input event не срабатывает при reset, можно вручную очистить
    }
    showModal(createModal);
});

function openEditModal(item) {
    editIdInput.value = item.id;
    document.getElementById('editFullName').value = item.fullName || '';
    document.getElementById('editPhone').value = item.phone || '';
    document.getElementById('editEmail').value = item.email || '';
    showModal(editModal);
}

function openDeleteModal(item) {
    deleteIdInput.value = item.id;
    showModal(deleteModal);
}

function validateForm(prefix) {
    const errors = [];
    const name = document.getElementById(prefix + 'FullName');
    const phone = document.getElementById(prefix + 'Phone');

    if (!name.value.trim()) errors.push('Поле "ФИО" обязательно.');

    // Проверка телефона по длине маски (+7 (999) 999 99-99 = 18 символов)
    if (!phone.value.trim()) {
        errors.push('Поле "Телефон" обязательно.');
    } else if (phone.value.length < 18) {
        errors.push('Телефон должен быть введен полностью.');
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
    const totalPages = Math.ceil(clients.length / perPage);
    if (currentPage < totalPages) { currentPage++; renderTable(); }
});

async function handleFormSubmit(e, handlerName, modalToClose) {
    e.preventDefault();

    if (e.target.id !== 'deleteForm') {
        const formPrefix = e.target.id.replace('Form', '');
        if (!validateForm(formPrefix)) return;
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
            await initClients();
            renderTable();
        }
    } finally {
        toggleLoader(false);
    }
}

createForm.addEventListener('submit', (e) => handleFormSubmit(e, 'CreateClient', createModal));
editForm.addEventListener('submit', (e) => handleFormSubmit(e, 'UpdateClient', editModal));
deleteForm.addEventListener('submit', (e) => handleFormSubmit(e, 'DeleteClient', deleteModal));

if (clearSearchTextBtn) {
    clearSearchTextBtn.addEventListener('click', () => {
        searchInput.value = '';
        currentPage = 1;
        renderTable();
    });
}

// Слушатель изменения фильтра
if (emailDomainFilter) {
    emailDomainFilter.addEventListener('change', () => {
        currentPage = 1;
        renderTable();
    });
}

function createCopyableContent(text) {
    const wrapper = document.createElement('div');
    wrapper.className = 'd-flex align-items-center justify-content-between gap-2';

    // Текст
    const span = document.createElement('span');
    span.textContent = text || '';
    wrapper.appendChild(span);

    // Кнопка (только если есть текст)
    if (text) {
        const btn = document.createElement('button');
        // Добавлен класс 'btn-square' для квадратной формы
        btn.className = 'btn-copy-action btn-square';
        btn.innerHTML = '📋';
        btn.title = 'Скопировать';

        // Убрали ручной padding, так как btn-square сам всё настроит
        // btn.style.padding = '0 5px'; <--- УДАЛЕНО

        btn.onclick = (e) => {
            e.stopPropagation(); // Чтобы клик не открывал модалку редактирования (если есть на строке)
            copyToClipboard(text, btn);
        };
        wrapper.appendChild(btn);
    }

    return wrapper;
}

function copyToClipboard(text, btnElement) {
    if (!text) return;
    navigator.clipboard.writeText(text).then(() => {
        if (btnElement) {
            const originalHTML = btnElement.innerHTML;
            btnElement.innerHTML = '✓';
            btnElement.style.color = 'var(--action-create)'; // Зеленый из темы
            setTimeout(() => {
                btnElement.innerHTML = originalHTML;
                btnElement.style.color = '';
            }, 1000);
        }
    }).catch(err => console.error('Copy failed', err));
}