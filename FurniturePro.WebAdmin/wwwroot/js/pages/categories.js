// wwwroot/js/pages/categories.js

let cached_categories = [];
let categories = [];

let currentPage = 1;
const perPage = 20;
let sortField = null;
let sortDirection = 'asc';

// --- UI Elements ---
const searchInput = document.getElementById('searchInput');
const searchButton = document.getElementById('searchButton');
const clearSearchTextBtn = document.getElementById('clearSearchText');
const createCategoryButton = document.getElementById('createCategoryButton');
const categoriesCount = document.getElementById('categoriesCount');

// --- Modals ---
const createModal = document.getElementById('createModal');
const editModal = document.getElementById('editModal');
const deleteModal = document.getElementById('deleteModal');
const descriptionModal = document.getElementById('descriptionModal');
const errorModal = document.getElementById('errorModal');

// --- Forms ---
const createForm = document.getElementById('createForm');
const editForm = document.getElementById('editForm');
const deleteForm = document.getElementById('deleteForm');

// --- Specific Inputs ---
const editIdInput = document.getElementById('editId');
const deleteIdInput = document.getElementById('deleteId');
const descriptionContent = document.getElementById('descriptionContent');

// --- Pagination & Table ---
const prevBtn = document.getElementById('prev');
const nextBtn = document.getElementById('next');
const pageInfo = document.getElementById('pageInfo');
const table = document.getElementById('categoriesTable');
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
        attachRowSelection('#categoriesTable tbody');

        await initCategories();
        renderTable();
    } finally {
        toggleLoader(false);
    }
    startAutoSync();
});

// === DATA FETCHING ===

async function initCategories() {
    await syncTable('Categories');
    const categoriesData = await getAll('Categories');
    cached_categories = categoriesData || [];
}

function startAutoSync() {
    async function loop() {
        await initCategories();
        renderTable();
        setTimeout(loop, 5 * 60 * 1000);
    }
    loop();
}

// === SORTING ===

function onHeaderClick(index, th) {
    const fieldMap = ['name', 'description', 'actions'];
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
    const tbody = document.querySelector('#categoriesTable tbody');
    if (!tbody) return;

    const search = (searchInput.value || '').toLowerCase().trim();

    tbody.innerHTML = '';

    let finded_categories = cached_categories.filter(c => {
        if (search) {
            const haystack = [c.name, c.description].map(v => (v ?? '').toString().toLowerCase()).join(' ');
            if (!haystack.includes(search)) return false;
        }
        return true;
    });

    const mode = getSearchMode();
    let isHighlight = false;

    if (mode === 'filter') {
        categories = finded_categories;
    } else if (mode === 'highlight') {
        categories = cached_categories; // Show all
        isHighlight = true;
    }

    if (categoriesCount) {
        const total = cached_categories.length;
        const filtered = finded_categories.length;
        categoriesCount.textContent = mode === 'filter'
            ? `Категорий: ${filtered} из ${total}`
            : `Категорий: ${categories.length} (совпадений: ${filtered}) из ${total}`;
    }

    // Sorting
    if (categories.length > 0 && sortField) {
        categories.sort((a, b) => {
            const dir = sortDirection === 'asc' ? 1 : -1;
            const getValue = (item) => {
                switch (sortField) {
                    case 'name': return (item.name || '').toLowerCase();
                    case 'description': return (item.description || '').toLowerCase();
                    default: return '';
                }
            };
            const va = getValue(a);
            const vb = getValue(b);
            return va < vb ? -1 * dir : (va > vb ? 1 * dir : 0);
        });
    }

    currentPage = updatePaginationUI(currentPage, categories.length, perPage, pageInfo, prevBtn, nextBtn);

    if (categories.length === 0) {
        tbody.innerHTML = '<tr><td colspan="3" class="text-center">Записей нет</td></tr>';
        return;
    }

    const pageCategories = paginateData(categories, currentPage, perPage);
    const fragment = document.createDocumentFragment();

    for (const item of pageCategories) {
        const tr = document.createElement('tr');
        if (isHighlight && finded_categories.includes(item)) {
            tr.classList.add('highlight-match');
        }

        // Name
        const tdName = document.createElement('td');
        tdName.textContent = item.name;

        // Description
        const tdDescription = document.createElement('td');
        tdDescription.classList.add('col-fit');
        const descBtn = document.createElement('button');
        descBtn.textContent = '📄';
        descBtn.title = 'Описание';
        descBtn.className = 'btn btn-sm btn-description btn-square';
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

        tr.appendChild(tdName);
        tr.appendChild(tdDescription);
        tr.appendChild(tdActions);

        fragment.appendChild(tr);
    }
    tbody.appendChild(fragment);
}

// === FORM HANDLING ===

createCategoryButton.addEventListener('click', () => {
    if (createForm) {
        createForm.reset();
    }
    showModal(createModal);
});

function openEditModal(item) {
    editIdInput.value = item.id;
    document.getElementById('editName').value = item.name || '';
    document.getElementById('editDescription').value = item.description || '';
    showModal(editModal);
}

function openDeleteModal(item) {
    deleteIdInput.value = item.id;
    showModal(deleteModal);
}

function validateForm(prefix) {
    const errors = [];
    const name = document.getElementById(prefix + 'Name');

    if (!name.value.trim()) errors.push('Поле "Название" обязательно.');

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
    const totalPages = Math.ceil(categories.length / perPage);
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
            await initCategories();
            renderTable();
        }
    } finally {
        toggleLoader(false);
    }
}

createForm.addEventListener('submit', (e) => handleFormSubmit(e, 'CreateCategory', createModal));
editForm.addEventListener('submit', (e) => handleFormSubmit(e, 'UpdateCategory', editModal));
deleteForm.addEventListener('submit', (e) => handleFormSubmit(e, 'DeleteCategory', deleteModal));

if (clearSearchTextBtn) {
    clearSearchTextBtn.addEventListener('click', () => {
        searchInput.value = '';
        currentPage = 1;
        renderTable();
    });
}