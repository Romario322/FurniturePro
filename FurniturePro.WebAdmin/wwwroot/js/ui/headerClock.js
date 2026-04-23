export function initHeaderClock(clockId = 'liveClock', dateId = 'liveDate') {
    const elClock = document.getElementById(clockId);
    const elDate = document.getElementById(dateId);
    if (!elClock && !elDate) return;

    function tick() {
        const now = new Date();
        if (elClock) elClock.textContent = now.toLocaleTimeString('ru-RU');
        if (elDate) {
            const dateOpts = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
            let dateStr = now.toLocaleDateString('ru-RU', dateOpts);
            elDate.textContent = dateStr.charAt(0).toUpperCase() + dateStr.slice(1);
        }
    }
    tick();
    setInterval(tick, 1000);
}