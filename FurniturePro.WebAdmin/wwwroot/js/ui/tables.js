export function attachRowSelection(tbodySelector) {
    const tbody = document.querySelector(tbodySelector);
    if (!tbody) return;
    tbody.addEventListener('click', function (e) {
        const row = e.target.closest('tr');
        if (!row) return;
        tbody.querySelectorAll('tr.selected-row').forEach(r => r.classList.remove('selected-row'));
        row.classList.add('selected-row');
    });
}

export function updateHeaderSortUI(headers, activeIndex, sortDirection) {
    headers.forEach(h => {
        h.textContent = h.textContent.replace(/[\u25B2\u25BC]\s*$/, '').trim();
    });
    const th = headers[activeIndex];
    const label = th.textContent.replace(/[\u25B2\u25BC]\s*$/, '').trim();
    th.textContent = label + (sortDirection === 'asc' ? ' ▲' : ' ▼');
}