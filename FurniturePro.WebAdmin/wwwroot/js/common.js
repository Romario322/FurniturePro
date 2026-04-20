// common.js

// --- UI: Modals ---
function showModal(modal) {
    if (!modal) return;
    modal.classList.remove('hiding');
    modal.style.display = 'flex';
}

function hideModal(modal) {
    if (!modal) return;
    modal.classList.add('hiding');
    setTimeout(() => {
        modal.style.display = 'none';
        modal.classList.remove('hiding');
    }, 250);
}

// Global Modal Event Listeners (Close on click or backdrop)
document.addEventListener('click', (e) => {
    // Close button
    const closeBtn = e.target.closest('.modal-close');
    if (closeBtn) {
        const targetId = closeBtn.getAttribute('data-target');
        const modal = document.getElementById(targetId);
        hideModal(modal);
        return;
    }
});

// --- UI: Loader ---
function toggleLoader(show) {
    const globalLoader = document.getElementById('globalLoader');
    if (!globalLoader) return;
    if (show) globalLoader.classList.add('visible');
    else globalLoader.classList.remove('visible');
}

// --- Data: Pagination Helper ---
function paginateData(items, page, perPage) {
    const start = (page - 1) * perPage;
    return items.slice(start, start + perPage);
}

function updatePaginationUI(page, totalItems, perPage, elInfo, btnPrev, btnNext) {
    const totalPages = Math.max(1, Math.ceil(totalItems / perPage));

    // Safety check if current page exceeds total
    let safePage = page;
    if (safePage > totalPages) safePage = totalPages;

    if (elInfo) elInfo.textContent = `${safePage} / ${totalPages}`;
    if (btnPrev) btnPrev.disabled = safePage === 1;
    if (btnNext) btnNext.disabled = safePage === totalPages;

    return safePage;
}

// --- Data: Select Helper ---
function fillSelect(select, firstText, values) {
    if (!select) return;
    select.innerHTML = '';
    const first = document.createElement('option');
    first.value = '';
    first.textContent = firstText;
    select.appendChild(first);

    values.forEach(v => {
        const opt = document.createElement('option');
        opt.value = v.id;
        opt.textContent = v.name;
        select.appendChild(opt);
    });
}

// --- Data: Search Mode ---
function getSearchMode() {
    const checked = document.querySelector('input[name="searchMode"]:checked');
    return checked ? checked.value : 'filter';
}

function initSearchModeSwitcher(renderCallback) {
    const radios = document.querySelectorAll('input[name="searchMode"]');
    if (!radios.length) return;
    radios.forEach(radio => {
        radio.addEventListener('change', (e) => {
            if (e.target.checked && typeof renderCallback === 'function') {
                renderCallback();
            }
        });
    });
}

// --- UI: Table Interaction ---
function attachRowSelection(tbodySelector) {
    const tbody = document.querySelector(tbodySelector);
    if (!tbody) return;
    tbody.addEventListener('click', function (e) {
        const row = e.target.closest('tr');
        if (!row) return;
        tbody.querySelectorAll('tr.selected-row').forEach(r => r.classList.remove('selected-row'));
        row.classList.add('selected-row');
    });
}

function updateHeaderSortUI(headers, activeIndex, sortDirection) {
    headers.forEach(h => {
        h.textContent = h.textContent.replace(/[\u25B2\u25BC]\s*$/, '').trim();
    });
    const th = headers[activeIndex];
    const label = th.textContent.replace(/[\u25B2\u25BC]\s*$/, '').trim();
    th.textContent = label + (sortDirection === 'asc' ? ' ▲' : ' ▼');
}

/* =========================
CUSTOM SELECT LOGIC
========================= */

class CustomSelect {
    constructor(originalSelect) {
        this.originalSelect = originalSelect;
        this.wrapper = null;
        this.init();
    }

