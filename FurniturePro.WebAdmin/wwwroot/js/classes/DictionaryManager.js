import { syncTable } from '../api/syncService.js';
import { getAll } from '../db/repository.js';
import { toggleLoader } from '../ui/loader.js';
import { showModal, hideModal } from '../ui/modal.js';
import { attachRowSelection, updateHeaderSortUI } from '../ui/tables.js';
import { getSearchMode, initSearchModeSwitcher } from '../utils/domHelper.js';
import { paginateData, updatePaginationUI } from '../utils/pagination.js';

export class DictionaryManager {
    /**
     * @param {string} entityPlural - Название во мн. числе для БД и синхронизации (например, 'Categories')
     * @param {string} entitySingular - Название в ед. числе для обработчиков Razor (например, 'Category')
     * @param {string} tableId - ID таблицы (например, 'categoriesTable')
     * @param {string} createBtnId - ID кнопки создания (например, 'createCategoryButton')
     * @param {string} countId - ID элемента для вывода счетчика (например, 'categoriesCount')
     */
    constructor(entityPlural, entitySingular, tableId, createBtnId, countId) {
        this.entityPlural = entityPlural;
        this.entitySingular = entitySingular;

        // State
        this.cachedData = [];
        this.currentData = [];
        this.currentPage = 1;
        this.perPage = 20;
        this.sortField = null;
        this.sortDirection = 'asc';

        // Specific UI Elements
        this.table = document.getElementById(tableId);
        this.createButton = document.getElementById(createBtnId);
        this.countElement = document.getElementById(countId);

        // Common UI Elements (одинаковые на всех страницах)
        this.searchInput = document.getElementById('searchInput');
        this.clearSearchTextBtn = document.getElementById('clearSearchText');
        this.prevBtn = document.getElementById('prev');
        this.nextBtn = document.getElementById('next');
        this.pageInfo = document.getElementById('pageInfo');

        // Modals
        this.createModal = document.getElementById('createModal');
        this.editModal = document.getElementById('editModal');
        this.deleteModal = document.getElementById('deleteModal');
        this.descriptionModal = document.getElementById('descriptionModal');
        this.errorModal = document.getElementById('errorModal');

        // Forms & Inputs
        this.createForm = document.getElementById('createForm');
        this.editForm = document.getElementById('editForm');
        this.deleteForm = document.getElementById('deleteForm');
        this.editIdInput = document.getElementById('editId');
        this.deleteIdInput = document.getElementById('deleteId');
        this.descriptionContent = document.getElementById('descriptionContent');

        this.init();
    }

    async init() {
        if (!this.table) return;

        this.headers = this.table.querySelectorAll('thead th');
        this.headers.forEach((th, index) => {
            th.style.cursor = 'pointer';
            th.addEventListener('click', () => this.onHeaderClick(index, th));
        });

        this.attachEvents();

        try {
            toggleLoader(true);
            initSearchModeSwitcher(() => this.renderTable());
            attachRowSelection(`#${this.table.id} tbody`);

            await this.loadData();
            this.renderTable();
        } finally {
            toggleLoader(false);
        }

        this.startAutoSync();
    }

    attachEvents() {
        if (this.createButton) {
            this.createButton.addEventListener('click', () => {
                if (this.createForm) this.createForm.reset();
                showModal(this.createModal);
            });
        }

        if (this.clearSearchTextBtn) {
            this.clearSearchTextBtn.addEventListener('click', () => {
                if (this.searchInput) this.searchInput.value = '';
                this.currentPage = 1;
                this.renderTable();
            });
        }

        if (this.prevBtn) {
            this.prevBtn.addEventListener('click', () => {
                if (this.currentPage > 1) { this.currentPage--; this.renderTable(); }
            });
        }
        if (this.nextBtn) {
            this.nextBtn.addEventListener('click', () => {
                const totalPages = Math.ceil(this.currentData.length / this.perPage);
                if (this.currentPage < totalPages) { this.currentPage++; this.renderTable(); }
            });
        }

        if (this.createForm) this.createForm.addEventListener('submit', (e) => this.handleFormSubmit(e, `Create${this.entitySingular}`, this.createModal));
        if (this.editForm) this.editForm.addEventListener('submit', (e) => this.handleFormSubmit(e, `Update${this.entitySingular}`, this.editModal));
        if (this.deleteForm) this.deleteForm.addEventListener('submit', (e) => this.handleFormSubmit(e, `Delete${this.entitySingular}`, this.deleteModal));
    }

