import { syncTable } from '../api/syncService.js';
import { getAll } from '../db/repository.js';
import { toggleLoader } from '../ui/loader.js';

class ReportPage {
    constructor() {
        this.fmtMoney = new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB', maximumFractionDigits: 0 });

        // State
        this.rawData = { orders: [], orderComps: [], furniture: [], furnComps: [], categories: [], prices: [], statusChanges: [] };
        this.lastCalculatedData = null;
        this.catChart = null;
        this.histChart = null;
        this.fpReportFrom = null;
        this.fpReportTo = null;

        this.initDOM();
        this.attachEvents();
    }

    initDOM() {
        this.dateFromInput = document.getElementById('reportDateFrom');
        this.dateToInput = document.getElementById('reportDateTo');
        this.refreshBtn = document.getElementById('refreshReportBtn');
        this.exportBtn = document.getElementById('exportReportBtn');

        this.viewRadios = document.querySelectorAll('input[name="viewMode"]');
        this.sectionHistory = document.getElementById('viewHistory');
        this.sectionCategories = document.getElementById('viewCategories');
        this.sectionTable = document.getElementById('viewTable');

        this.kpiRevenue = document.getElementById('kpiRevenue');
        this.kpiCost = document.getElementById('kpiCost');
        this.kpiProfit = document.getElementById('kpiProfit');
        this.kpiOrders = document.getElementById('kpiOrders');
        this.chartCatTitle = document.getElementById('chartCatTitle');
    }

    async init() {
        this.initPickers();
        await this.syncAndLoadData();
    }

    attachEvents() {
        this.viewRadios.forEach(radio => {
            radio.addEventListener('change', (e) => this.handleViewChange(e.target.value));
        });

        if (this.refreshBtn) this.refreshBtn.addEventListener('click', () => this.syncAndLoadData());
        if (this.exportBtn) this.exportBtn.addEventListener('click', () => this.exportToPdf());
    }

    handleViewChange(mode) {
        this.sectionHistory?.classList.add('d-none');
        this.sectionCategories?.classList.add('d-none');
        this.sectionTable?.classList.add('d-none');

        switch (mode) {
            case 'history':
                this.sectionHistory?.classList.remove('d-none');
                if (this.histChart) setTimeout(() => this.histChart.updateOptions({ chart: { width: '100%' } }), 50);
                break;
            case 'categories':
                this.sectionCategories?.classList.remove('d-none');
                if (this.catChart) setTimeout(() => this.catChart.updateOptions({ chart: { width: '100%' } }), 50);
                break;
            case 'table':
                this.sectionTable?.classList.remove('d-none');
                break;
        }
    }

    initPickers() {
        const commonConfig = {
            locale: "ru", dateFormat: "Y-m-d", altInput: true, altFormat: "d.m.Y",
            disableMobile: "true", maxDate: "today", allowInput: true
        };

        if (this.dateFromInput) {
            this.fpReportFrom = flatpickr(this.dateFromInput, {
                ...commonConfig,
                onChange: (selectedDates, dateStr) => {
                    if (this.fpReportTo) this.fpReportTo.set("minDate", dateStr);
                    this.calculateReport();
                }
            });
        }

        if (this.dateToInput) {
            this.fpReportTo = flatpickr(this.dateToInput, {
                ...commonConfig,
                onChange: (selectedDates, dateStr) => {
                    if (this.fpReportFrom) this.fpReportFrom.set("maxDate", dateStr);
                    this.calculateReport();
                }
            });
        }
    }