    init() {
        // Если уже инициализирован, удаляем старую обертку и пересоздаем (для обновления данных)
        if (this.originalSelect.parentNode.classList.contains('custom-select-wrapper')) {
            this.wrapper = this.originalSelect.parentNode;
            // Удаляем всё кроме самого селекта
            const oldTrigger = this.wrapper.querySelector('.custom-select-trigger');
            const oldOptions = this.wrapper.querySelector('.custom-options');
            if (oldTrigger) oldTrigger.remove();
            if (oldOptions) oldOptions.remove();
            this.originalSelect.style.display = 'none'; // Убеждаемся что скрыт
        } else {
            // Создаем обертку
            this.wrapper = document.createElement('div');
            this.wrapper.classList.add('custom-select-wrapper');
            // Переносим классы ширины/отступов, если нужно (опционально)
            this.originalSelect.parentNode.insertBefore(this.wrapper, this.originalSelect);
            this.wrapper.appendChild(this.originalSelect);
            this.originalSelect.style.display = 'none';
        }

        this.createElements();
        this.attachEvents();
    }

    createElements() {
        // 1. Создаем триггер (видимое поле)
        this.trigger = document.createElement('div');
        this.trigger.classList.add('custom-select-trigger');

        // Берем текст выбранной опции или плейсхолдер
        const selectedOption = this.originalSelect.options[this.originalSelect.selectedIndex];
        this.trigger.textContent = selectedOption ? selectedOption.textContent : 'Выберите...';

        this.wrapper.appendChild(this.trigger);

        // 2. Создаем список опций
        this.optionsList = document.createElement('div');
        this.optionsList.classList.add('custom-options');

        Array.from(this.originalSelect.options).forEach(option => {
            const customOption = document.createElement('div');
            customOption.classList.add('custom-option');
            customOption.dataset.value = option.value;
            customOption.textContent = option.textContent;

            if (option.selected) {
                customOption.classList.add('selected');
            }

            // Клик по опции
            customOption.addEventListener('click', (e) => {
                e.stopPropagation();
                this.selectValue(option.value, option.textContent, customOption);
            });

            this.optionsList.appendChild(customOption);
        });

        this.wrapper.appendChild(this.optionsList);
    }

    attachEvents() {
        // Открытие/закрытие по клику
        this.trigger.addEventListener('click', (e) => {
            // Закрываем все остальные перед открытием текущего
            document.querySelectorAll('.custom-select-wrapper.open').forEach(el => {
                if (el !== this.wrapper) el.classList.remove('open');
            });
            this.wrapper.classList.toggle('open');
        });

        // Закрытие при клике вне элемента
        document.addEventListener('click', (e) => {
            if (!this.wrapper.contains(e.target)) {
                this.wrapper.classList.remove('open');
            }
        });
    }

    selectValue(value, text, customOptionElement) {
        // 1. Визуальное обновление
        this.trigger.textContent = text;

        this.wrapper.querySelectorAll('.custom-option').forEach(el => el.classList.remove('selected'));
        customOptionElement.classList.add('selected');

        this.wrapper.classList.remove('open');

        // 2. Обновление оригинального селекта
        this.originalSelect.value = value;

        // 3. ВАЖНО: Триггерим событие change, чтобы сработали ваши фильтры
        this.originalSelect.dispatchEvent(new Event('change'));
    }
}

// Глобальная функция для инициализации всех селектов на странице
function initCustomSelects() {
    document.querySelectorAll('select.form-select, select.parts-filter, select.admin-filter-select').forEach(select => {
        // Пропускаем, если селект скрыт или уже обработан (хотя класс CustomSelect умеет обновлять)
        new CustomSelect(select);
    });
}

document.addEventListener('DOMContentLoaded', function () {
    // 1. Блокировка клавиши "минус" при вводе
    document.body.addEventListener('keydown', function (e) {
        if (e.target.tagName === 'INPUT' && e.target.type === 'number') {
            // Блокируем основной минус и минус на NumPad
            if (e.key === '-' || e.key === 'Subtract') {
                e.preventDefault();
            }
        }
    });

    // 2. Блокировка вставки текста с минусом (Ctrl+V)
    document.body.addEventListener('paste', function (e) {
        if (e.target.tagName === 'INPUT' && e.target.type === 'number') {
            let clipboardData = (e.clipboardData || window.clipboardData).getData('text');
            if (clipboardData.includes('-')) {
                e.preventDefault();
                // Очищаем от минусов и вставляем
                let cleanData = clipboardData.replace(/-/g, '');
                if (cleanData) {
                    document.execCommand('insertText', false, cleanData);
                }
            }
        }
    });
});