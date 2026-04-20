// wwwroot/js/pages/report.js

// --- UI Elements ---
const dateFromInput = document.getElementById('reportDateFrom');
const dateToInput = document.getElementById('reportDateTo');
const refreshBtn = document.getElementById('refreshReportBtn');
const exportBtn = document.getElementById('exportReportBtn');

const viewRadios = document.querySelectorAll('input[name="viewMode"]');
const sectionHistory = document.getElementById('viewHistory');
const sectionCategories = document.getElementById('viewCategories');
const sectionTable = document.getElementById('viewTable');

const kpiRevenue = document.getElementById('kpiRevenue');
const kpiCost = document.getElementById('kpiCost');
const kpiProfit = document.getElementById('kpiProfit');
const kpiOrders = document.getElementById('kpiOrders');
const chartCatTitle = document.getElementById('chartCatTitle');

// Charts Instances
let catChart = null;
let histChart = null;

// Flatpickr Instances
let fpReportFrom = null;
let fpReportTo = null;

// Data
let rawData = {
    orders: [], orderComps: [], furniture: [], furnComps: [],
    categories: [], prices: [], statusChanges: []
};
let lastCalculatedData = null;

// Utils
const fmtMoney = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB', maximumFractionDigits: 0 });
const getCssVar = (name) => getComputedStyle(document.documentElement).getPropertyValue(name).trim();

document.addEventListener('DOMContentLoaded', async () => {
    // 1. Инициализация календарей (пустых)
    initReportPickers();

    // 2. Логика переключения вкладок
    viewRadios.forEach(radio => {
        radio.addEventListener('change', (e) => {
            const mode = e.target.value;

            // Скрываем все
            sectionHistory.classList.add('d-none');
            sectionCategories.classList.add('d-none');
            sectionTable.classList.add('d-none');

            // Показываем нужное + обновляем графики
            switch (mode) {
                case 'history':
                    sectionHistory.classList.remove('d-none');
                    if (histChart) {
                        setTimeout(() => histChart.updateOptions({ chart: { width: '100%' } }), 50);
                    }
                    break;

                case 'categories':
                    sectionCategories.classList.remove('d-none');
                    if (catChart) {
                        setTimeout(() => catChart.updateOptions({ chart: { width: '100%' } }), 50);
                    }
                    break;

                case 'table':
                    sectionTable.classList.remove('d-none');
                    break;
            }
        });
    });

    // 3. Загрузка данных
    await syncAndLoadData();

    // 4. Листенеры событий кнопок
    refreshBtn.addEventListener('click', async () => await syncAndLoadData());

    if (exportBtn) {
        exportBtn.addEventListener('click', exportToPdf);
    }
});

// === ИНИЦИАЛИЗАЦИЯ FLATPICKR ===
function initReportPickers() {
    const commonConfig = {
        locale: "ru",
        dateFormat: "Y-m-d",
        altInput: true,
        altFormat: "d.m.Y",
        disableMobile: "true", // Важно для стилизации
        maxDate: "today",       // В будущее нельзя
        allowInput: true
    };

    fpReportFrom = flatpickr(dateFromInput, {
        ...commonConfig,
        onChange: function (selectedDates, dateStr, instance) {
            // При смене "С", обновляем minDate у "По"
            if (fpReportTo) fpReportTo.set("minDate", dateStr);
            calculateReport();
        }
    });

    fpReportTo = flatpickr(dateToInput, {
        ...commonConfig,
        onChange: function (selectedDates, dateStr, instance) {
            // При смене "По", обновляем maxDate у "С" (но не больше сегодня)
            if (fpReportFrom) {
                // Ограничиваем "С" текущей выбранной датой "По"
                fpReportFrom.set("maxDate", dateStr);
            }
            calculateReport();
        }
    });
}

