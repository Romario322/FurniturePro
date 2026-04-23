export function fillSelect(select, firstText, values) {
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

export function getSearchMode() {
    const checked = document.querySelector('input[name="searchMode"]:checked');
    return checked ? checked.value : 'filter';
}

export function initSearchModeSwitcher(renderCallback) {
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

export function animateValue(obj, end) {
    if (!obj) return;
    let start = parseInt(obj.textContent.replace(/[^0-9]/g, '')) || 0;
    if (start === end) {
        obj.textContent = end;
        return;
    }

    let range = end - start;
    let current = start;
    let increment = end > start ? 1 : -1;
    let stepTime = Math.abs(Math.floor(1000 / (range || 1)));

    if (Math.abs(range) > 50) { increment = Math.floor(range / 20); stepTime = 50; }
    if (stepTime > 100) stepTime = 100;
    if (stepTime < 20) stepTime = 20;

    const timer = setInterval(function () {
        current += increment;
        if ((increment > 0 && current >= end) || (increment < 0 && current <= end)) {
            current = end;
            clearInterval(timer);
        }
        obj.textContent = current;
    }, stepTime);
}