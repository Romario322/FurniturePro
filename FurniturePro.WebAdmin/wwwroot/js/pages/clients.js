import { syncTable } from '../api/syncService.js';
import { getAll } from '../db/repository.js';
import { toggleLoader } from '../ui/loader.js';
import { showModal, hideModal } from '../ui/modal.js';
import { attachRowSelection, updateHeaderSortUI } from '../ui/tables.js';
import { getSearchMode, initSearchModeSwitcher } from '../utils/domHelper.js';
import { paginateData, updatePaginationUI } from '../utils/pagination.js';
import { initPhoneMask } from '../utils/phoneMask.js';
import { createCopyableContent } from '../utils/clipboard.js';

class ClientsPage {
    constructor() {
        this.cachedData = [];
        this.currentData = [];
        this.currentPage = 1;
        this.perPage = 20;
        this.sortField = null;
        this.sortDirection = 'asc';

        this.initDOM();
        this.attachEvents();
    }

    initDOM() {
        // Таблица и пагинация
        this.table = document.getElementById('clientsTable');
        this.tbody = this.table?.querySelector('tbody');
        this.headers = this.table?.querySelectorAll('thead th');
        this.pageInfo = document.getElementById('pageInfo');
        this.prevBtn = document.getElementById('prev');
        this.nextBtn = document.getElementById('next');

        // Поиск и фильтры
        this.searchInput = document.getElementById('searchInput');
        this.clearSearchTextBtn = document.getElementById('clearSearchText');
        this.emailDomainFilter = document.getElementById('emailDomainFilter');
        this.countElement = document.getElementById('clientsCount');

        // Кнопки и модалки
        this.createButton = document.getElementById('createClientButton');
        this.createModal = document.getElementById('createModal');
        this.editModal = document.getElementById('editModal');
        this.deleteModal = document.getElementById('deleteModal');
        this.errorModal = document.getElementById('errorModal');

        // Формы и инпуты
        this.createForm = document.getElementById('createForm');
        this.editForm = document.getElementById('editForm');
        this.deleteForm = document.getElementById('deleteForm');
        this.editIdInput = document.getElementById('editId');
        this.deleteIdInput = document.getElementById('deleteId');

        // Инициализация масок для телефонов
        initPhoneMask(document.getElementById('createPhone'));
        initPhoneMask(document.getElementById('editPhone'));
    }