// === ОБНОВЛЕНИЕ ГРАНИЦ ДАТ ===
function updatePickersWithData() {
    if (!rawData.statusChanges || rawData.statusChanges.length === 0) return;

    // Находим самую раннюю дату в системе
    const dates = rawData.statusChanges.map(sc => new Date(sc.date).getTime());
    if (dates.length === 0) return;

    const minDateTs = Math.min(...dates);
    const minISO = new Date(minDateTs).toISOString().slice(0, 10);
    const todayISO = new Date().toISOString().slice(0, 10);

    // Устанавливаем глобальные границы
    if (fpReportFrom) {
        fpReportFrom.set("minDate", minISO);
        // Если дата еще не выбрана, ставим minISO
        if (!fpReportFrom.selectedDates.length) fpReportFrom.setDate(minISO, false);
    }

    if (fpReportTo) {
        fpReportTo.set("minDate", minISO);
        // Если дата еще не выбрана, ставим сегодня
        if (!fpReportTo.selectedDates.length) fpReportTo.setDate(todayISO, false);
    }

    // Синхронизируем перекрестные ограничения
    if (fpReportFrom && fpReportTo) {
        fpReportTo.set("minDate", fpReportFrom.input.value);
        fpReportFrom.set("maxDate", fpReportTo.input.value);
    }
}

async function syncAndLoadData() {
    try {
        toggleLoader(true);
        const tablesToSync = ['Orders', 'OrderCompositions', 'Furniture', 'FurnitureCompositions', 'Categories', 'Prices', 'StatusChanges'];

        try {
            await Promise.all(tablesToSync.map(t => syncTable(t)));
        } catch (e) {
            console.warn("Offline mode:", e);
        }

        const [orders, orderComps, furniture, furnComps, categories, prices, statusChanges] = await Promise.all([
            getAll('Orders'), getAll('OrderCompositions'), getAll('Furniture'), getAll('FurnitureCompositions'),
            getAll('Categories'), getAll('Prices'), getAll('StatusChanges')
        ]);

        rawData = { orders, orderComps, furniture, furnComps, categories, prices, statusChanges };

        // Обновляем календари с учетом полученных данных
        updatePickersWithData();

        calculateReport();

    } catch (e) {
        console.warn("Error:", e);
    } finally {
        toggleLoader(false);
    }
}

