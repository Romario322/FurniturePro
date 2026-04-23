export function paginateData(items, page, perPage) {
    const start = (page - 1) * perPage;
    return items.slice(start, start + perPage);
}

export function updatePaginationUI(page, totalItems, perPage, elInfo, btnPrev, btnNext) {
    const totalPages = Math.max(1, Math.ceil(totalItems / perPage));

    let safePage = page;
    if (safePage > totalPages) safePage = totalPages;

    if (elInfo) elInfo.textContent = `${safePage} / ${totalPages}`;
    if (btnPrev) btnPrev.disabled = safePage === 1;
    if (btnNext) btnNext.disabled = safePage === totalPages;

    return safePage;
}