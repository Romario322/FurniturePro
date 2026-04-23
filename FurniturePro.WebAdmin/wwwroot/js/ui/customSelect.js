export class CustomSelect {
    constructor(originalSelect) {
        this.originalSelect = originalSelect;
        this.wrapper = null;
        this.init();
    }

    init() {
        if (this.originalSelect.parentNode.classList.contains('custom-select-wrapper')) {
            this.wrapper = this.originalSelect.parentNode;
            const oldTrigger = this.wrapper.querySelector('.custom-select-trigger');
            const oldOptions = this.wrapper.querySelector('.custom-options');
            if (oldTrigger) oldTrigger.remove();
            if (oldOptions) oldOptions.remove();
            this.originalSelect.style.display = 'none';
        } else {
            this.wrapper = document.createElement('div');
            this.wrapper.classList.add('custom-select-wrapper');
            this.originalSelect.parentNode.insertBefore(this.wrapper, this.originalSelect);
            this.wrapper.appendChild(this.originalSelect);
            this.originalSelect.style.display = 'none';
        }
        this.createElements();
        this.attachEvents();
    }

    createElements() {
        this.trigger = document.createElement('div');
        this.trigger.classList.add('custom-select-trigger');
        const selectedOption = this.originalSelect.options[this.originalSelect.selectedIndex];
        this.trigger.textContent = selectedOption ? selectedOption.textContent : 'Выберите...';
        this.wrapper.appendChild(this.trigger);

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

            customOption.addEventListener('click', (e) => {
                e.stopPropagation();
                this.selectValue(option.value, option.textContent, customOption);
            });

            this.optionsList.appendChild(customOption);
        });
        this.wrapper.appendChild(this.optionsList);
    }

    attachEvents() {
        this.trigger.addEventListener('click', (e) => {
            document.querySelectorAll('.custom-select-wrapper.open').forEach(el => {
                if (el !== this.wrapper) el.classList.remove('open');
            });
            this.wrapper.classList.toggle('open');
        });

        document.addEventListener('click', (e) => {
            if (!this.wrapper.contains(e.target)) {
                this.wrapper.classList.remove('open');
            }
        });
    }

    selectValue(value, text, customOptionElement) {
        this.trigger.textContent = text;
        this.wrapper.querySelectorAll('.custom-option').forEach(el => el.classList.remove('selected'));
        customOptionElement.classList.add('selected');
        this.wrapper.classList.remove('open');
        this.originalSelect.value = value;
        this.originalSelect.dispatchEvent(new Event('change'));
    }
}

export function initCustomSelects() {
    document.querySelectorAll('select.form-select, select.parts-filter, select.admin-filter-select').forEach(select => {
        new CustomSelect(select);
    });
}