    updatePickersWithData() {
        if (!this.rawData.statusChanges || this.rawData.statusChanges.length === 0) return;
        const dates = this.rawData.statusChanges.map(sc => new Date(sc.date).getTime());
        if (dates.length === 0) return;

        const minISO = new Date(Math.min(...dates)).toISOString().slice(0, 10);
        const todayISO = new Date().toISOString().slice(0, 10);

        if (this.fpReportFrom) {
            this.fpReportFrom.set("minDate", minISO);
            if (!this.fpReportFrom.selectedDates.length) this.fpReportFrom.setDate(minISO, false);
        }

        if (this.fpReportTo) {
            this.fpReportTo.set("minDate", minISO);
            if (!this.fpReportTo.selectedDates.length) this.fpReportTo.setDate(todayISO, false);
        }

        if (this.fpReportFrom && this.fpReportTo) {
            this.fpReportTo.set("minDate", this.fpReportFrom.input.value);
            this.fpReportFrom.set("maxDate", this.fpReportTo.input.value);
        }
    }

    async syncAndLoadData() {
        try {
            toggleLoader(true);
            const tables = ['Orders', 'OrderCompositions', 'Furniture', 'FurnitureCompositions', 'Categories', 'Prices', 'StatusChanges'];
            try { await Promise.all(tables.map(t => syncTable(t))); } catch (e) { console.warn("Offline mode:", e); }

            const [orders, orderComps, furniture, furnComps, categories, prices, statusChanges] = await Promise.all([
                getAll('Orders'), getAll('OrderCompositions'), getAll('Furniture'), getAll('FurnitureCompositions'),
                getAll('Categories'), getAll('Prices'), getAll('StatusChanges')
            ]);

            this.rawData = { orders, orderComps, furniture, furnComps, categories, prices, statusChanges };
            this.updatePickersWithData();
            this.calculateReport();
        } catch (e) {
            console.error("Error loading report data:", e);
        } finally {
            toggleLoader(false);
        }
    }

