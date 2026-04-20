// wwwroot/js/pages/index.js

// Локальный кэш
let cached_furniture = [];
let cached_parts = [];
let cached_materials = [];
let cached_colors = [];
let cached_orders = [];
let cached_clients = [];

// Элементы счетчиков
const counters = {
    furniture: document.getElementById('countFurniture'),
    parts: document.getElementById('countParts'),
    materials: document.getElementById('countMaterials'),
    colors: document.getElementById('countColors'),
    orders: document.getElementById('countOrders'),
    clients: document.getElementById('countClients')
};

document.addEventListener('DOMContentLoaded', async () => {
    try {
        toggleLoader(true);
        startClock(); // Запуск часов в шапке

        // Первая инициализация
        await initDashboard();

    } catch (e) {
        console.error("Ошибка инициализации:", e);
    } finally {
        toggleLoader(false);
    }

    // Фоновая синхронизация
    startAutoSync();
});

async function initDashboard() {
    // 1. Синхронизируем таблицы (запрашиваем изменения с сервера)
    await Promise.allSettled([
        syncTable('Furniture'),
        syncTable('Parts'),
        syncTable('Materials'),
        syncTable('Colors'),
        syncTable('Orders'),
        syncTable('Clients')
    ]);

    // 2. Получаем полные данные из кэша
    const [furniture, parts, materials, colors, orders, clients] = await Promise.all([
        getAll('Furniture'),
        getAll('Parts'),
        getAll('Materials'),
        getAll('Colors'),
        getAll('Orders'),
        getAll('Clients')
    ]);

    // 3. Сохраняем в переменные
    cached_furniture = furniture || [];
    cached_parts = parts || [];
    cached_materials = materials || [];
    cached_colors = colors || [];
    cached_orders = orders || [];
    cached_clients = clients || [];

    // 4. Обновляем UI (счетчики)
    updateCounts();
}

function updateCounts() {
    animateValue(counters.furniture, cached_furniture.length);
    animateValue(counters.parts, cached_parts.length);
    animateValue(counters.materials, cached_materials.length);
    animateValue(counters.colors, cached_colors.length);
    animateValue(counters.orders, cached_orders.length);
    animateValue(counters.clients, cached_clients.length);
}

// Анимация чисел
function animateValue(obj, end) {
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

    // Оптимизация скорости анимации для больших чисел
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

// Логика часов в шапке
function startClock() {
    const elClock = document.getElementById('liveClock');
    const elDate = document.getElementById('liveDate');

    function tick() {
        const now = new Date();
        if (elClock) elClock.textContent = now.toLocaleTimeString('ru-RU');
        if (elDate) {
            const dateOpts = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
            let dateStr = now.toLocaleDateString('ru-RU', dateOpts);
            dateStr = dateStr.charAt(0).toUpperCase() + dateStr.slice(1);
            elDate.textContent = dateStr;
        }
    }

    tick();
    setInterval(tick, 1000);
}

// Автообновление каждые 5 минут
function startAutoSync() {
    async function loop() {
        await new Promise(r => setTimeout(r, 5 * 60 * 1000));
        await initDashboard();
        loop();
    }
    loop();
}