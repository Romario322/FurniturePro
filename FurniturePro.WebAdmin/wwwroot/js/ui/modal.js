export function showModal(modal) {
    if (!modal) return;
    modal.classList.remove('hiding');
    modal.style.display = 'flex';
}

export function hideModal(modal) {
    if (!modal) return;
    modal.classList.add('hiding');
    setTimeout(() => {
        modal.style.display = 'none';
        modal.classList.remove('hiding');
    }, 250);
}

export function initModal() {
    document.addEventListener('click', (e) => {
        const closeBtn = e.target.closest('.modal-close');
        if (closeBtn) {
            const targetId = closeBtn.getAttribute('data-target');
            const modal = document.getElementById(targetId);
            hideModal(modal);
        }
    });
}