    calculateReport() {
        const furnMap = new Map(this.rawData.furniture.map(f => [f.id, f]));
        const catMap = new Map(this.rawData.categories.map(c => [c.id, c]));

        const furnCompLookup = new Map();
        this.rawData.furnComps.forEach(fc => {
            const fId = fc.idFurniture ?? fc.entity1Id;
            const pId = fc.idPart ?? fc.entity2Id;
            if (!furnCompLookup.has(fId)) furnCompLookup.set(fId, []);
            furnCompLookup.get(fId).push({ partId: pId, count: fc.count });
        });

        const priceLookup = new Map();
        const sortedPrices = [...this.rawData.prices].sort((a, b) => new Date(b.date) - new Date(a.date));
        sortedPrices.forEach(p => {
            if (!priceLookup.has(p.partId)) priceLookup.set(p.partId, []);
            priceLookup.get(p.partId).push(p);
        });

        const orderDateLookup = new Map();
        this.rawData.statusChanges.forEach(sc => {
            const d = new Date(sc.date).getTime();
            if (!orderDateLookup.has(sc.idOrder) || d < orderDateLookup.get(sc.idOrder)) orderDateLookup.set(sc.idOrder, d);
        });

        const dFrom = this.fpReportFrom?.selectedDates[0] || null;
        const dTo = this.fpReportTo?.selectedDates[0] || null;
        if (dTo) dTo.setHours(23, 59, 59, 999);

        const reportLines = [];
        this.rawData.orders.forEach(order => {
            let orderDateTs = orderDateLookup.get(order.id) || (order.updateDate ? new Date(order.updateDate).getTime() : new Date().getTime());
            const orderDate = new Date(orderDateTs);

            if ((dFrom && orderDate < dFrom) || (dTo && orderDate > dTo)) return;

            this.rawData.orderComps.filter(oc => oc.idOrder === order.id).forEach(item => {
                const furnObj = furnMap.get(item.idFurniture);
                if (!furnObj) return;

                let unitCost = 0;
                (furnCompLookup.get(furnObj.id) || []).forEach(partLink => {
                    const history = priceLookup.get(partLink.partId);
                    let partPrice = history && history.length > 0
                        ? (history.find(p => new Date(p.date) <= orderDate) || history[history.length - 1]).value
                        : 0;
                    unitCost += partPrice * partLink.count;
                });

                const markup = furnObj.markup || 0;
                const unitSoldPrice = (unitCost + (unitCost * markup / 100)) * (1 - ((order.discount || 0) / 100));

                reportLines.push({
                    categoryName: catMap.get(furnObj.categoryId)?.name || "Без категории",
                    furnitureName: furnObj.name, orderDate, orderId: order.id, count: item.count,
                    totalCost: unitCost * item.count, totalRevenue: unitSoldPrice * item.count
                });
            });
        });

        const kpi = { revenue: 0, cost: 0, profit: 0, ordersCount: new Set() };
        const byCategoryMap = new Map();
        const byMonthMap = new Map();
        const monthlyFurnitureMap = new Map();

        reportLines.forEach(line => {
            const profit = line.totalRevenue - line.totalCost;
            kpi.revenue += line.totalRevenue; kpi.cost += line.totalCost; kpi.profit += profit;
            kpi.ordersCount.add(line.orderId);

            if (!byCategoryMap.has(line.categoryName)) byCategoryMap.set(line.categoryName, { name: line.categoryName, value: 0 });
            byCategoryMap.get(line.categoryName).value += line.totalRevenue;

            const mKey = `${line.orderDate.getFullYear()}-${String(line.orderDate.getMonth() + 1).padStart(2, '0')}`;
            if (!byMonthMap.has(mKey)) byMonthMap.set(mKey, { date: new Date(line.orderDate.getFullYear(), line.orderDate.getMonth(), 1), revenue: 0, cost: 0, profit: 0 });

            const mData = byMonthMap.get(mKey);
            mData.revenue += line.totalRevenue; mData.cost += line.totalCost; mData.profit += profit;

            if (!monthlyFurnitureMap.has(mKey)) monthlyFurnitureMap.set(mKey, new Map());
            const monthGroup = monthlyFurnitureMap.get(mKey);
            if (!monthGroup.has(line.furnitureName)) monthGroup.set(line.furnitureName, { name: line.furnitureName, count: 0, revenue: 0 });

            const fData = monthGroup.get(line.furnitureName);
            fData.count += line.count; fData.revenue += line.totalRevenue;
        });

        this.lastCalculatedData = {
            kpi: { ...kpi, ordersCount: kpi.ordersCount.size },
            byCategory: Array.from(byCategoryMap.values()).sort((a, b) => b.value - a.value),
            byMonth: Array.from(byMonthMap.values()).sort((a, b) => a.date - b.date),
            topFurniture: Array.from(byMonthMap.values()).sort((a, b) => a.date - b.date).map(monthData => {
                const mKey = `${monthData.date.getFullYear()}-${String(monthData.date.getMonth() + 1).padStart(2, '0')}`;
                return {
                    monthName: monthData.date.toLocaleString('ru-RU', { month: 'long', year: 'numeric' }),
                    items: Array.from(monthlyFurnitureMap.get(mKey).values()).sort((a, b) => b.revenue - a.revenue).slice(0, 10)
                };
            }).reverse(),
            dateRange: { from: dFrom ? dFrom.toLocaleDateString() : 'Начало', to: dTo ? dTo.toLocaleDateString() : 'Сейчас' }
        };

        this.renderDashboard();
    }

    renderDashboard() {
        const data = this.lastCalculatedData;
        if (this.kpiRevenue) this.kpiRevenue.textContent = this.fmtMoney.format(data.kpi.revenue);
        if (this.kpiCost) this.kpiCost.textContent = this.fmtMoney.format(data.kpi.cost);
        if (this.kpiProfit) {
            this.kpiProfit.textContent = this.fmtMoney.format(data.kpi.profit);
            this.kpiProfit.style.color = data.kpi.profit >= 0 ? 'var(--action-create)' : 'var(--danger-main)';
        }
        if (this.kpiOrders) this.kpiOrders.textContent = data.kpi.ordersCount;

        this.renderCategoryChart(data.byCategory);
        this.renderHistoryChart(data.byMonth);
        this.renderTopTable(data.topFurniture);
    }