    async loadData() {
        await syncTable(this.entityPlural);
        const data = await getAll(this.entityPlural);
        this.cachedData = data || [];
    }

    startAutoSync() {
        const loop = async () => {
            await this.loadData();
            this.renderTable();
            setTimeout(loop, 5 * 60 * 1000);
        };
        loop();
    }

    onHeaderClick(index, th) {
        const fieldMap = ['name', 'description', 'actions'];
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
        const tbody = this.table.querySelector('tbody');
        if (!tbody) return;

        const search = (this.searchInput?.value || '').toLowerCase().trim();
        tbody.innerHTML = '';

        let findedItems = this.cachedData.filter(item => {
            if (search) {
                const haystack = [item.name, item.description].map(v => (v ?? '').toString().toLowerCase()).join(' ');
                if (!haystack.includes(search)) return false;
            }
            return true;
        });

        const mode = getSearchMode();
        let isHighlight = false;

        if (mode === 'filter') {
            this.currentData = findedItems;
        } else if (mode === 'highlight') {
            this.currentData = this.cachedData;
            isHighlight = true;
        }

        if (this.countElement) {
            const total = this.cachedData.length;
            const filtered = findedItems.length;
            // Упрощенный падеж для счетчика или можно передавать кастомную строку
            this.countElement.textContent = mode === 'filter'
                ? `Записей: ${filtered} из ${total}`
                : `Записей: ${this.currentData.length} (совпадений: ${filtered}) из ${total}`;
        }

        if (this.currentData.length > 0 && this.sortField) {
            this.currentData.sort((a, b) => {
                const dir = this.sortDirection === 'asc' ? 1 : -1;
                const va = (a[this.sortField] || '').toLowerCase();
                const vb = (b[this.sortField] || '').toLowerCase();
                return va < vb ? -1 * dir : (va > vb ? 1 * dir : 0);
            });
        }

        this.currentPage = updatePaginationUI(this.currentPage, this.currentData.length, this.perPage, this.pageInfo, this.prevBtn, this.nextBtn);

        if (this.currentData.length === 0) {
            tbody.innerHTML = '<tr><td colspan="3" class="text-center">Записей нет</td></tr>';
            return;
        }

        const pageData = paginateData(this.currentData, this.currentPage, this.perPage);
        const fragment = document.createDocumentFragment();

        for (const item of pageData) {
            const tr = document.createElement('tr');
            if (isHighlight && findedItems.includes(item)) tr.classList.add('highlight-match');

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
                this.descriptionContent.textContent = item.description || 'Описания нет';
                showModal(this.descriptionModal);
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
            editBtn.addEventListener('click', () => this.openEditModal(item));

            const deleteBtn = document.createElement('button');
            deleteBtn.textContent = '🗑';
            deleteBtn.title = 'Удалить';
            deleteBtn.className = 'btn btn-sm btn-danger btn-square';
            deleteBtn.addEventListener('click', () => this.openDeleteModal(item));

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

    openEditModal(item) {
        if (this.editIdInput) this.editIdInput.value = item.id;
        const editName = document.getElementById('editName');
        const editDesc = document.getElementById('editDescription');
        if (editName) editName.value = item.name || '';
        if (editDesc) editDesc.value = item.description || '';
        showModal(this.editModal);
    }

    openDeleteModal(item) {
        if (this.deleteIdInput) this.deleteIdInput.value = item.id;
        showModal(this.deleteModal);
    }

    validateForm(prefix) {
        const errors = [];
        const nameInput = document.getElementById(prefix + 'Name');
        if (nameInput && !nameInput.value.trim()) errors.push('Поле "Название" обязательно.');

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

        if (e.target.id !== 'deleteForm') {
            const formPrefix = e.target.id.replace('Form', '');
            if (!this.validateForm(formPrefix)) return;
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
                this.renderTable();
            }
        } finally {
            toggleLoader(false);
        }
    }
}