    async init() {
        try {
            toggleLoader(true);
            initSearchModeSwitcher(() => this.renderTable());
            attachRowSelection('#clientsTable tbody');

            if (this.headers) {
                this.headers.forEach((th, index) => {
                    th.style.cursor = 'pointer';
                    th.addEventListener('click', () => this.onHeaderClick(index, th));
                });
            }

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

        if (this.emailDomainFilter) {
            this.emailDomainFilter.addEventListener('change', () => {
                this.currentPage = 1;
                this.renderTable();
            });
        }

        if (this.searchInput) {
            this.searchInput.addEventListener('input', () => {
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

        if (this.createForm) this.createForm.addEventListener('submit', (e) => this.handleFormSubmit(e, 'CreateClient', this.createModal));
        if (this.editForm) this.editForm.addEventListener('submit', (e) => this.handleFormSubmit(e, 'UpdateClient', this.editModal));
        if (this.deleteForm) this.deleteForm.addEventListener('submit', (e) => this.handleFormSubmit(e, 'DeleteClient', this.deleteModal));
    }

    async loadData() {
        await syncTable('Clients');
        this.cachedData = await getAll('Clients') || [];
        this.updateEmailFilterOptions();
    }

    updateEmailFilterOptions() {
        if (!this.emailDomainFilter) return;

        const currentVal = this.emailDomainFilter.value;
        const domains = new Set();

        this.cachedData.forEach(c => {
            if (c.email && c.email.includes('@')) {
                const domain = c.email.split('@')[1].toLowerCase();
                domains.add(domain);
            }
        });

        while (this.emailDomainFilter.options.length > 1) {
            this.emailDomainFilter.remove(1);
        }

        Array.from(domains).sort().forEach(domain => {
            const opt = document.createElement('option');
            opt.value = '@' + domain;
            opt.textContent = '@' + domain;
            this.emailDomainFilter.appendChild(opt);
        });

        this.emailDomainFilter.value = currentVal;
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
        const fieldMap = ['fullName', 'phone', 'email', 'actions'];
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
        const domainFilter = this.emailDomainFilter ? this.emailDomainFilter.value : '';

        this.tbody.innerHTML = '';

        let findedItems = this.cachedData.filter(c => {
            if (search) {
                const haystack = [c.fullName, c.phone, c.email].map(v => (v ?? '').toString().toLowerCase()).join(' ');
                if (!haystack.includes(search)) return false;
            }
            if (domainFilter) {
                if (!c.email || !c.email.toLowerCase().endsWith(domainFilter)) return false;
            }
            return true;
        });

        const mode = getSearchMode();
        let isHighlight = false;

        if (mode === 'filter') {
            this.currentData = findedItems;
        } else if (mode === 'highlight') {
            this.currentData = this.cachedData;
            if (domainFilter) {
                this.currentData = this.currentData.filter(c => c.email && c.email.toLowerCase().endsWith(domainFilter));
            }
            isHighlight = true;
        }

        if (this.countElement) {
            const total = this.cachedData.length;
            const filtered = findedItems.length;
            this.countElement.textContent = mode === 'filter'
                ? `Клиентов: ${filtered} из ${total}`
                : `Клиентов: ${this.currentData.length} (совпадений: ${filtered}) из ${total}`;
        }

        if (this.currentData.length > 0 && this.sortField) {
            this.currentData.sort((a, b) => {
                const dir = this.sortDirection === 'asc' ? 1 : -1;
                const getValue = (item) => {
                    switch (this.sortField) {
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

        this.currentPage = updatePaginationUI(this.currentPage, this.currentData.length, this.perPage, this.pageInfo, this.prevBtn, this.nextBtn);

        if (this.currentData.length === 0) {
            this.tbody.innerHTML = '<tr><td colspan="4" class="text-center" style="opacity:0.6; padding:1rem;">Записей нет</td></tr>';
            return;
        }

        const pageData = paginateData(this.currentData, this.currentPage, this.perPage);
        const fragment = document.createDocumentFragment();

        for (const item of pageData) {
            const tr = document.createElement('tr');
            if (isHighlight && findedItems.includes(item)) {
                tr.classList.add('highlight-match');
            }

            const tdName = document.createElement('td');
            tdName.textContent = item.fullName;
            tdName.style.fontWeight = '500';

            const tdPhone = document.createElement('td');
            tdPhone.appendChild(createCopyableContent(item.phone));

            const tdEmail = document.createElement('td');
            tdEmail.appendChild(createCopyableContent(item.email));

            const tdActions = document.createElement('td');
            tdActions.classList.add('col-fit');
            const actionsDiv = document.createElement('div');
            actionsDiv.className = 'actions-group justify-content-end';

            const editBtn = document.createElement('button');
            editBtn.textContent = '✎';
            editBtn.title = 'Изменить';
            editBtn.className = 'btn btn-sm btn-primary btn-square';
            editBtn.onclick = (e) => { e.stopPropagation(); this.openEditModal(item); };

            const deleteBtn = document.createElement('button');
            deleteBtn.textContent = '🗑';
            deleteBtn.title = 'Удалить';
            deleteBtn.className = 'btn btn-sm btn-danger btn-square';
            deleteBtn.onclick = (e) => { e.stopPropagation(); this.openDeleteModal(item); };

            actionsDiv.appendChild(editBtn);
            actionsDiv.appendChild(deleteBtn);
            tdActions.appendChild(actionsDiv);

            tr.appendChild(tdName);
            tr.appendChild(tdPhone);
            tr.appendChild(tdEmail);
            tr.appendChild(tdActions);

            fragment.appendChild(tr);
        }
        this.tbody.appendChild(fragment);
    }

    openEditModal(item) {
        if (this.editIdInput) this.editIdInput.value = item.id;
        document.getElementById('editFullName').value = item.fullName || '';
        document.getElementById('editPhone').value = item.phone || '';
        document.getElementById('editEmail').value = item.email || '';
        showModal(this.editModal);
    }

    openDeleteModal(item) {
        if (this.deleteIdInput) this.deleteIdInput.value = item.id;
        showModal(this.deleteModal);
    }

    validateForm(prefix) {
        const errors = [];
        const name = document.getElementById(prefix + 'FullName');
        const phone = document.getElementById(prefix + 'Phone');

        if (!name.value.trim()) errors.push('Поле "ФИО" обязательно.');

        if (!phone.value.trim()) {
            errors.push('Поле "Телефон" обязательно.');
        } else if (phone.value.length < 18) {
            errors.push('Телефон должен быть введен полностью.');
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

document.addEventListener('DOMContentLoaded', () => {
    const page = new ClientsPage();
    page.init();
});