    generateThemePalette(count) {
        const baseColors = ['#e27b32', '#1f5a35', '#2c5d63', '#500805', '#dba356', '#8a6540'];
        let palette = [];
        if (count <= baseColors.length) return baseColors.slice(0, count);
        const adjustColor = (color, amount) => '#' + color.replace(/^#/, '').replace(/../g, c => ('0' + Math.min(255, Math.max(0, parseInt(c, 16) + amount)).toString(16)).substr(-2));
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

    renderCategoryChart(items) {
        if (this.catChart) this.catChart.destroy();
        const series = items.map(x => x.value);
        if (this.chartCatTitle) this.chartCatTitle.innerHTML = 'Доля выручки по категориям';

        const isEmpty = series.length === 0 || series.every(x => x === 0);
        const emptyMsg = document.getElementById('chartCatEmpty');
        if (emptyMsg) emptyMsg.style.display = isEmpty ? 'block' : 'none';
        if (isEmpty) return;

        const options = {
            series: series, labels: items.map(x => x.name),
            chart: { type: 'donut', height: '100%', fontFamily: 'inherit', selection: { enabled: false }, events: { dataPointSelection: () => { }, markerClick: () => { } } },
            colors: this.generateThemePalette(series.length), stroke: { show: false, width: 0 }, dataLabels: { enabled: false },
            states: { normal: { filter: { type: 'none' } }, hover: { filter: { type: 'none' } }, active: { filter: { type: 'none' } } },
            tooltip: { enabled: false },
            legend: {
                show: true, position: 'bottom', fontSize: '14px', fontFamily: 'inherit', fontWeight: 500,
                labels: { colors: '#2b1a10', useSeriesColors: false }, itemMargin: { horizontal: 15, vertical: 8 }, markers: { strokeWidth: 0, radius: 12 },
                onItemClick: { toggleDataSeries: false }, onItemHover: { highlightDataSeries: false },
                formatter: (seriesName, opts) => {
                    const val = opts.w.globals.series[opts.seriesIndex];
                    const total = opts.w.globals.series.reduce((a, b) => a + b, 0);
                    return `${seriesName}: ${(total > 0 ? (val / total * 100) : 0).toFixed(1)}% (${this.fmtMoney.format(val)})`;
                }
            },
            plotOptions: { pie: { expandOnClick: false, donut: { size: '65%', labels: { show: false } } } }
        };
        this.catChart = new ApexCharts(document.querySelector("#chartCategories"), options);
        this.catChart.render();
    }

    renderHistoryChart(items) {
        if (this.histChart) this.histChart.destroy();
        const options = {
            series: [
                { name: 'Выручка', type: 'column', data: items.map(x => x.revenue) },
                { name: 'Себестоимость', type: 'column', data: items.map(x => x.cost) },
                { name: 'Прибыль', type: 'line', data: items.map(x => x.profit) }
            ],
            chart: { type: 'line', height: '100%', toolbar: { show: false }, fontFamily: 'inherit', zoom: { enabled: false }, events: { click: () => { }, markerClick: () => { } } },
            colors: ['#2c5d63', '#8c3b38', '#e27b32'],
            states: { normal: { filter: { type: 'none' } }, hover: { filter: { type: 'none' } }, active: { filter: { type: 'none' } } },
            tooltip: { enabled: false },
            plotOptions: { bar: { horizontal: false, columnWidth: '65%', borderRadius: 2 } }, stroke: { width: [0, 0, 4], curve: 'smooth' }, dataLabels: { enabled: false },
            xaxis: {
                categories: items.map(x => {
                    let m = x.date.toLocaleString('ru-RU', { month: 'short' }).replace('.', '');
                    return (m.length > 0 ? m[0].toUpperCase() + m.slice(1) : '') + ` ${x.date.getFullYear()}`;
                }),
                axisBorder: { show: true, color: 'rgba(125, 94, 60, 0.4)' }, axisTicks: { show: true, color: 'rgba(125, 94, 60, 0.4)' },
                tooltip: { enabled: false }, labels: { rotate: -45, style: { colors: '#8a6540', fontSize: '12px', fontFamily: 'inherit', fontWeight: 500 } }
            },
            yaxis: [{
                labels: {
                    formatter: (val) => val >= 1000000 ? (val / 1000000).toFixed(1).replace(/\.0$/, '') + ' млн ₽' : (val >= 1000 ? (val / 1000).toFixed(0) + ' тыс ₽' : val + ' ₽'),
                    style: { colors: '#8a6540', fontSize: '11px', fontFamily: 'inherit' }
                }
            }],
            grid: { borderColor: 'rgba(125, 94, 60, 0.15)', strokeDashArray: 4 },
            markers: { size: 5, colors: ['#e27b32'], strokeColors: '#fff', strokeWidth: 2, hover: { size: 5 } },
            legend: { show: true, position: 'top', horizontalAlign: 'right', floating: true, offsetY: -20, markers: { radius: 12, strokeWidth: 0 }, itemMargin: { horizontal: 10 }, onItemClick: { toggleDataSeries: false }, onItemHover: { highlightDataSeries: false } }
        };
        this.histChart = new ApexCharts(document.querySelector("#chartHistory"), options);
        this.histChart.render();
    }

    renderTopTable(groups) {
        const tbody = document.querySelector('#topTable tbody');
        if (!tbody) return;
        tbody.innerHTML = groups?.length ? groups.map(g => `
            <tr class="month-group-header"><td colspan="3"><div class="month-group-title"><span>📅</span><span>${g.monthName}</span></div></td></tr>
            ${g.items.map(item => `
                <tr class="month-item-row">
                    <td style="font-weight: 500;">${item.name}</td>
                    <td class="text-center"><span class="badge badge-theme" style="background-color: var(--btn-comp-bg); min-width: 30px;">${item.count}</span></td>
                    <td class="text-end fw-bold" style="padding-right: 1.5rem;">${this.fmtMoney.format(item.revenue)}</td>
                </tr>`).join('')}
        `).join('') : `<tr><td colspan="3" class="text-center p-3" style="opacity:0.6;">Нет продаж</td></tr>`;
    }

    async exportToPdf() {
        if (!this.lastCalculatedData) return;
        try {
            toggleLoader(true);
            const data = this.lastCalculatedData;
            if (!data.kpi || data.kpi.cost === undefined) throw new Error("Данные отсутствуют!");

            const imgHistory = await this.generateGhostImage('line',
                [{ name: 'Выручка', type: 'column', data: data.byMonth.map(x => x.revenue) }, { name: 'Себестоимость', type: 'column', data: data.byMonth.map(x => x.cost) }, { name: 'Прибыль', type: 'line', data: data.byMonth.map(x => x.profit) }],
                data.byMonth.map(x => { let m = x.date.toLocaleString('ru-RU', { month: 'short' }).replace('.', ''); return (m[0].toUpperCase() + m.slice(1)) + ' ' + x.date.getFullYear(); }),
                [], 1000, 500);

            const imgCategory = await this.generateGhostImage('donut', data.byCategory.map(x => x.value), data.byCategory.map(x => x.name), this.generateThemePalette(data.byCategory.length), 1000, 800);

            const createKpiBlock = () => ({
                style: 'kpiTable',
                table: {
                    widths: ['*', '*', '*', '*'], body: [
                        [{ text: 'Выручка', style: 'kpiLabel', fillColor: '#2c5d63' }, { text: 'Себестоимость', style: 'kpiLabel', fillColor: '#8c3b38' }, { text: 'Прибыль', style: 'kpiLabel', fillColor: '#e27b32' }, { text: 'Заказов', style: 'kpiLabel', fillColor: '#8a6540' }],
                        [{ text: this.fmtMoney.format(data.kpi.revenue), style: 'kpiValue' }, { text: this.fmtMoney.format(data.kpi.cost), style: 'kpiValue' }, { text: this.fmtMoney.format(data.kpi.profit), style: 'kpiValue' }, { text: data.kpi.ordersCount, style: 'kpiValue' }]
                    ]
                },
                layout: { hLineWidth: (i) => (i === 0 || i === 2) ? 1 : 0, vLineWidth: () => 0, hLineColor: '#ccc' }
            });

            pdfMake.createPdf({
                info: { title: `Отчет FurniturePro`, author: 'System' }, pageSize: 'A4', pageMargins: [30, 30, 30, 30],
                content: [
                    { text: 'РАЗДЕЛ 1: АНАЛИЗ ДИНАМИКИ ФИНАНСОВ', style: 'header' },
                    { text: `Период: ${data.dateRange.from} — ${data.dateRange.to}`, style: 'subheader' },
                    createKpiBlock(),
                    { text: 'График динамики (Выручка, Себестоимость, Прибыль)', style: 'sectionTitle' },
                    { image: imgHistory, width: 535, alignment: 'center', margin: [0, 0, 0, 15] },
                    { text: 'Таблица детализации по месяцам', style: 'sectionTitle' },
                    { style: 'dataTable', layout: 'lightHorizontalLines', table: { headerRows: 1, widths: ['*', 'auto', 'auto', 'auto'], body: [[{ text: 'Месяц', style: 'th' }, { text: 'Выручка', style: 'th', alignment: 'right' }, { text: 'Себестоимость', style: 'th', alignment: 'right' }, { text: 'Прибыль', style: 'th', alignment: 'right' }], ...data.byMonth.map(m => [m.date.toLocaleDateString('ru-RU', { month: 'long', year: 'numeric' }), { text: this.fmtMoney.format(m.revenue), alignment: 'right' }, { text: this.fmtMoney.format(m.cost), alignment: 'right' }, { text: this.fmtMoney.format(m.profit), alignment: 'right' }])] } },
                    { text: '', pageBreak: 'before' },
                    { text: 'РАЗДЕЛ 2: АНАЛИЗ КАТЕГОРИЙ ПРОДАЖ', style: 'header' },
                    { text: `Период: ${data.dateRange.from} — ${data.dateRange.to}`, style: 'subheader' },
                    createKpiBlock(),
                    { text: 'Распределение выручки по категориям', style: 'sectionTitle' },
                    { image: imgCategory, width: 450, alignment: 'center', margin: [0, 0, 0, 15] },
                    { text: 'Таблица долей категорий', style: 'sectionTitle' },
                    { style: 'dataTable', layout: 'lightHorizontalLines', table: { headerRows: 1, widths: ['*', 'auto', 'auto'], body: [[{ text: 'Категория', style: 'th' }, { text: 'Выручка', style: 'th', alignment: 'right' }, { text: 'Доля (%)', style: 'th', alignment: 'right' }], ...data.byCategory.map(c => [c.name, { text: this.fmtMoney.format(c.value), alignment: 'right' }, { text: (data.kpi.revenue > 0 ? (c.value / data.kpi.revenue * 100) : 0).toFixed(1) + '%', alignment: 'right' }])] } },
                    { text: '', pageBreak: 'before' },
                    { text: 'РАЗДЕЛ 3: ТОП ПРОДАЖ ПО МЕСЯЦАМ', style: 'header' },
                    { text: `Период: ${data.dateRange.from} — ${data.dateRange.to}`, style: 'subheader' },
                    createKpiBlock(),
                    { text: 'Детализация ТОП-10 позиций мебели по месяцам', style: 'sectionTitle' },
                    { style: 'dataTable', layout: 'lightHorizontalLines', table: { headerRows: 1, widths: ['*', 'auto', 'auto'], body: [[{ text: 'Название / Месяц', style: 'th' }, { text: 'Кол-во (шт)', style: 'th', alignment: 'center' }, { text: 'Выручка', style: 'th', alignment: 'right' }], ...data.topFurniture.flatMap(group => [[{ text: group.monthName.toUpperCase(), fillColor: '#f0f0f0', bold: true, colSpan: 3, margin: [0, 2, 0, 2] }, {}, {}], ...group.items.map(f => [{ text: f.name, margin: [10, 0, 0, 0] }, { text: f.count, alignment: 'center' }, { text: this.fmtMoney.format(f.revenue), alignment: 'right' }])])] } }
                ],
                styles: { header: { fontSize: 16, bold: true, alignment: 'center', color: '#2b1a10', margin: [0, 0, 0, 5] }, subheader: { fontSize: 10, italics: true, alignment: 'center', color: '#8a6540', margin: [0, 0, 0, 15] }, kpiTable: { margin: [0, 0, 0, 15] }, kpiLabel: { color: 'white', bold: true, fontSize: 9, alignment: 'center', margin: [0, 4, 0, 4] }, kpiValue: { fontSize: 11, bold: true, alignment: 'center', margin: [0, 4, 0, 4], color: '#2b1a10' }, sectionTitle: { fontSize: 12, bold: true, color: '#2c5d63', margin: [0, 5, 0, 5] }, th: { bold: true, fontSize: 9, color: 'black', fillColor: '#f0f0f0', margin: [0, 3, 0, 3] }, dataTable: { fontSize: 9, margin: [0, 0, 0, 10] } }
            }).download(`Детальный Отчет ${new Date().toISOString().slice(0, 10)}.pdf`);
        } catch (e) {
            console.error("PDF Error:", e);
            alert("Ошибка PDF: " + e.message);
        } finally {
            toggleLoader(false);
        }
    }

    async generateGhostImage(type, series, labels, colors, width, height) {
        const div = document.createElement('div');
        div.style.cssText = `position:fixed; left:-10000px; top:-10000px; width:${width}px; height:${height}px; background:#fff;`;
        document.body.appendChild(div);

        const options = {
            series, chart: { type, width, height, animations: { enabled: false }, toolbar: { show: false }, zoom: { enabled: false } },
            dataLabels: { enabled: false }, stroke: { width: type === 'line' ? [0, 0, 4] : 0, curve: 'smooth' },
            colors: colors.length > 0 ? colors : ['#2c5d63', '#8c3b38', '#e27b32'],
            legend: { show: true, position: 'bottom', fontSize: '16px', fontWeight: 600, itemMargin: { horizontal: 10, vertical: 8 } }
        };

        if (type === 'donut') {
            options.labels = labels;
            options.plotOptions = { pie: { donut: { size: '65%', labels: { show: false } } } };
            options.legend.formatter = (sName, opts) => {
                const total = opts.w.globals.series.reduce((a, b) => a + b, 0);
                return `${sName}: ${total > 0 ? ((opts.w.globals.series[opts.seriesIndex] / total) * 100).toFixed(1) : 0}%`;
            };
        } else {
            options.xaxis = { categories: labels, labels: { style: { fontSize: '14px', fontWeight: 600 } } };
            options.plotOptions = { bar: { horizontal: false, columnWidth: '65%', borderRadius: 0 } };
            options.yaxis = { labels: { style: { fontSize: '14px' }, formatter: val => val >= 1000000000 ? (val / 1000000000).toFixed(1) + ' млрд ₽' : (val >= 1000000 ? (val / 1000000).toFixed(1) + ' млн ₽' : (val >= 1000 ? (val / 1000).toFixed(0) + ' к ₽' : val + ' ₽')) } };
        }

        const chart = new ApexCharts(div, options);
        await chart.render();
        await new Promise(r => setTimeout(r, 400));
        const uri = await chart.dataURI({ scale: 1 });
        chart.destroy();
        document.body.removeChild(div);
        return uri.imgURI;
    }
}

document.addEventListener('DOMContentLoaded', () => {
    const page = new ReportPage();
    page.init();
});