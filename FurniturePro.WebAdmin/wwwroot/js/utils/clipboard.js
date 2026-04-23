export function copyToClipboard(text, btnElement) {
    if (!text) return;
    navigator.clipboard.writeText(text).then(() => {
        if (btnElement) {
            const originalHTML = btnElement.innerHTML;
            btnElement.innerHTML = '✓';
            btnElement.style.color = 'var(--action-create)';
            setTimeout(() => {
                btnElement.innerHTML = originalHTML;
                btnElement.style.color = '';
            }, 1000);
        }
    }).catch(err => console.error('Copy failed', err));
}

export function createCopyableContent(text) {
    const wrapper = document.createElement('div');
    wrapper.className = 'd-flex align-items-center justify-content-between gap-2';

    const span = document.createElement('span');
    span.textContent = text || '';
    wrapper.appendChild(span);

    if (text) {
        const btn = document.createElement('button');
        btn.className = 'btn-copy-action btn-square';
        btn.innerHTML = '📋';
        btn.title = 'Скопировать';
        btn.onclick = (e) => {
            e.stopPropagation();
            copyToClipboard(text, btn);
        };
        wrapper.appendChild(btn);
    }
    return wrapper;
}