function calculateReport() {
    // 1. Подготовка справочников
    const furnMap = new Map(rawData.furniture.map(f => [f.id, f]));
    const catMap = new Map(rawData.categories.map(c => [c.id, c]));

    const furnCompLookup = new Map();
    rawData.furnComps.forEach(fc => {
        const fId = fc.idFurniture ?? fc.entity1Id;
        const pId = fc.idPart ?? fc.entity2Id;
        if (!furnCompLookup.has(fId)) furnCompLookup.set(fId, []);
        furnCompLookup.get(fId).push({ partId: pId, count: fc.count });
    });

    const priceLookup = new Map();
    const sortedPrices = [...rawData.prices].sort((a, b) => new Date(b.date) - new Date(a.date));
    sortedPrices.forEach(p => {
        if (!priceLookup.has(p.partId)) priceLookup.set(p.partId, []);
        priceLookup.get(p.partId).push(p);
    });

    const orderDateLookup = new Map();
    rawData.statusChanges.forEach(sc => {
        const d = new Date(sc.date).getTime();
        if (!orderDateLookup.has(sc.idOrder) || d < orderDateLookup.get(sc.idOrder)) orderDateLookup.set(sc.idOrder, d);
    });

    // 2. Получение дат из Flatpickr
    const dFrom = fpReportFrom && fpReportFrom.selectedDates[0] ? fpReportFrom.selectedDates[0] : null;
    const dTo = fpReportTo && fpReportTo.selectedDates[0] ? fpReportTo.selectedDates[0] : null;
    if (dTo) dTo.setHours(23, 59, 59, 999);

    const reportLines = [];
    rawData.orders.forEach(order => {
        let orderDateTs = orderDateLookup.get(order.id);
        if (!orderDateTs) orderDateTs = order.updateDate ? new Date(order.updateDate).getTime() : new Date().getTime();
        const orderDate = new Date(orderDateTs);

        if (dFrom && orderDate < dFrom) return;
        if (dTo && orderDate > dTo) return;

        rawData.orderComps.filter(oc => oc.idOrder === order.id).forEach(item => {
            const furnObj = furnMap.get(item.idFurniture);
            if (!furnObj) return;

            let unitCost = 0;
            const partsList = furnCompLookup.get(furnObj.id) || [];
            partsList.forEach(partLink => {
                let partPrice = 0;
                const history = priceLookup.get(partLink.partId);
                if (history && history.length > 0) {
                    const match = history.find(p => new Date(p.date) <= orderDate);
                    partPrice = match ? match.value : history[history.length - 1].value;
                }
                unitCost += partPrice * partLink.count;
            });

            const markup = furnObj.markup || 0;
            const unitBasePrice = unitCost + (unitCost * markup / 100);
            const discount = order.discount || 0;
            const unitSoldPrice = unitBasePrice * (1 - (discount / 100));

            reportLines.push({
                categoryName: catMap.get(furnObj.categoryId)?.name || "Без категории",
                furnitureName: furnObj.name,
                orderDate: orderDate,
                orderId: order.id,
                count: item.count,
                totalCost: unitCost * item.count,
                totalRevenue: unitSoldPrice * item.count
            });
        });
    });

    // 3. Агрегация KPI и группировка по месяцам
    const kpi = { revenue: 0, cost: 0, profit: 0, ordersCount: new Set() };
    const byCategoryMap = new Map();
    const byMonthMap = new Map();
    const monthlyFurnitureMap = new Map(); // Карта для хранения мебели внутри месяцев

    reportLines.forEach(line => {
        const profit = line.totalRevenue - line.totalCost;
        kpi.revenue += line.totalRevenue;
        kpi.cost += line.totalCost;
        kpi.profit += profit;
        kpi.ordersCount.add(line.orderId);

        if (!byCategoryMap.has(line.categoryName)) byCategoryMap.set(line.categoryName, { name: line.categoryName, value: 0 });
        byCategoryMap.get(line.categoryName).value += line.totalRevenue;

        const mKey = `${line.orderDate.getFullYear()}-${String(line.orderDate.getMonth() + 1).padStart(2, '0')}`;

        // Статистика для графика динамики
        if (!byMonthMap.has(mKey)) {
            byMonthMap.set(mKey, { date: new Date(line.orderDate.getFullYear(), line.orderDate.getMonth(), 1), revenue: 0, cost: 0, profit: 0 });
        }
        const mData = byMonthMap.get(mKey);
        mData.revenue += line.totalRevenue; mData.cost += line.totalCost; mData.profit += profit;

        // Группировка мебели для ТОПа внутри месяца
        if (!monthlyFurnitureMap.has(mKey)) monthlyFurnitureMap.set(mKey, new Map());
        const monthGroup = monthlyFurnitureMap.get(mKey);
        if (!monthGroup.has(line.furnitureName)) monthGroup.set(line.furnitureName, { name: line.furnitureName, count: 0, revenue: 0 });
        const fData = monthGroup.get(line.furnitureName);
        fData.count += line.count; fData.revenue += line.totalRevenue;
    });

    const byCategory = Array.from(byCategoryMap.values()).sort((a, b) => b.value - a.value);
    const byMonth = Array.from(byMonthMap.values()).sort((a, b) => a.date - b.date);

    // Формируем структуру ТОП-10 по каждому месяцу
    const topFurnitureGrouped = byMonth.map(monthData => {
        const mKey = `${monthData.date.getFullYear()}-${String(monthData.date.getMonth() + 1).padStart(2, '0')}`;
        const items = Array.from(monthlyFurnitureMap.get(mKey).values())
            .sort((a, b) => b.revenue - a.revenue)
            .slice(0, 10); // Берем ТОП-10
        return {
            monthName: monthData.date.toLocaleString('ru-RU', { month: 'long', year: 'numeric' }),
            items: items
        };
    }).reverse(); // Показываем последние месяцы сверху

    const fromStr = dFrom ? dFrom.toLocaleDateString() : 'Начало';
    const toStr = dTo ? dTo.toLocaleDateString() : 'Сейчас';

    lastCalculatedData = {
        kpi: { ...kpi, ordersCount: kpi.ordersCount.size },
        byCategory, byMonth,
        topFurniture: topFurnitureGrouped, // Сохраняем сгруппированные данные
        dateRange: { from: fromStr, to: toStr }
    };

    renderDashboard(lastCalculatedData);
}

