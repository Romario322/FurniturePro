import { syncTable } from '../api/syncService.js';
import { getAll } from '../db/repository.js';
import { toggleLoader } from '../ui/loader.js';
import { initHeaderClock } from '../ui/headerClock.js';
import { animateValue } from '../utils/domHelper.js';

class DashboardPage {
    constructor() {
        this.counters = {
            furniture: document.getElementById('countFurniture'),
            parts: document.getElementById('countParts'),
            materials: document.getElementById('countMaterials'),
            colors: document.getElementById('countColors'),
            orders: document.getElementById('countOrders'),
            clients: document.getElementById('countClients')
        };
    }

    async init() {
        try {
            toggleLoader(true);
            initHeaderClock('liveClock', 'liveDate');
            await this.loadData();
        } catch (e) {
            console.error("Ошибка инициализации дашборда:", e);
        } finally {
            toggleLoader(false);
        }
        this.startAutoSync();
    }

    async loadData() {
        // Синхронизация с сервером
        await Promise.allSettled([
            syncTable('Furniture'), syncTable('Parts'),
            syncTable('Materials'), syncTable('Colors'),
            syncTable('Orders'), syncTable('Clients')
        ]);

        // Получение данных из БД
        const [furniture, parts, materials, colors, orders, clients] = await Promise.all([
            getAll('Furniture'), getAll('Parts'),
            getAll('Materials'), getAll('Colors'),
            getAll('Orders'), getAll('Clients')
        ]);

        this.updateCounts({
            furniture: furniture?.length || 0,
            parts: parts?.length || 0,
            materials: materials?.length || 0,
            colors: colors?.length || 0,
            orders: orders?.length || 0,
            clients: clients?.length || 0
        });
    }

    updateCounts(counts) {
        animateValue(this.counters.furniture, counts.furniture);
        animateValue(this.counters.parts, counts.parts);
        animateValue(this.counters.materials, counts.materials);
        animateValue(this.counters.colors, counts.colors);
        animateValue(this.counters.orders, counts.orders);
        animateValue(this.counters.clients, counts.clients);
    }

    startAutoSync() {
        const loop = async () => {
            await new Promise(r => setTimeout(r, 5 * 60 * 1000));
            await this.loadData();
            loop();
        };
        loop();
    }
}

document.addEventListener('DOMContentLoaded', () => {
    const page = new DashboardPage();
    page.init();
});