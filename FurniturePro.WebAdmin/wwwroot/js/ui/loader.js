export function toggleLoader(show) {
    const globalLoader = document.getElementById('globalLoader');
    if (!globalLoader) return;
    if (show) globalLoader.classList.add('visible');
    else globalLoader.classList.remove('visible');
}