function renderDashboard(data) {
    kpiRevenue.textContent = fmtMoney.format(data.kpi.revenue);
    kpiCost.textContent = fmtMoney.format(data.kpi.cost);
    kpiProfit.textContent = fmtMoney.format(data.kpi.profit);
    kpiOrders.textContent = data.kpi.ordersCount;
    kpiProfit.style.color = data.kpi.profit >= 0 ? 'var(--action-create)' : 'var(--danger-main)';

    renderCategoryChart(data.byCategory);
    renderHistoryChart(data.byMonth);
    renderTopTable(data.topFurniture);
}

// === Chart Renderers (UI) ===
function generateThemePalette(count) {
    const baseColors = ['#e27b32', '#1f5a35', '#2c5d63', '#500805', '#dba356', '#8a6540'];
    let palette = [];
    if (count <= baseColors.length) return baseColors.slice(0, count);
    const adjustColor = (color, amount) => '#' + color.replace(/^#/, '').replace(/../g, color => ('0' + Math.min(255, Math.max(0, parseInt(color, 16) + amount)).toString(16)).substr(-2));
    let i = 0;
    while (palette.length < count) {
        let base = baseColors[i % baseColors.length];
        let round = Math.floor(i / baseColors.length);
        let adjustment = (round === 1) ? 40 : (round === 2 ? -40 : 0);
        palette.push(adjustment !== 0 ? adjustColor(base, adjustment) : base);
        i++;
    }
    return palette;
}

function renderCategoryChart(items) {
    if (catChart) catChart.destroy();
    const labels = items.map(x => x.name);
    const series = items.map(x => x.value);
    if (chartCatTitle) chartCatTitle.innerHTML = 'Доля выручки по категориям';

    const isEmpty = series.length === 0 || series.every(x => x === 0);
    const emptyMsg = document.getElementById('chartCatEmpty');
    if (emptyMsg) emptyMsg.style.display = isEmpty ? 'block' : 'none';
    if (isEmpty) return;

    const options = {
        series: series,
        labels: labels,
        chart: {
            type: 'donut',
            height: '100%',
            fontFamily: 'inherit',
            selection: { enabled: false },
            // Отключаем клики
            events: {
                dataPointSelection: () => { },
                markerClick: () => { }
            }
        },
        colors: generateThemePalette(series.length),
        stroke: { show: false, width: 0 },
        dataLabels: { enabled: false },

        // ОТКЛЮЧЕНИЕ ЭФФЕКТОВ
        states: {
            normal: { filter: { type: 'none', value: 0 } },
            hover: { filter: { type: 'none', value: 0 } },
            active: { filter: { type: 'none', value: 0 } },
        },
        tooltip: { enabled: false },

        legend: {
            show: true, position: 'bottom', fontSize: '14px', fontFamily: 'inherit', fontWeight: 500,
            labels: { colors: '#2b1a10', useSeriesColors: false },
            itemMargin: { horizontal: 15, vertical: 8 },
            markers: { strokeWidth: 0, radius: 12 },
            // Отключаем кликабельность легенды
            onItemClick: { toggleDataSeries: false },
            onItemHover: { highlightDataSeries: false },
            formatter: function (seriesName, opts) {
                const val = opts.w.globals.series[opts.seriesIndex];
                const total = opts.w.globals.series.reduce((a, b) => a + b, 0);
                const percent = total > 0 ? ((val / total) * 100).toFixed(1) : 0;
                return `${seriesName}: ${percent}% (${fmtMoney.format(val)})`;
            }
        },
        plotOptions: { pie: { expandOnClick: false, donut: { size: '65%', labels: { show: false } } } }
    };
    catChart = new ApexCharts(document.querySelector("#chartCategories"), options);
    catChart.render();
}

function renderHistoryChart(items) {
    if (histChart) histChart.destroy();
    const categories = items.map(x => {
        let m = x.date.toLocaleString('ru-RU', { month: 'short' }).replace('.', '');
        if (m.length > 0) m = m[0].toUpperCase() + m.slice(1);
        return `${m} ${x.date.getFullYear()}`;
    });
    const options = {
        series: [
            { name: 'Выручка', type: 'column', data: items.map(x => x.revenue) },
            { name: 'Себестоимость', type: 'column', data: items.map(x => x.cost) },
            { name: 'Прибыль', type: 'line', data: items.map(x => x.profit) }
        ],
        chart: {
            type: 'line',
            height: '100%',
            toolbar: { show: false },
            fontFamily: 'inherit',
            zoom: { enabled: false },
            // Отключаем клики
            events: {
                click: () => { },
                markerClick: () => { }
            }
        },
        colors: ['#2c5d63', '#8c3b38', '#e27b32'],

        // ОТКЛЮЧЕНИЕ ЭФФЕКТОВ
        states: {
            normal: { filter: { type: 'none', value: 0 } },
            hover: { filter: { type: 'none', value: 0 } },
            active: { filter: { type: 'none', value: 0 } },
        },
        tooltip: { enabled: false },

        plotOptions: { bar: { horizontal: false, columnWidth: '65%', borderRadius: 2 } },
        stroke: { width: [0, 0, 4], curve: 'smooth' },
        dataLabels: { enabled: false },
        xaxis: {
            categories: categories,
            axisBorder: { show: true, color: 'rgba(125, 94, 60, 0.4)' },
            axisTicks: { show: true, color: 'rgba(125, 94, 60, 0.4)' },
            tooltip: { enabled: false },
            labels: { rotate: -45, style: { colors: '#8a6540', fontSize: '12px', fontFamily: 'inherit', fontWeight: 500 } }
        },
        yaxis: [{
            labels: {
                formatter: (val) => {
                    if (val >= 1000000) return (val / 1000000).toFixed(1).replace(/\.0$/, '') + ' млн ₽';
                    if (val >= 1000) return (val / 1000).toFixed(0) + ' тыс ₽';
                    return val + ' ₽';
                },
                style: { colors: '#8a6540', fontSize: '11px', fontFamily: 'inherit' }
            }
        }],
        grid: { borderColor: 'rgba(125, 94, 60, 0.15)', strokeDashArray: 4 },
        markers: {
            size: 5,
            colors: ['#e27b32'],
            strokeColors: '#fff',
            strokeWidth: 2,
            hover: { size: 5 } // Запрет увеличения
        },
        legend: {
            show: true,
            position: 'top',
            horizontalAlign: 'right',
            floating: true,
            offsetY: -20,
            markers: { radius: 12, strokeWidth: 0 },
            itemMargin: { horizontal: 10 },
            onItemClick: { toggleDataSeries: false },
            onItemHover: { highlightDataSeries: false }
        }
    };
    histChart = new ApexCharts(document.querySelector("#chartHistory"), options);
    histChart.render();
}

function renderTopTable(groups) {
    const tbody = document.querySelector('#topTable tbody');
    if (!tbody) return;
    tbody.innerHTML = '';

    if (!groups || groups.length === 0) {
        tbody.innerHTML = `<tr><td colspan="3" class="text-center p-3" style="opacity:0.6;">Нет продаж</td></tr>`;
        return;
    }

    groups.forEach(group => {
        // 1. Создаем строку-заголовок месяца
        const headerTr = document.createElement('tr');
        headerTr.className = 'month-group-header'; // Применяем наш новый CSS класс
        headerTr.innerHTML = `
            <td colspan="3">
                <div class="month-group-title">
                    <span>📅</span> 
                    <span>${group.monthName}</span>
                </div>
            </td>
        `;
        tbody.appendChild(headerTr);

        // 2. Добавляем ТОП-10 товаров этого месяца
        group.items.forEach(item => {
            const tr = document.createElement('tr');
            tr.className = 'month-item-row'; // Класс для отступа первой ячейки
            tr.innerHTML = `
                <td style="font-weight: 500;">${item.name}</td>
                <td class="text-center">
                    <span class="badge badge-theme" style="background-color: var(--btn-comp-bg); min-width: 30px;">
                        ${item.count}
                    </span>
                </td>
                <td class="text-end fw-bold" style="padding-right: 1.5rem;">
                    ${fmtMoney.format(item.revenue)}
                </td>
            `;
            tbody.appendChild(tr);
        });
    });
}

// ==========================================
// ЭКСПОРТ В PDF
// ==========================================
async function exportToPdf() {
    if (!lastCalculatedData) return;

    try {
        toggleLoader(true);

        if (!lastCalculatedData.kpi || lastCalculatedData.kpi.cost === undefined) {
            throw new Error("Данные отсутствуют!");
        }

        // 1. Подготовка изображений для графиков
        const histCats = lastCalculatedData.byMonth.map(x => {
            let m = x.date.toLocaleString('ru-RU', { month: 'short' }).replace('.', '');
            return (m[0].toUpperCase() + m.slice(1)) + ' ' + x.date.getFullYear();
        });
        const histSeries = [
            { name: 'Выручка', type: 'column', data: lastCalculatedData.byMonth.map(x => x.revenue) },
            { name: 'Себестоимость', type: 'column', data: lastCalculatedData.byMonth.map(x => x.cost) },
            { name: 'Прибыль', type: 'line', data: lastCalculatedData.byMonth.map(x => x.profit) }
        ];

        const catLabels = lastCalculatedData.byCategory.map(x => x.name);
        const catSeries = lastCalculatedData.byCategory.map(x => x.value);
        const catColors = generateThemePalette(catSeries.length);

        const imgHistory = await generateGhostImage('line', histSeries, histCats, [], 1000, 500); // [cite: 21, 22]
        const imgCategory = await generateGhostImage('donut', catSeries, catLabels, catColors, 1000, 800); // [cite: 24, 25]

        const rangeText = `${lastCalculatedData.dateRange.from} — ${lastCalculatedData.dateRange.to}`;
        const totalCatRev = lastCalculatedData.byCategory.reduce((sum, c) => sum + c.value, 0);

        // Шаблон блока KPI (используется в каждом разделе) 
        const createKpiBlock = () => ({
            style: 'kpiTable',
            table: {
                widths: ['*', '*', '*', '*'],
                body: [
                    [
                        { text: 'Выручка', style: 'kpiLabel', fillColor: '#2c5d63' },
                        { text: 'Себестоимость', style: 'kpiLabel', fillColor: '#8c3b38' },
                        { text: 'Прибыль', style: 'kpiLabel', fillColor: '#e27b32' },
                        { text: 'Заказов', style: 'kpiLabel', fillColor: '#8a6540' }
                    ],
                    [
                        { text: fmtMoney.format(lastCalculatedData.kpi.revenue), style: 'kpiValue' },
                        { text: fmtMoney.format(lastCalculatedData.kpi.cost), style: 'kpiValue' },
                        { text: fmtMoney.format(lastCalculatedData.kpi.profit), style: 'kpiValue' },
                        { text: lastCalculatedData.kpi.ordersCount, style: 'kpiValue' }
                    ]
                ]
            },
            layout: { hLineWidth: (i) => (i === 0 || i === 2) ? 1 : 0, vLineWidth: () => 0, hLineColor: '#ccc' }
        });

        const docDefinition = {
            info: { title: `Отчет FurniturePro`, author: 'System' },
            pageSize: 'A4',
            pageMargins: [30, 30, 30, 30],

            content: [
                // === РАЗДЕЛ 1: ДИНАМИКА ПРОДАЖ ===
                { text: 'РАЗДЕЛ 1: АНАЛИЗ ДИНАМИКИ ФИНАНСОВ', style: 'header' },
                { text: `Период: ${rangeText}`, style: 'subheader' },
                createKpiBlock(),
                { text: 'График динамики (Выручка, Себестоимость, Прибыль)', style: 'sectionTitle' },
                { image: imgHistory, width: 535, alignment: 'center', margin: [0, 0, 0, 15] },
                { text: 'Таблица детализации по месяцам', style: 'sectionTitle' },
                {
                    style: 'dataTable',
                    table: {
                        headerRows: 1, widths: ['*', 'auto', 'auto', 'auto'],
                        body: [
                            [{ text: 'Месяц', style: 'th' }, { text: 'Выручка', style: 'th', alignment: 'right' }, { text: 'Себестоимость', style: 'th', alignment: 'right' }, { text: 'Прибыль', style: 'th', alignment: 'right' }],
                            ...lastCalculatedData.byMonth.map(m => [
                                m.date.toLocaleDateString('ru-RU', { month: 'long', year: 'numeric' }),
                                { text: fmtMoney.format(m.revenue), alignment: 'right' },
                                { text: fmtMoney.format(m.cost), alignment: 'right' },
                                { text: fmtMoney.format(m.profit), alignment: 'right' }
                            ])
                        ]
                    },
                    layout: 'lightHorizontalLines'
                },

                // === РАЗДЕЛ 2: ДОЛИ КАТЕГОРИЙ ===
                { text: '', pageBreak: 'before' },
                { text: 'РАЗДЕЛ 2: АНАЛИЗ КАТЕГОРИЙ ПРОДАЖ', style: 'header' },
                { text: `Период: ${rangeText}`, style: 'subheader' },
                createKpiBlock(),
                { text: 'Распределение выручки по категориям', style: 'sectionTitle' },
                { image: imgCategory, width: 450, alignment: 'center', margin: [0, 0, 0, 15] },
                { text: 'Таблица долей категорий', style: 'sectionTitle' },
                {
                    style: 'dataTable',
                    table: {
                        headerRows: 1, widths: ['*', 'auto', 'auto'],
                        body: [
                            [{ text: 'Категория', style: 'th' }, { text: 'Выручка', style: 'th', alignment: 'right' }, { text: 'Доля (%)', style: 'th', alignment: 'right' }],
                            ...lastCalculatedData.byCategory.map(c => {
                                const p = totalCatRev > 0 ? ((c.value / totalCatRev) * 100).toFixed(1) : 0;
                                return [c.name, { text: fmtMoney.format(c.value), alignment: 'right' }, { text: p + '%', alignment: 'right' }];
                            })
                        ]
                    },
                    layout: 'lightHorizontalLines'
                },

                // === РАЗДЕЛ 3: ТОП ПРОДАЖ ===
                { text: '', pageBreak: 'before' },
                { text: 'РАЗДЕЛ 3: ТОП ПРОДАЖ ПО МЕСЯЦАМ', style: 'header' },
                { text: `Период: ${rangeText}`, style: 'subheader' },
                createKpiBlock(),
                { text: 'Детализация ТОП-10 позиций мебели по месяцам', style: 'sectionTitle' },
                {
                    style: 'dataTable',
                    table: {
                        headerRows: 1, widths: ['*', 'auto', 'auto'],
                        body: [
                            [{ text: 'Название / Месяц', style: 'th' }, { text: 'Кол-во (шт)', style: 'th', alignment: 'center' }, { text: 'Выручка', style: 'th', alignment: 'right' }],
                            ...lastCalculatedData.topFurniture.flatMap(group => [
                                [{ text: group.monthName.toUpperCase(), fillColor: '#f0f0f0', bold: true, colSpan: 3, margin: [0, 2, 0, 2] }, {}, {}],
                                ...group.items.map(f => [
                                    { text: f.name, margin: [10, 0, 0, 0] },
                                    { text: f.count, alignment: 'center' },
                                    { text: fmtMoney.format(f.revenue), alignment: 'right' }
                                ])
                            ])
                        ]
                    },
                    layout: 'lightHorizontalLines'
                }
            ],
            styles: {
                header: { fontSize: 16, bold: true, alignment: 'center', color: '#2b1a10', margin: [0, 0, 0, 5] },
                subheader: { fontSize: 10, italics: true, alignment: 'center', color: '#8a6540', margin: [0, 0, 0, 15] },
                kpiTable: { margin: [0, 0, 0, 15] },
                kpiLabel: { color: 'white', bold: true, fontSize: 9, alignment: 'center', margin: [0, 4, 0, 4] },
                kpiValue: { fontSize: 11, bold: true, alignment: 'center', margin: [0, 4, 0, 4], color: '#2b1a10' },
                sectionTitle: { fontSize: 12, bold: true, color: '#2c5d63', margin: [0, 5, 0, 5] },
                th: { bold: true, fontSize: 9, color: 'black', fillColor: '#f0f0f0', margin: [0, 3, 0, 3] },
                dataTable: { fontSize: 9, margin: [0, 0, 0, 10] }
            }
        };

        pdfMake.createPdf(docDefinition).download(`Детальный Отчет ${new Date().toISOString().slice(0, 10)}.pdf`);

    } catch (e) {
        console.error("PDF Error:", e);
        alert("Ошибка PDF: " + e.message);
    } finally {
        toggleLoader(false);
    }
}

async function generateGhostImage(type, series, labels, colors, width, height) {
    const div = document.createElement('div');
    div.style.position = 'fixed';
    div.style.left = '-10000px';
    div.style.top = '-10000px';
    div.style.width = width + 'px';
    div.style.height = height + 'px';
    div.style.background = '#fff';
    document.body.appendChild(div);

    const options = {
        series: series,
        chart: { type: type, width: width, height: height, animations: { enabled: false }, toolbar: { show: false }, zoom: { enabled: false } },
        dataLabels: { enabled: false },
        stroke: { width: type === 'line' ? [0, 0, 4] : 0, curve: 'smooth' },
        colors: colors.length > 0 ? colors : ['#2c5d63', '#8c3b38', '#e27b32'],
        legend: {
            show: true, position: 'bottom', fontSize: '16px', fontWeight: 600,
            itemMargin: { horizontal: 10, vertical: 8 }
        }
    };

    if (type === 'donut') {
        options.labels = labels;
        options.plotOptions = { pie: { donut: { size: '65%', labels: { show: false } } } };
        options.legend.formatter = function (seriesName, opts) {
            const val = opts.w.globals.series[opts.seriesIndex];
            const total = opts.w.globals.series.reduce((a, b) => a + b, 0);
            const percent = total > 0 ? ((val / total) * 100).toFixed(1) : 0;
            return `${seriesName}: ${percent}%`;
        };
    } else {
        options.xaxis = { categories: labels, labels: { style: { fontSize: '14px', fontWeight: 600 } } };
        options.plotOptions = { bar: { horizontal: false, columnWidth: '65%', borderRadius: 0 } };
        options.yaxis = {
            labels: {
                style: { fontSize: '14px' },
                formatter: (val) => {
                    if (val >= 1000000000) return (val / 1000000000).toFixed(1) + ' млрд ₽'
                    if (val >= 1000000) return (val / 1000000).toFixed(1) + ' млн ₽';
                    if (val >= 1000) return (val / 1000).toFixed(0) + ' к ₽';
                    return val + ' ₽';
                }
            }
        };
    }

    const chart = new ApexCharts(div, options);
    await chart.render();
    await new Promise(r => setTimeout(r, 400));
    const uri = await chart.dataURI({ scale: 1 });
    chart.destroy();
    document.body.removeChild(div);
    return uri.imgURI;
}