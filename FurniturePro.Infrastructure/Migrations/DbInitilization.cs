using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FurniturePro.Infrastructure.Migrations;

public static class DbInitilization
{
    public static void AddFixedData(AppDbContext context)
    {
        context.Database.EnsureCreated();

        var now = DateTime.UtcNow;

        if (!context.Statuses.AsNoTracking().Any())
        {
            var items = new Status[]
            {
                new() { Name = "Новый", Description = "Заказ создан, но еще не обработан", UpdateDate = now },
                new() { Name = "В обработке", Description = "Проверка наличия, расчет стоимости и сроков", UpdateDate = now },
                new() { Name = "Ожидает оплаты", Description = "Выставлен счет, ожидается оплата от клиента", UpdateDate = now },
                new() { Name = "Оплачен", Description = "Оплата получена, заказ передан в производство/комплектацию", UpdateDate = now },
                new() { Name = "В производстве", Description = "Идет изготовление/комплектация мебели по заказу", UpdateDate = now },
                new() { Name = "Готов к отгрузке", Description = "Заказ укомплектован и находится на складе готовой продукции", UpdateDate = now },
                new() { Name = "Отгружен", Description = "Заказ передан в доставку или выдан клиенту", UpdateDate = now },
                new() { Name = "Доставлен",Description = "Клиент получил заказ, работа по заказу завершена", UpdateDate = now },
                new() { Name = "На паузе", Description = "Исполнение заказа временно приостановлено", UpdateDate = now },
                new() { Name = "Отменен", Description = "Заказ отменен клиентом или менеджером", UpdateDate = now },
            };

            context.Statuses.AddRange(items);
            context.SaveChanges();
        }

        if (!context.OperationTypes.AsNoTracking().Any())
        {
            var items = new OperationType[]
            {
                new() { Name = "Пополнение", Description = "Пополнение деталей на скалд", UpdateDate = now },
                new() { Name = "Списание", Description = "Списание деталей со склада", UpdateDate = now },
            };

            context.OperationTypes.AddRange(items);
            context.SaveChanges();
        }
    }

    public static void AddExampleData(AppDbContext context)
    {
        context.Database.EnsureCreated();

        var now = DateTime.UtcNow;

        if (!context.Categories.AsNoTracking().Any())
        {
            var items = new Category[]
            {
                new() { Name = "Столы", Description = "Обеденные, письменные и рабочие столы", UpdateDate = now },
                new() { Name = "Стулья и табуреты", Description = "Кухонные, обеденные стулья и табуреты", UpdateDate = now },
                new() { Name = "Диваны", Description = "Прямые, угловые и модульные диваны", UpdateDate = now },
                new() { Name = "Кресла", Description = "Кресла для гостиной, реклайнеры и т.п.", UpdateDate = now },
                new() { Name = "Кровати", Description = "Одно- и двуспальные кровати, подиумы", UpdateDate = now },
                new() { Name = "Матрасы и аксессуары для сна", Description = "Матрасы, наматрасники, основания", UpdateDate = now },
                new() { Name = "Шкафы и гардеробы", Description = "Шкафы-купе, распашные шкафы и гардеробные системы", UpdateDate = now },
                new() { Name = "Комоды и тумбы", Description = "Комоды, прикроватные и прикроватные тумбы", UpdateDate = now },
                new() { Name = "Стеллажи и полки", Description = "Открытые стеллажи, навесные полки", UpdateDate = now },
                new() { Name = "Журнальные и приставные столики", Description = "Журнальные, кофейные, приставные столики", UpdateDate = now },
                new() { Name = "Офисные столы", Description = "Письменные и компьютерные столы для офиса", UpdateDate = now },
                new() { Name = "Офисные кресла и стулья", Description = "Кресла оператора, посетительские стулья", UpdateDate = now },
                new() { Name = "Детская мебель", Description = "Кровати, столы и системы хранения для детской", UpdateDate = now },
                new() { Name = "Мебель для прихожей", Description = "Обувницы, вешалки, консоли и шкафы для прихожей", UpdateDate = now },
                new() { Name = "Кухонные гарнитуры", Description = "Кухонные модули, навесные и напольные шкафы", UpdateDate = now },
                new() { Name = "Уличная и садовая мебель", Description = "Мебель для террасы, сада и балкона", UpdateDate = now },
            };

            context.Categories.AddRange(items);
            context.SaveChanges();
        }

        if (!context.Colors.AsNoTracking().Any())
        {
            var items = new Color[]
            {
                new() { Name = "Белый", Description = "Чистый матовый белый", UpdateDate = now },
                new() { Name = "Молочный белый", Description = "Теплый молочный оттенок белого", UpdateDate = now },
                new() { Name = "Светло-серый", Description = "Нейтральный светло-серый", UpdateDate = now },
                new() { Name = "Серый графит", Description = "Темный графитовый серый", UpdateDate = now },
                new() { Name = "Черный", Description = "Глубокий черный матовый", UpdateDate = now },

                new() { Name = "Бежевый", Description = "Нейтральный бежевый", UpdateDate = now },
                new() { Name = "Кремовый", Description = "Теплый кремовый оттенок", UpdateDate = now },
                new() { Name = "Песочный", Description = "Светлый песочно-бежевый цвет", UpdateDate = now },
                new() { Name = "Капучино", Description = "Теплый коричнево-бежевый", UpdateDate = now },

                new() { Name = "Дуб натуральный", Description = "Светлый натуральный дуб", UpdateDate = now },
                new() { Name = "Дуб сонома", Description = "Декор дуб сонома с выраженной текстурой", UpdateDate = now },
                new() { Name = "Дуб выбеленный", Description = "Светлый выбеленный дуб", UpdateDate = now },
                new() { Name = "Дуб медовый", Description = "Теплый медово-коричневый дуб", UpdateDate = now },
                new() { Name = "Дуб дымчатый", Description = "Холодный серо-коричневый дуб", UpdateDate = now },
                new() { Name = "Орех классический", Description = "Классический коричневый орех", UpdateDate = now },
                new() { Name = "Орех темный", Description = "Насыщенный темный орех", UpdateDate = now },
                new() { Name = "Венге", Description = "Очень темный коричнево-черный венге", UpdateDate = now },
                new() { Name = "Тик", Description = "Золотисто-коричневый тик", UpdateDate = now },
                new() { Name = "Сосна натуральная", Description = "Светлый желтоватый оттенок сосны", UpdateDate = now },
                new() { Name = "Бук натуральный", Description = "Светло-розовато-бежевый бук", UpdateDate = now },

                new() { Name = "Дуб серый", Description = "Дуб с холодным серым тоном", UpdateDate = now },
                new() { Name = "Серый бетон", Description = "Декор под архитектурный бетон", UpdateDate = now },
                new() { Name = "Сланцевый серый", Description = "Темный серый сланцевый оттенок", UpdateDate = now },

                new() { Name = "Синий индиго", Description = "Глубокий синий для акцентных деталей и обивки", UpdateDate = now },
                new() { Name = "Темно-синий", Description = "Насыщенный темно-синий", UpdateDate = now },
                new() { Name = "Бирюзовый", Description = "Холодный бирюзовый оттенок", UpdateDate = now },
                new() { Name = "Изумрудный зеленый", Description = "Глубокий зеленый для акцентной мебели", UpdateDate = now },
                new() { Name = "Оливковый", Description = "Приглушенный зеленовато-серый", UpdateDate = now },
                new() { Name = "Горчичный", Description = "Желто-охристый для мягкой мебели", UpdateDate = now },
                new() { Name = "Терракотовый", Description = "Теплый красно-коричневый", UpdateDate = now },
                new() { Name = "Бордовый", Description = "Глубокий красно-бордовый", UpdateDate = now },
                new() { Name = "Пудрово-розовый", Description = "Приглушенный розовый для современных интерьеров", UpdateDate = now },

                new() { Name = "Металлик хром", Description = "Серебристый металлический блеск", UpdateDate = now },
                new() { Name = "Металлик никель", Description = "Теплый матовый металл", UpdateDate = now },
                new() { Name = "Металлик черный", Description = "Черный металл с легким глянцем", UpdateDate = now },

                new() { Name = "Антрацит", Description = "Очень темный серо-черный антрацит", UpdateDate = now },
                new() { Name = "Темно-зеленый уличный", Description = "Классический цвет для уличной мебели", UpdateDate = now },
                new() { Name = "Коричневый шоколад", Description = "Насыщенный коричневый для ротанга и пластика", UpdateDate = now },
                new() { Name = "Светло-голубой", Description = "Нежный голубой для детской мебели и акцентов", UpdateDate = now },
                new() { Name = "Желтый пастельный", Description = "Мягкий желтый пастельный оттенок", UpdateDate = now },
            };

            context.Colors.AddRange(items);
            context.SaveChanges();
        }

        if (!context.Materials.AsNoTracking().Any())
        {
            var items = new Material[]
            {
                new() { Name = "Массив дуба", Description = "Натуральное дерево дуба для столешниц и корпусов", UpdateDate = now },
                new() { Name = "Массив бука", Description = "Твердая порода для стульев, ножек и каркасов", UpdateDate = now },
                new() { Name = "Массив сосны", Description = "Мягкая хвойная порода для недорогой корпусной мебели", UpdateDate = now },
                new() { Name = "Массив ясеня", Description = "Твердая светлая порода для столов и фасадов", UpdateDate = now },
                new() { Name = "Массив березы", Description = "Светлая порода для каркасов и ящиков", UpdateDate = now },

                new() { Name = "Шпон дуба на плите", Description = "Плита с облицовкой шпоном дуба под лак/масло", UpdateDate = now },
                new() { Name = "Шпон ореха на плите", Description = "Плита с облицовкой шпоном ореха для премиальных фасадов", UpdateDate = now },

                new() { Name = "ЛДСП белая", Description = "Ламинированная ДСП белого цвета для корпусов", UpdateDate = now },
                new() { Name = "ЛДСП дуб сонома", Description = "ЛДСП с декором под дуб сонома", UpdateDate = now },
                new() { Name = "ЛДСП серый бетон", Description = "ЛДСП с урбанистичным декором под бетон", UpdateDate = now },
                new() { Name = "МДФ под покраску", Description = "МДФ без покрытия для лакокрасочной отделки", UpdateDate = now },
                new() { Name = "МДФ влагостойкий", Description = "Влагостойкая плита МДФ для кухонь и ванных", UpdateDate = now },
                new() { Name = "Фанера березовая", Description = "Фанера для оснований, сидений и элементов каркаса", UpdateDate = now },

                new() { Name = "Сталь хромированная", Description = "Хромированные стальные трубы для опор и ножек", UpdateDate = now },
                new() { Name = "Алюминий анодированный", Description = "Легкий алюминиевый профиль для каркасов и фурнитуры", UpdateDate = now },
                new() { Name = "Нержавеющая сталь", Description = "Коррозионностойкие детали для кухонь и общественных зон", UpdateDate = now },

                new() { Name = "Стекло закаленное прозрачное", Description = "Закаленное прозрачное стекло для столешниц и полок", UpdateDate = now },
                new() { Name = "Стекло закаленное тонированное",Description = "Тонированное стекло для витрин и журнальных столиков", UpdateDate = now },
                new() { Name = "Керамика/керамогранит", Description = "Керамогранитные плиты для столешниц и накладок", UpdateDate = now },

                new() { Name = "Пластик ABS", Description = "ABS‑пластик для опор, заглушек и декоративных деталей", UpdateDate = now },
                new() { Name = "Пластик полипропилен", Description = "Полипропилен для сидений, спинок и ножек стульев", UpdateDate = now },
                new() { Name = "ПВХ кромочный материал",Description = "ПВХ‑кромка для облицовки торцов плитных деталей", UpdateDate = now },

                new() { Name = "Ткань мебельная", Description = "Износостойкая ткань для мягкой мебели и сидений", UpdateDate = now },
                new() { Name = "Экокожа", Description = "Искусственная кожа для кресел, стульев и изголовий", UpdateDate = now },
                new() { Name = "Натуральная кожа", Description = "Натуральная кожа премиум‑класса для кресел и диванов", UpdateDate = now },
                new() { Name = "Пенополиуретан (ППУ)", Description = "Поролон и формованный ППУ для сидений и спинок", UpdateDate = now },
            };

            context.Materials.AddRange(items);
            context.SaveChanges();
        }

        if (!context.Parts.AsNoTracking().Any())
        {
            var items = new Part[]
            {
                new() { Name = "Фасад верхнего шкафа 300x720 белый", Length = 720, Width = 296, Height = 18, Diameter = 0, Weight = (decimal?)3.2, Activity = true, ColorId = 1,  MaterialId = 11, Description = "МДФ под покраску, матовый белый, кромка ПВХ", UpdateDate = now }, // 1
                new() { Name = "Фасад верхнего шкафа 400x720 белый", Length = 720, Width = 396, Height = 18, Diameter = 0, Weight = (decimal?)3.9, Activity = true, ColorId = 1,  MaterialId = 11, Description = "Распашной фасад кухни, белый матовый", UpdateDate = now }, // 2
                new() { Name = "Фасад верхнего шкафа 600x720 белый", Length = 720, Width = 596, Height = 18, Diameter = 0, Weight = (decimal?)5.7, Activity = true, ColorId = 1,  MaterialId = 11, Description = "Широкий фасад для кухонного шкафа, белый", UpdateDate = now }, // 3
                new() { Name = "Фасад верхнего шкафа 400x720 дуб сонома", Length = 720, Width = 396, Height = 18, Diameter = 0, Weight = (decimal?)3.8, Activity = true, ColorId = 11, MaterialId = 9,  Description = "Фасад из ЛДСП дуб сонома", UpdateDate = now }, // 4
                new() { Name = "Фасад верхнего шкафа 600x720 дуб сонома", Length = 720, Width = 596, Height = 18, Diameter = 0, Weight = (decimal?)5.5, Activity = true, ColorId = 11, MaterialId = 9,  Description = "Широкий фасад ЛДСП дуб сонома", UpdateDate = now }, // 5
                new() { Name = "Фасад верхнего шкафа 400x720 серый бетон", Length = 720, Width = 396, Height = 18, Diameter = 0, Weight = (decimal?)3.9, Activity = true, ColorId = 22, MaterialId = 10, Description = "Фасад ЛДСП с декором серый бетон", UpdateDate = now }, // 6
                new() { Name = "Фасад верхнего шкафа 600x720 серый бетон", Length = 720, Width = 596, Height = 18, Diameter = 0, Weight = (decimal?)5.7, Activity = true, ColorId = 22, MaterialId = 10, Description = "Фасад широкого шкафа, серый бетон", UpdateDate = now }, // 7
                new() { Name = "Фасад витрины 400x720 белый со стеклом", Length = 720, Width = 396, Height = 22, Diameter = 0, Weight = (decimal?)5.0, Activity = true, ColorId = 1,  MaterialId = 11, Description = "Рамочный фасад МДФ белый с вставкой из прозрачного стекла", UpdateDate = now }, // 8
                new() { Name = "Фасад витрины 600x720 белый со стеклом", Length = 720, Width = 596, Height = 22, Diameter = 0, Weight = (decimal?)6.8, Activity = true, ColorId = 1,  MaterialId = 11, Description = "Широкий фасад витрины со стеклом", UpdateDate = now }, // 9
                new() { Name = "Фасад высокий 450x2100 дуб натуральный", Length = 2100, Width = 446, Height = 18, Diameter = 0, Weight = (decimal?)15.2, Activity = true, ColorId = 10, MaterialId = 6,  Description = "Фасад пенала кухни, шпон дуба", UpdateDate = now }, // 10

                new() { Name = "Фасад нижнего шкафа 400x720 белый", Length = 720, Width = 396, Height = 18, Diameter = 0, Weight = (decimal?)4.0, Activity = true, ColorId = 1,  MaterialId = 11, Description = "Фасад под напольный шкаф, белый матовый", UpdateDate = now }, // 11
                new() { Name = "Фасад нижнего шкафа 600x720 белый", Length = 720, Width = 596, Height = 18, Diameter = 0, Weight = (decimal?)5.8, Activity = true, ColorId = 1,  MaterialId = 11, Description = "Фасад под мойку или шкаф 600, белый", UpdateDate = now }, // 12
                new() { Name = "Фасад выдвижного ящика 400x140 белый", Length = 140, Width = 396, Height = 18, Diameter = 0, Weight = (decimal?)1.2, Activity = true, ColorId = 1,  MaterialId = 11, Description = "Фасад узкого выдвижного ящика", UpdateDate = now }, // 13
                new() { Name = "Фасад выдвижного ящика 600x140 белый", Length = 140, Width = 596, Height = 18, Diameter = 0, Weight = (decimal?)1.8, Activity = true, ColorId = 1,  MaterialId = 11, Description = "Фасад широкого выдвижного ящика", UpdateDate = now }, // 14
                new() { Name = "Фасад выдвижного ящика 600x280 дуб сонома", Length = 280, Width = 596, Height = 18, Diameter = 0, Weight = (decimal?)2.4, Activity = true, ColorId = 11, MaterialId = 9,  Description = "Высокий фасад ящика из ЛДСП дуб сонома", UpdateDate = now }, // 15
                new() { Name = "Фасад выдвижного ящика 600x140 серый графит", Length = 140, Width = 596, Height = 18, Diameter = 0, Weight = (decimal?)1.9, Activity = true, ColorId = 4,  MaterialId = 11, Description = "Фасад ящика в темно-сером цвете", UpdateDate = now }, // 16
                new() { Name = "Фасад выдвижного ящика 450x140 дуб серый", Length = 140, Width = 446, Height = 18, Diameter = 0, Weight = (decimal?)1.5, Activity = true, ColorId = 21, MaterialId = 6,  Description = "Фасад ящика, шпон дуба с серым оттенком", UpdateDate = now }, // 17
                new() { Name = "Фасад нижнего шкафа 450x720 дуб серый", Length = 720, Width = 446, Height = 18, Diameter = 0, Weight = (decimal?)4.7, Activity = true, ColorId = 21, MaterialId = 6,  Description = "Распашной фасад кухни, дуб серый", UpdateDate = now }, // 18
                new() { Name = "Фасад глухой 600x920 белый высокий", Length = 920, Width = 596, Height = 18, Diameter = 0, Weight = (decimal?)7.0, Activity = true, ColorId = 1,  MaterialId = 11, Description = "Высокий фасад для кладовой или шкафа", UpdateDate = now }, // 19
                new() { Name = "Фасад глухой 600x920 дуб дымчатый", Length = 920, Width = 596, Height = 18, Diameter = 0, Weight = (decimal?)7.1, Activity = true, ColorId = 14, MaterialId = 6,  Description = "Высокий фасад дуб дымчатый", UpdateDate = now }, // 20

                new() { Name = "Боковая стенка шкафа 720x560 белая", Length = 720, Width = 560, Height = 16, Diameter = 0, Weight = (decimal?)4.5, Activity = true, ColorId = 1,  MaterialId = 8,  Description = "Боковая деталь корпуса из ЛДСП белая", UpdateDate = now }, // 21
                new() { Name = "Боковая стенка шкафа 720x560 дуб сонома", Length = 720, Width = 560, Height = 16, Diameter = 0, Weight = (decimal?)4.6, Activity = true, ColorId = 11, MaterialId = 9,  Description = "Корпусная деталь из ЛДСП дуб сонома", UpdateDate = now }, // 22
                new() { Name = "Полка шкафа 700x300 белая", Length = 700, Width = 300, Height = 16, Diameter = 0, Weight = (decimal?)2.0, Activity = true, ColorId = 1,  MaterialId = 8,  Description = "Внутренняя полка корпуса, белая ЛДСП", UpdateDate = now }, // 23
                new() { Name = "Полка шкафа 900x300 дуб сонома", Length = 900, Width = 300, Height = 16, Diameter = 0, Weight = (decimal?)2.5, Activity = true, ColorId = 11, MaterialId = 9,  Description = "Удлиненная полка шкафа", UpdateDate = now }, // 24
                new() { Name = "Полка стеклянная 600x300 прозрачная", Length = 600, Width = 300, Height = 8, Diameter = 0, Weight = (decimal?)3.2, Activity = true, ColorId = 1,  MaterialId = 17, Description = "Закаленное стекло для витрины", UpdateDate = now }, // 25
                new() { Name = "Задняя стенка шкафа 720x560 белая", Length = 720, Width = 560, Height = 3, Diameter = 0, Weight = (decimal?)1.2, Activity = true, ColorId = 1,  MaterialId = 13, Description = "Тонкая фанерная задняя стенка", UpdateDate = now }, // 26
                new() { Name = "Задняя стенка шкафа 2100x600 белая", Length = 2100, Width = 600, Height = 3, Diameter = 0, Weight = (decimal?)3.5, Activity = true, ColorId = 1,  MaterialId = 13, Description = "Задняя стенка шкафа-пенала", UpdateDate = now }, // 27
                new() { Name = "Дно выдвижного ящика 500x450 белое", Length = 500, Width = 450, Height = 3, Diameter = 0, Weight = (decimal?)0.9, Activity = true, ColorId = 1,  MaterialId = 13, Description = "Фанерное дно стандартного ящика", UpdateDate = now }, // 28
                new() { Name = "Цокольная планка 2400x100 белая", Length = 2400, Width = 100, Height = 16, Diameter = 0, Weight = (decimal?)4.0, Activity = true, ColorId = 1,  MaterialId = 8,  Description = "Планка цоколя кухни, белая", UpdateDate = now }, // 29
                new() { Name = "Цокольная планка 2400x100 серый графит", Length = 2400, Width = 100, Height = 16, Diameter = 0, Weight = (decimal?)4.1, Activity = true, ColorId = 4,  MaterialId = 8,  Description = "Планка цоколя в темно-сером цвете", UpdateDate = now }, // 30

                new() { Name = "Столешница кухонная 2000x600 дуб натуральный", Length = 2000, Width = 600, Height = 26, Diameter = 0, Weight = (decimal?)34.0, Activity = true, ColorId = 10, MaterialId = 6,  Description = "Столешница шпон дуба на плите", UpdateDate = now }, // 31
                new() { Name = "Столешница кухонная 3000x600 дуб натуральный", Length = 3000, Width = 600, Height = 26, Diameter = 0, Weight = (decimal?)50.0, Activity = true, ColorId = 10, MaterialId = 6,  Description = "Удлиненная столешница под кухонный ряд", UpdateDate = now }, // 32
                new() { Name = "Столешница кухонная 2000x600 серый бетон", Length = 2000, Width = 600, Height = 38, Diameter = 0, Weight = (decimal?)40.0, Activity = true, ColorId = 22, MaterialId = 10, Description = "Столешница ЛДСП с кромкой ПВХ, бетон", UpdateDate = now }, // 33
                new() { Name = "Столешница обеденного стола 1600x800 дуб массив", Length = 1600, Width = 800, Height = 30, Diameter = 0, Weight = (decimal?)42.0, Activity = true, ColorId = 10, MaterialId = 1,  Description = "Массив дуба, шлифованный и покрытый маслом", UpdateDate = now }, // 34
                new() { Name = "Столешница обеденного стола 1400x800 ясень", Length = 1400, Width = 800, Height = 28, Diameter = 0, Weight = (decimal?)35.0, Activity = true, ColorId = 10, MaterialId = 4,  Description = "Массив ясеня для стола", UpdateDate = now }, // 35
                new() { Name = "Столешница журнального стола 900x600 стекло", Length = 900, Width = 600, Height = 10, Diameter = 0, Weight = (decimal?)12.0, Activity = true, ColorId = 1,  MaterialId = 17, Description = "Закаленное стекло для журнального столика", UpdateDate = now }, // 36
                new() { Name = "Столешница барной стойки 1600x400 тик", Length = 1600, Width = 400, Height = 30, Diameter = 0, Weight = (decimal?)25.0, Activity = true, ColorId = 18, MaterialId = 1,  Description = "Барная столешница из массива тика", UpdateDate = now }, // 37
                new() { Name = "Столешница кофейного столика 700x700 дуб выбеленный", Length = 700, Width = 700, Height = 28, Diameter = 0, Weight = (decimal?)18.0, Activity = true, ColorId = 12, MaterialId = 1,  Description = "Квадратная столешница дуб выбеленный", UpdateDate = now }, // 38
                new() { Name = "Столешница письменного стола 1200x600 серый графит", Length = 1200, Width = 600, Height = 25, Diameter = 0, Weight = (decimal?)20.0, Activity = true, ColorId = 4,  MaterialId = 8,  Description = "ЛДСП с кромкой ПВХ, графит", UpdateDate = now }, // 39
                new() { Name = "Столешница письменного стола 1400x700 дуб сонома", Length = 1400, Width = 700, Height = 25, Diameter = 0, Weight = (decimal?)24.0, Activity = true, ColorId = 11, MaterialId = 9,  Description = "Столешница для рабочего стола", UpdateDate = now }, // 40

                new() { Name = "Опора стола круглая 710 мм хром", Length = 710, Width = 0, Height = 0, Diameter = 60, Weight = (decimal?)3.2, Activity = true, ColorId = 33, MaterialId = 14, Description = "Металлическая труба с регулируемой опорой", UpdateDate = now }, // 41
                new() { Name = "Опора стола круглая 710 мм черный металл", Length = 710, Width = 0, Height = 0, Diameter = 60, Weight = (decimal?)3.3, Activity = true, ColorId = 35, MaterialId = 14, Description = "Опора для стола, порошковая окраска черная", UpdateDate = now }, // 42
                new() { Name = "Ножка стола профильная 710x60x30 черный", Length = 710, Width = 60, Height = 30, Diameter = 0, Weight = (decimal?)2.8, Activity = true, ColorId = 35, MaterialId = 14, Description = "П-образная металлическая ножка", UpdateDate = now }, // 43
                new() { Name = "Ножка стола профильная 710x80x40 антрацит", Length = 710, Width = 80, Height = 40, Diameter = 0, Weight = (decimal?)3.0, Activity = true, ColorId = 36, MaterialId = 14, Description = "Металлическая опора для рабочего стола", UpdateDate = now }, // 44
                new() { Name = "Комплект ножек стола конусных бук", Length = 720, Width = 60, Height = 60, Diameter = 0, Weight = (decimal?)4.5, Activity = true, ColorId = 20, MaterialId = 2,  Description = "4 конусные ножки из массива бука", UpdateDate = now }, // 45
                new() { Name = "Опора дивана пластиковая черная 50 мм", Length = 50, Width = 50, Height = 50, Diameter = 0, Weight = (decimal?)0.1, Activity = true, ColorId = 5,  MaterialId = 20, Description = "Квадратная пластиковая опора дивана", UpdateDate = now }, // 46
                new() { Name = "Опора регулируемая кухонного шкафа 100 мм", Length = 100, Width = 50, Height = 50, Diameter = 0, Weight = (decimal?)0.12, Activity = true, ColorId = 5,  MaterialId = 20, Description = "Регулируемая опора для цоколя кухни", UpdateDate = now }, // 47
                new() { Name = "Комплект роликовых опор для тумбы", Length = 50, Width = 50, Height = 60, Diameter = 0, Weight = (decimal?)0.6, Activity = true, ColorId = 33, MaterialId = 14, Description = "4 роликовых колеса с креплением", UpdateDate = now }, // 48
                new() { Name = "Ножка кровати металлическая 250 мм черный", Length = 250, Width = 40, Height = 40, Diameter = 0, Weight = (decimal?)0.6, Activity = true, ColorId = 35, MaterialId = 14, Description = "Прямая опора для кровати или тумбы", UpdateDate = now }, // 49
                new() { Name = "Ножка шкафа регулируемая 60 мм", Length = 60, Width = 40, Height = 40, Diameter = 0, Weight = (decimal?)0.08, Activity = true, ColorId = 5,  MaterialId = 20, Description = "Пластиковая регулируемая опора шкафа", UpdateDate = now }, // 50

                new() { Name = "Спинка кровати 1600x1000 дуб медовый", Length = 1000, Width = 1600, Height = 30, Diameter = 0, Weight = (decimal?)28.0, Activity = true, ColorId = 13, MaterialId = 1,  Description = "Щит из массива дуба для изголовья кровати", UpdateDate = now }, // 51
                new() { Name = "Царга кровати 2000x200 дуб медовый", Length = 2000, Width = 200, Height = 25, Diameter = 0, Weight = (decimal?)12.0, Activity = true, ColorId = 13, MaterialId = 1,  Description = "Боковая царга двухспальной кровати", UpdateDate = now }, // 52
                new() { Name = "Центральная балка кровати 2000x80 сосна", Length = 2000, Width = 80, Height = 40, Diameter = 0, Weight = (decimal?)7.0, Activity = true, ColorId = 19, MaterialId = 3,  Description = "Средняя опорная балка кровати", UpdateDate = now }, // 53
                new() { Name = "Ламель буковая 800x63", Length = 800, Width = 63, Height = 8, Diameter = 0, Weight = (decimal?)0.5, Activity = true, ColorId = 20, MaterialId = 2,  Description = "Пружинящая ламель для основания кровати", UpdateDate = now }, // 54
                new() { Name = "Комплект ламелей 800x63 на основу 1600", Length = 800, Width = 63, Height = 8, Diameter = 0, Weight = (decimal?)8.0, Activity = true, ColorId = 20, MaterialId = 2,  Description = "Набор ламелей для двуспального основания", UpdateDate = now }, // 55
                new() { Name = "Изножье кровати 1600x500 дуб медовый", Length = 500, Width = 1600, Height = 25, Diameter = 0, Weight = (decimal?)15.0, Activity = true, ColorId = 13, MaterialId = 1,  Description = "Нижняя планка каркаса кровати", UpdateDate = now }, // 56
                new() { Name = "Накладка мягкая на спинку кровати 1600", Length = 900, Width = 1600, Height = 60, Diameter = 0, Weight = (decimal?)12.0, Activity = true, ColorId = 24, MaterialId = 23, Description = "Мягкая панель с тканевой обивкой", UpdateDate = now }, // 57
                new() { Name = "Борт кровати мягкий 2000x200", Length = 2000, Width = 200, Height = 80, Diameter = 0, Weight = (decimal?)10.0, Activity = true, ColorId = 25, MaterialId = 23, Description = "Обитый тканью боковой борт кровати", UpdateDate = now }, // 58
                new() { Name = "Комплект опор кровати металлических 120 мм", Length = 120, Width = 50, Height = 50, Diameter = 0, Weight = (decimal?)1.8, Activity = true, ColorId = 33, MaterialId = 16, Description = "4 опоры из нержавеющей стали", UpdateDate = now }, // 59
                new() { Name = "Опора центральная регулируемая 200-300 мм", Length = 300, Width = 40, Height = 40, Diameter = 0, Weight = (decimal?)0.7, Activity = true, ColorId = 35, MaterialId = 14, Description = "Центральная регулируемая опора кровати", UpdateDate = now }, // 60

                new() { Name = "Сиденье стула мягкое 420x420 ткань серая", Length = 420, Width = 420, Height = 60, Diameter = 0, Weight = (decimal?)2.5, Activity = true, ColorId = 3,  MaterialId = 23, Description = "Пенополиуретан с обивкой из серой ткани", UpdateDate = now }, // 61
                new() { Name = "Спинка стула мягкая 420x350 ткань серая", Length = 350, Width = 420, Height = 60, Diameter = 0, Weight = (decimal?)2.0, Activity = true, ColorId = 3,  MaterialId = 23, Description = "Мягкая спинка в серой ткани", UpdateDate = now }, // 62
                new() { Name = "Сиденье стула мягкое 420x420 экокожа черная", Length = 420, Width = 420, Height = 60, Diameter = 0, Weight = (decimal?)2.6, Activity = true, ColorId = 5,  MaterialId = 24, Description = "Сиденье стула в черной экокоже", UpdateDate = now }, // 63
                new() { Name = "Спинка стула мягкая 420x350 экокожа черная", Length = 350, Width = 420, Height = 60, Diameter = 0, Weight = (decimal?)2.1, Activity = true, ColorId = 5,  MaterialId = 24, Description = "Спинка стула в черной экокоже", UpdateDate = now }, // 64
                new() { Name = "Подушка дивана 600x600 ткань пудрово-розовая", Length = 600, Width = 600, Height = 180, Diameter = 0, Weight = (decimal?)3.0, Activity = true, ColorId = 32, MaterialId = 23, Description = "Съемная диванная подушка", UpdateDate = now }, // 65
                new() { Name = "Подушка дивана 450x450 терракотовая", Length = 450, Width = 450, Height = 160, Diameter = 0, Weight = (decimal?)2.0, Activity = true, ColorId = 30, MaterialId = 23, Description = "Акцентная подушка для дивана", UpdateDate = now }, // 66
                new() { Name = "Сиденье дивана трехместного 1800x600", Length = 1800, Width = 600, Height = 220, Diameter = 0, Weight = (decimal?)18.0, Activity = true, ColorId = 6,  MaterialId = 23, Description = "Мягкое сиденье дивана под три посадочных места", UpdateDate = now }, // 67
                new() { Name = "Спинка дивана трехместного 1800x500", Length = 1800, Width = 500, Height = 200, Diameter = 0, Weight = (decimal?)15.0, Activity = true, ColorId = 6,  MaterialId = 23, Description = "Мягкая спинка дивана", UpdateDate = now }, // 68
                new() { Name = "Подлокотник дивана мягкий 800x250", Length = 800, Width = 250, Height = 220, Diameter = 0, Weight = (decimal?)6.0, Activity = true, ColorId = 29, MaterialId = 24, Description = "Подлокотник с обивкой из экокожи", UpdateDate = now }, // 69
                new() { Name = "Матрас односпальный 900x2000", Length = 2000, Width = 900, Height = 200, Diameter = 0, Weight = (decimal?)18.0, Activity = true, ColorId = 2,  MaterialId = 26, Description = "Матрас из ППУ в чехле из ткани", UpdateDate = now }, // 70

                new() { Name = "Петля накладная с доводчиком 110°", Length = 110, Width = 50, Height = 30, Diameter = 0, Weight = (decimal?)0.08, Activity = true, ColorId = 33, MaterialId = 14, Description = "Шарнирная петля для фасадов, комплект 2 шт.", UpdateDate = now }, // 71
                new() { Name = "Петля вкладная 110°", Length = 110, Width = 50, Height = 30, Diameter = 0, Weight = (decimal?)0.07, Activity = true, ColorId = 33, MaterialId = 14, Description = "Петля для вкладных фасадов", UpdateDate = now }, // 72
                new() { Name = "Направляющие шариковые 450 мм", Length = 450, Width = 45, Height = 12, Diameter = 0, Weight = (decimal?)0.75, Activity = true, ColorId = 33, MaterialId = 16, Description = "Комплект направляющих полного выдвижения", UpdateDate = now }, // 73
                new() { Name = "Направляющие шариковые 500 мм", Length = 500, Width = 45, Height = 12, Diameter = 0, Weight = (decimal?)0.8, Activity = true, ColorId = 33, MaterialId = 16, Description = "Направляющие для глубоких ящиков", UpdateDate = now }, // 74
                new() { Name = "Направляющие роликовые 400 мм белые", Length = 400, Width = 45, Height = 20, Diameter = 0, Weight = (decimal?)0.5, Activity = true, ColorId = 1,  MaterialId = 14, Description = "Роликовые направляющие для ящиков", UpdateDate = now }, // 75
                new() { Name = "Ручка скоба 128 мм хром", Length = 128, Width = 20, Height = 30, Diameter = 0, Weight = (decimal?)0.05, Activity = true, ColorId = 33, MaterialId = 14, Description = "Металлическая ручка для фасадов", UpdateDate = now }, // 76
                new() { Name = "Ручка скоба 160 мм черный", Length = 160, Width = 20, Height = 30, Diameter = 0, Weight = (decimal?)0.06, Activity = true, ColorId = 35, MaterialId = 14, Description = "Ручка-скоба в черном цвете", UpdateDate = now }, // 77
                new() { Name = "Ручка профиль Gola 3600 мм алюминий", Length = 3600, Width = 25, Height = 40, Diameter = 0, Weight = (decimal?)2.8, Activity = true, ColorId = 34, MaterialId = 15, Description = "Алюминиевый профиль ручки для кухонь без ручек", UpdateDate = now }, // 78
                new() { Name = "Опора полки металлическая хром", Length = 20, Width = 15, Height = 15, Diameter = 0, Weight = (decimal?)0.01, Activity = true, ColorId = 33, MaterialId = 14, Description = "Штифтовый держатель полки, комплект 4 шт.", UpdateDate = now }, // 79
                new() { Name = "Подвес шкафа регулируемый", Length = 80, Width = 40, Height = 30, Diameter = 0, Weight = (decimal?)0.08, Activity = true, ColorId = 33, MaterialId = 14, Description = "Скрытый навес для навесных шкафов", UpdateDate = now }, // 80
                new() { Name = "Уголок мебельный перфорированный 40x40", Length = 40, Width = 40, Height = 20, Diameter = 0, Weight = (decimal?)0.03, Activity = true, ColorId = 33, MaterialId = 14, Description = "Стальной уголок для жесткого крепления", UpdateDate = now }, // 81
                new() { Name = "Конфирмат 7x50 оцинкованный", Length = 50, Width = 7, Height = 7, Diameter = 0, Weight = (decimal?)0.01, Activity = true, ColorId = 33, MaterialId = 14, Description = "Шуруп-конфирмат для сборки корпусов, упаковка 50 шт.", UpdateDate = now }, // 82
                new() { Name = "Стяжка эксцентриковая 15 мм", Length = 35, Width = 15, Height = 15, Diameter = 0, Weight = (decimal?)0.01, Activity = true, ColorId = 33, MaterialId = 14, Description = "Эксцентриковая стяжка для соединения деталей", UpdateDate = now }, // 83
                new() { Name = "Полкодержатель пластиковый с штифтом", Length = 25, Width = 15, Height = 15, Diameter = 0, Weight = (decimal?)0.005, Activity = true, ColorId = 5,  MaterialId = 21, Description = "Пластиковый держатель полки, комплект 4 шт.", UpdateDate = now }, // 84
                new() { Name = "Демпфер дверной врезной", Length = 40, Width = 15, Height = 15, Diameter = 0, Weight = (decimal?)0.01, Activity = true, ColorId = 5,  MaterialId = 21, Description = "Скрытый демпфер для мягкого закрывания", UpdateDate = now }, // 85
                new() { Name = "Ответная планка защелки магнитной", Length = 40, Width = 15, Height = 5, Diameter = 0, Weight = (decimal?)0.01, Activity = true, ColorId = 33, MaterialId = 16, Description = "Металлическая планка под магнитный фиксатор", UpdateDate = now }, // 86
                new() { Name = "Защелка магнитная малая белая", Length = 45, Width = 15, Height = 15, Diameter = 0, Weight = (decimal?)0.02, Activity = true, ColorId = 1,  MaterialId = 21, Description = "Магнитная защелка для легких фасадов", UpdateDate = now }, // 87
                new() { Name = "Опора регулируемая для стола 30 мм", Length = 30, Width = 30, Height = 20, Diameter = 0, Weight = (decimal?)0.02, Activity = true, ColorId = 5,  MaterialId = 20, Description = "Регулируемая опора для выравнивания стола", UpdateDate = now }, // 88
                new() { Name = "Крепеж для панели кровати угловой", Length = 60, Width = 60, Height = 20, Diameter = 0, Weight = (decimal?)0.05, Activity = true, ColorId = 33, MaterialId = 16, Description = "Уголок для соединения царг и спинок", UpdateDate = now }, // 89
                new() { Name = "Комплект стяжек для столешницы", Length = 65, Width = 35, Height = 15, Diameter = 0, Weight = (decimal?)0.2, Activity = true, ColorId = 33, MaterialId = 16, Description = "3 стяжки для стыковки столешниц", UpdateDate = now }, // 90

                new() { Name = "Профиль алюминиевый торцевой 2700 мм", Length = 2700, Width = 30, Height = 10, Diameter = 0, Weight = (decimal?)1.4, Activity = true, ColorId = 34, MaterialId = 15, Description = "Торцевой профиль для столешницы", UpdateDate = now }, // 91
                new() { Name = "Профиль угловой для столешницы 600 мм", Length = 600, Width = 40, Height = 40, Diameter = 0, Weight = (decimal?)0.4, Activity = true, ColorId = 34, MaterialId = 15, Description = "Внутренний алюминиевый уголок", UpdateDate = now }, // 92
                new() { Name = "Профиль светодиодный встраиваемый 2000 мм", Length = 2000, Width = 20, Height = 10, Diameter = 0, Weight = (decimal?)0.7, Activity = true, ColorId = 34, MaterialId = 15, Description = "Алюминиевый профиль под LED ленту", UpdateDate = now }, // 93
                new() { Name = "Планка декоративная 2400x80 венге", Length = 2400, Width = 80, Height = 18, Diameter = 0, Weight = (decimal?)4.0, Activity = true, ColorId = 17, MaterialId = 9,  Description = "Декоративная накладка для шкафов", UpdateDate = now }, // 94
                new() { Name = "Карниз верхний кухонный 2400x60 дуб натуральный", Length = 2400, Width = 60, Height = 25, Diameter = 0, Weight = (decimal?)4.5, Activity = true, ColorId = 10, MaterialId = 4,  Description = "Деревянный карниз для классической кухни", UpdateDate = now }, // 95
                new() { Name = "Плинтус мебельный 2400x60 белый", Length = 2400, Width = 60, Height = 12, Diameter = 0, Weight = (decimal?)1.5, Activity = true, ColorId = 1,  MaterialId = 21, Description = "Пластиковый плинтус с кабель-каналом", UpdateDate = now }, // 96
                new() { Name = "Плинтус мебельный 2400x60 дуб сонома", Length = 2400, Width = 60, Height = 12, Diameter = 0, Weight = (decimal?)1.5, Activity = true, ColorId = 11, MaterialId = 21, Description = "Плинтус под цвет дуб сонома", UpdateDate = now }, // 97
                new() { Name = "Профиль кромочный ПВХ 19 мм белый", Length = 25000, Width = 19, Height = 2, Diameter = 0, Weight = (decimal?)3.0, Activity = true, ColorId = 1,  MaterialId = 22, Description = "Рулон кромочного материала ПВХ белого цвета", UpdateDate = now }, // 98
                new() { Name = "Профиль кромочный ПВХ 19 мм дуб сонома", Length = 25000, Width = 19, Height = 2, Diameter = 0, Weight = (decimal?)3.0, Activity = true, ColorId = 11, MaterialId = 22, Description = "Кромка в декоре дуб сонома", UpdateDate = now }, // 99
                new() { Name = "Профиль кромочный ПВХ 19 мм серый графит", Length = 25000, Width = 19, Height = 2, Diameter = 0, Weight = (decimal?)3.0, Activity = true, ColorId = 4,  MaterialId = 22, Description = "Кромка ПВХ темно-серая для ЛДСП", UpdateDate = now }, // 100
            };

            context.Parts.AddRange(items);
            context.SaveChanges();
        }

        if (!context.Prices.Any())
        {
            var prices = new Price[] {
                new() { PartId = 1, Value = 139.77m, Date = now, UpdateDate = now },
                new() { PartId = 2, Value = 3599.54m, Date = now, UpdateDate = now },
                new() { PartId = 3, Value = 3179.77m, Date = now, UpdateDate = now },
                new() { PartId = 4, Value = 1881.76m, Date = now, UpdateDate = now },
                new() { PartId = 4, Value = 1866.3m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 4, Value = 1855.77m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 4, Value = 1806.74m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 4, Value = 1818.0m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 4, Value = 1795.13m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 5, Value = 2361.0m, Date = now, UpdateDate = now },
                new() { PartId = 5, Value = 2321.12m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 5, Value = 2335.37m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 5, Value = 2282.46m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 5, Value = 2247.33m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 5, Value = 2223.91m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 6, Value = 4389.98m, Date = now, UpdateDate = now },
                new() { PartId = 7, Value = 3414.9m, Date = now, UpdateDate = now },
                new() { PartId = 8, Value = 4228.51m, Date = now, UpdateDate = now },
                new() { PartId = 9, Value = 3918.93m, Date = now, UpdateDate = now },
                new() { PartId = 10, Value = 3144.91m, Date = now, UpdateDate = now },
                new() { PartId = 11, Value = 4892.46m, Date = now, UpdateDate = now },
                new() { PartId = 12, Value = 2514.14m, Date = now, UpdateDate = now },
                new() { PartId = 12, Value = 2491.61m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 12, Value = 2480.42m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 12, Value = 2444.67m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 12, Value = 2431.76m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 12, Value = 2392.32m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 13, Value = 3818.29m, Date = now, UpdateDate = now },
                new() { PartId = 14, Value = 1183.3m, Date = now, UpdateDate = now },
                new() { PartId = 14, Value = 1165.03m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 14, Value = 1154.65m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 14, Value = 1137.85m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 14, Value = 1129.64m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 14, Value = 1114.69m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 15, Value = 2111.89m, Date = now, UpdateDate = now },
                new() { PartId = 15, Value = 2096.51m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 15, Value = 2063.95m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 15, Value = 2043.05m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 15, Value = 2015.15m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 15, Value = 1996.46m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 16, Value = 4746.62m, Date = now, UpdateDate = now },
                new() { PartId = 17, Value = 3592.14m, Date = now, UpdateDate = now },
                new() { PartId = 18, Value = 3436.52m, Date = now, UpdateDate = now },
                new() { PartId = 18, Value = 3379.56m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 18, Value = 3383.54m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 18, Value = 3310.29m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 18, Value = 3290.78m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 18, Value = 3298.34m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 19, Value = 3560.0m, Date = now, UpdateDate = now },
                new() { PartId = 20, Value = 3227.8m, Date = now, UpdateDate = now },
                new() { PartId = 21, Value = 1184.61m, Date = now, UpdateDate = now },
                new() { PartId = 22, Value = 1342.85m, Date = now, UpdateDate = now },
                new() { PartId = 23, Value = 4104.0m, Date = now, UpdateDate = now },
                new() { PartId = 24, Value = 1916.19m, Date = now, UpdateDate = now },
                new() { PartId = 25, Value = 1128.4m, Date = now, UpdateDate = now },
                new() { PartId = 26, Value = 815.45m, Date = now, UpdateDate = now },
                new() { PartId = 27, Value = 767.74m, Date = now, UpdateDate = now },
                new() { PartId = 28, Value = 710.98m, Date = now, UpdateDate = now },
                new() { PartId = 28, Value = 710.17m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 28, Value = 702.12m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 28, Value = 687.02m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 28, Value = 684.75m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 28, Value = 673.95m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 29, Value = 1414.55m, Date = now, UpdateDate = now },
                new() { PartId = 29, Value = 1399.24m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 29, Value = 1379.6m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 29, Value = 1364.94m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 29, Value = 1359.7m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 29, Value = 1337.11m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 30, Value = 1084.59m, Date = now, UpdateDate = now },
                new() { PartId = 30, Value = 1082.37m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 30, Value = 1060.71m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 30, Value = 1045.96m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 30, Value = 1051.99m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 30, Value = 1030.56m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 31, Value = 6363.64m, Date = now, UpdateDate = now },
                new() { PartId = 32, Value = 5706.75m, Date = now, UpdateDate = now },
                new() { PartId = 32, Value = 5605.13m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 32, Value = 5607.16m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 32, Value = 5568.88m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 32, Value = 5469.59m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 32, Value = 5371.59m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 33, Value = 10724.29m, Date = now, UpdateDate = now },
                new() { PartId = 34, Value = 19941.82m, Date = now, UpdateDate = now },
                new() { PartId = 35, Value = 12936.72m, Date = now, UpdateDate = now },
                new() { PartId = 36, Value = 19566.18m, Date = now, UpdateDate = now },
                new() { PartId = 36, Value = 19511.7m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 36, Value = 18983.68m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 36, Value = 19065.56m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 36, Value = 18854.64m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 36, Value = 18602.33m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 37, Value = 9002.38m, Date = now, UpdateDate = now },
                new() { PartId = 38, Value = 14614.43m, Date = now, UpdateDate = now },
                new() { PartId = 39, Value = 6673.28m, Date = now, UpdateDate = now },
                new() { PartId = 40, Value = 11521.48m, Date = now, UpdateDate = now },
                new() { PartId = 41, Value = 2814.89m, Date = now, UpdateDate = now },
                new() { PartId = 42, Value = 4815.26m, Date = now, UpdateDate = now },
                new() { PartId = 43, Value = 4503.41m, Date = now, UpdateDate = now },
                new() { PartId = 44, Value = 2053.56m, Date = now, UpdateDate = now },
                new() { PartId = 45, Value = 3002.34m, Date = now, UpdateDate = now },
                new() { PartId = 46, Value = 7679.78m, Date = now, UpdateDate = now },
                new() { PartId = 47, Value = 465.05m, Date = now, UpdateDate = now },
                new() { PartId = 48, Value = 274.1m, Date = now, UpdateDate = now },
                new() { PartId = 49, Value = 219.38m, Date = now, UpdateDate = now },
                new() { PartId = 50, Value = 355.58m, Date = now, UpdateDate = now },
                new() { PartId = 51, Value = 221.79m, Date = now, UpdateDate = now },
                new() { PartId = 52, Value = 130.57m, Date = now, UpdateDate = now },
                new() { PartId = 53, Value = 252.5m, Date = now, UpdateDate = now },
                new() { PartId = 54, Value = 207.88m, Date = now, UpdateDate = now },
                new() { PartId = 55, Value = 255.73m, Date = now, UpdateDate = now },
                new() { PartId = 55, Value = 253.32m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 55, Value = 248.06m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 55, Value = 247.15m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 55, Value = 243.04m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 55, Value = 245.13m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 56, Value = 275.74m, Date = now, UpdateDate = now },
                new() { PartId = 57, Value = 266.33m, Date = now, UpdateDate = now },
                new() { PartId = 58, Value = 161.5m, Date = now, UpdateDate = now },
                new() { PartId = 59, Value = 111.59m, Date = now, UpdateDate = now },
                new() { PartId = 60, Value = 451.2m, Date = now, UpdateDate = now },
                new() { PartId = 61, Value = 4787.8m, Date = now, UpdateDate = now },
                new() { PartId = 62, Value = 1342.61m, Date = now, UpdateDate = now },
                new() { PartId = 63, Value = 2943.96m, Date = now, UpdateDate = now },
                new() { PartId = 64, Value = 1276.85m, Date = now, UpdateDate = now },
                new() { PartId = 65, Value = 16409.03m, Date = now, UpdateDate = now },
                new() { PartId = 65, Value = 16332.18m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 65, Value = 15958.9m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 65, Value = 15908.65m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 65, Value = 15769.02m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 65, Value = 15511.48m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 66, Value = 18086.5m, Date = now, UpdateDate = now },
                new() { PartId = 67, Value = 11347.07m, Date = now, UpdateDate = now },
                new() { PartId = 68, Value = 8176.97m, Date = now, UpdateDate = now },
                new() { PartId = 69, Value = 13089.44m, Date = now, UpdateDate = now },
                new() { PartId = 70, Value = 15948.97m, Date = now, UpdateDate = now },
                new() { PartId = 70, Value = 15694.15m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 70, Value = 15569.93m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 70, Value = 15628.44m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 70, Value = 15358.82m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 70, Value = 15131.77m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 71, Value = 307.03m, Date = now, UpdateDate = now },
                new() { PartId = 72, Value = 148.4m, Date = now, UpdateDate = now },
                new() { PartId = 72, Value = 146.1m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 72, Value = 144.95m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 72, Value = 144.21m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 72, Value = 141.66m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 72, Value = 140.15m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 73, Value = 128.4m, Date = now, UpdateDate = now },
                new() { PartId = 74, Value = 352.44m, Date = now, UpdateDate = now },
                new() { PartId = 75, Value = 191.58m, Date = now, UpdateDate = now },
                new() { PartId = 76, Value = 462.17m, Date = now, UpdateDate = now },
                new() { PartId = 76, Value = 460.87m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 76, Value = 448.96m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 76, Value = 445.88m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 76, Value = 445.24m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 76, Value = 436.42m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 77, Value = 152.92m, Date = now, UpdateDate = now },
                new() { PartId = 78, Value = 474.21m, Date = now, UpdateDate = now },
                new() { PartId = 78, Value = 470.14m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 78, Value = 464.46m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 78, Value = 462.68m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 78, Value = 458.15m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 78, Value = 447.56m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 79, Value = 138.77m, Date = now, UpdateDate = now },
                new() { PartId = 80, Value = 186.21m, Date = now, UpdateDate = now },
                new() { PartId = 81, Value = 24.06m, Date = now, UpdateDate = now },
                new() { PartId = 82, Value = 26.02m, Date = now, UpdateDate = now },
                new() { PartId = 82, Value = 25.88m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 82, Value = 25.59m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 82, Value = 25.49m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 82, Value = 24.77m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 82, Value = 24.66m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 83, Value = 20.27m, Date = now, UpdateDate = now },
                new() { PartId = 84, Value = 43.78m, Date = now, UpdateDate = now },
                new() { PartId = 85, Value = 16.19m, Date = now, UpdateDate = now },
                new() { PartId = 86, Value = 138.04m, Date = now, UpdateDate = now },
                new() { PartId = 87, Value = 25.19m, Date = now, UpdateDate = now },
                new() { PartId = 87, Value = 24.9m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 87, Value = 24.57m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 87, Value = 24.31m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 87, Value = 24.39m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 87, Value = 23.9m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 88, Value = 4445.4m, Date = now, UpdateDate = now },
                new() { PartId = 89, Value = 210.07m, Date = now, UpdateDate = now },
                new() { PartId = 90, Value = 1202.35m, Date = now, UpdateDate = now },
                new() { PartId = 91, Value = 299.86m, Date = now, UpdateDate = now },
                new() { PartId = 92, Value = 4344.11m, Date = now, UpdateDate = now },
                new() { PartId = 93, Value = 293.8m, Date = now, UpdateDate = now },
                new() { PartId = 94, Value = 285.27m, Date = now, UpdateDate = now },
                new() { PartId = 95, Value = 269.74m, Date = now, UpdateDate = now },
                new() { PartId = 95, Value = 265.24m, Date = now.AddMonths(-1), UpdateDate = now },
                new() { PartId = 95, Value = 264.27m, Date = now.AddMonths(-2), UpdateDate = now },
                new() { PartId = 95, Value = 260.1m, Date = now.AddMonths(-3), UpdateDate = now },
                new() { PartId = 95, Value = 258.42m, Date = now.AddMonths(-4), UpdateDate = now },
                new() { PartId = 95, Value = 253.87m, Date = now.AddMonths(-5), UpdateDate = now },
                new() { PartId = 96, Value = 175.79m, Date = now, UpdateDate = now },
                new() { PartId = 97, Value = 297.06m, Date = now, UpdateDate = now },
                new() { PartId = 98, Value = 153.04m, Date = now, UpdateDate = now },
                new() { PartId = 99, Value = 256.81m, Date = now, UpdateDate = now },
                new() { PartId = 100, Value = 191.0m, Date = now, UpdateDate = now }
            };

            context.Prices.AddRange(prices);
            context.SaveChanges();
        }

        if (!context.Furniture.AsNoTracking().Any())
        {

            var furnitureItems = new Furniture[]
            {
                new() { Name = "Шкаф навесной 400мм Белый", Description = "Верхний кухонный модуль с одной дверцей", Markup = 30, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Шкаф навесной 600мм Белый", Description = "Широкий верхний модуль", Markup = 30, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Шкаф навесной 400мм Дуб Сонома", Description = "Верхний модуль, декор дерево", Markup = 35, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Шкаф навесной 600мм Дуб Сонома", Description = "Широкий модуль, декор дерево", Markup = 35, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Шкаф навесной 400мм Бетон", Description = "Модуль в стиле лофт", Markup = 40, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Шкаф навесной 600мм Бетон", Description = "Широкий модуль лофт", Markup = 40, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Шкаф-витрина 400мм Белый", Description = "Модуль со стеклянным фасадом", Markup = 45, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Шкаф-витрина 600мм Белый", Description = "Широкая витрина", Markup = 45, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Шкаф навесной угловой Белый", Description = "Угловой модуль кухни", Markup = 35, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Пенал кухонный Дуб Натуральный", Description = "Высокий шкаф для хранения", Markup = 40, Activity = true, CategoryId = 15, UpdateDate = now },

                // --- 2. Кухня (Нижние шкафы) ID 11-20 ---
                new() { Name = "Стол рабочий 400мм Белый", Description = "Нижний модуль с полкой", Markup = 30, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Стол под мойку 600мм Белый", Description = "Тумба без задней стенки", Markup = 30, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Тумба с ящиками 400мм Белая", Description = "3 выдвижных ящика", Markup = 45, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Тумба с ящиками 600мм Белая", Description = "3 широких ящика", Markup = 45, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Тумба с глубокими ящиками 600мм Сонома", Description = "2 глубоких ящика", Markup = 50, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Тумба нижняя 600мм Графит", Description = "Модуль с ящиком и дверцей", Markup = 40, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Тумба нижняя 450мм Дуб Серый", Description = "Узкий модуль", Markup = 42, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Шкаф хозяйственный 600мм Белый", Description = "Высокий шкаф для инвентаря", Markup = 35, Activity = true, CategoryId = 7, UpdateDate = now },
                new() { Name = "Шкаф продуктовый 600мм Дуб Дымчатый", Description = "Высокий шкаф с полками", Markup = 45, Activity = true, CategoryId = 7, UpdateDate = now },
                new() { Name = "Бутылочница 200мм", Description = "Выдвижная корзина карго", Markup = 50, Activity = true, CategoryId = 15, UpdateDate = now },

                // --- 3. Столы ID 21-30 ---
                new() { Name = "Стол обеденный 'Лофт' Дуб", Description = "Массив дуба, черные ножки", Markup = 60, Activity = true, CategoryId = 1, UpdateDate = now },
                new() { Name = "Стол обеденный 'Сканди' Ясень", Description = "Светлый ясень, конусные ножки", Markup = 55, Activity = true, CategoryId = 1, UpdateDate = now },
                new() { Name = "Стол круглый 'Бистро'", Description = "Белая столешница, хромированная опора", Markup = 40, Activity = true, CategoryId = 1, UpdateDate = now },
                new() { Name = "Стол журнальный 'Гласс'", Description = "Стекло и металл", Markup = 50, Activity = true, CategoryId = 10, UpdateDate = now },
                new() { Name = "Столик кофейный 'Прованс'", Description = "Выбеленный дуб", Markup = 45, Activity = true, CategoryId = 10, UpdateDate = now },
                new() { Name = "Стойка барная 'Тик'", Description = "Натуральный тик", Markup = 70, Activity = true, CategoryId = 1, UpdateDate = now },
                new() { Name = "Стол письменный 'Офис-Про'", Description = "Строгий стиль, графит", Markup = 35, Activity = true, CategoryId = 11, UpdateDate = now },
                new() { Name = "Стол рабочий 'Домашний'", Description = "Удобный стол, сонома", Markup = 30, Activity = true, CategoryId = 11, UpdateDate = now },
                new() { Name = "Стол переговоров 'Босс'", Description = "Большой стол из двух столешниц", Markup = 80, Activity = true, CategoryId = 11, UpdateDate = now },
                new() { Name = "Стол геймерский 'Кибер'", Description = "Черный с подсветкой", Markup = 60, Activity = true, CategoryId = 11, UpdateDate = now },

                // --- 4. Мягкая мебель и стулья ID 31-40 ---
                new() { Name = "Стул мягкий 'Комфорт' Серый", Description = "Тканевая обивка", Markup = 35, Activity = true, CategoryId = 2, UpdateDate = now },
                new() { Name = "Стул кухонный 'Престиж'", Description = "Черная экокожа", Markup = 40, Activity = true, CategoryId = 2, UpdateDate = now },
                new() { Name = "Стул барный 'Лофт'", Description = "Высокий стул", Markup = 45, Activity = true, CategoryId = 2, UpdateDate = now },
                new() { Name = "Диван трехместный 'Фемили'", Description = "Серая рогожка", Markup = 40, Activity = true, CategoryId = 3, UpdateDate = now },
                new() { Name = "Диван 'Модерн' Терракот", Description = "Яркий акцент", Markup = 50, Activity = true, CategoryId = 3, UpdateDate = now },
                new() { Name = "Кресло 'Релакс'", Description = "Мягкое кресло", Markup = 45, Activity = true, CategoryId = 4, UpdateDate = now },
                new() { Name = "Кресло офисное 'Менеджер'", Description = "На колесиках", Markup = 40, Activity = true, CategoryId = 12, UpdateDate = now },
                new() { Name = "Пуф 'Зефир'", Description = "Пудровый розовый", Markup = 30, Activity = true, CategoryId = 4, UpdateDate = now },
                new() { Name = "Банкетка в прихожую", Description = "С полкой для обуви", Markup = 35, Activity = true, CategoryId = 14, UpdateDate = now },
                new() { Name = "Диван уличный 'Терраса'", Description = "Влагостойкий", Markup = 55, Activity = true, CategoryId = 16, UpdateDate = now },

                // --- 5. Кровати и спальня ID 41-50 ---
                new() { Name = "Кровать двуспальная 'Кантри'", Description = "Массив дуба медового цвета", Markup = 40, Activity = true, CategoryId = 5, UpdateDate = now },
                new() { Name = "Кровать с мягким изголовьем 'Софт'", Description = "Тканевая обивка", Markup = 45, Activity = true, CategoryId = 5, UpdateDate = now },
                new() { Name = "Кровать односпальная 'Дачная'", Description = "Массив сосны", Markup = 30, Activity = true, CategoryId = 5, UpdateDate = now },
                new() { Name = "Кровать с ящиками 'Практик'", Description = "Дополнительное хранение", Markup = 50, Activity = true, CategoryId = 5, UpdateDate = now },
                new() { Name = "Матрас 'Стандарт' 90x200", Description = "Беспружинный", Markup = 20, Activity = true, CategoryId = 6, UpdateDate = now },
                new() { Name = "Матрас 'Кинг' 180x200", Description = "Двуспальный", Markup = 25, Activity = true, CategoryId = 6, UpdateDate = now },
                new() { Name = "Тумба прикроватная 'Дуб'", Description = "Два ящика", Markup = 35, Activity = true, CategoryId = 8, UpdateDate = now },
                new() { Name = "Тумба прикроватная 'Белая'", Description = "Лаконичный стиль", Markup = 35, Activity = true, CategoryId = 8, UpdateDate = now },
                new() { Name = "Шкаф платяной 2-х дверный", Description = "Штанга и полка", Markup = 30, Activity = true, CategoryId = 7, UpdateDate = now },
                new() { Name = "Комод бельевой 'Классик'", Description = "4 ящика", Markup = 35, Activity = true, CategoryId = 8, UpdateDate = now },

                // --- 6. Кухня (Специальные модули) ID 51-60 ---
                new() { Name = "Шкаф горизонтальный 600мм Белый", Description = "С подъемным механизмом", Markup = 35, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Шкаф горизонтальный 800мм Белый", Description = "Широкий модуль с подъемником", Markup = 35, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Шкаф горизонтальный 600мм Сонома", Description = "Фасад дерево, подъемник", Markup = 35, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Шкаф под вытяжку 600мм Белый", Description = "Укороченный модуль", Markup = 30, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Шкаф под встраиваемую СВЧ", Description = "Ниша для микроволновки", Markup = 40, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Пенал под духовой шкаф", Description = "Ниша 600мм для духовки", Markup = 45, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Фасад на посудомойку 450мм", Description = "Панель для ПММ 45см", Markup = 20, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Фасад на посудомойку 600мм", Description = "Панель для ПММ 60см", Markup = 20, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Угловое окончание кухни (Верх)", Description = "Открытые радиусные полки", Markup = 30, Activity = true, CategoryId = 15, UpdateDate = now },
                new() { Name = "Угловое окончание кухни (Низ)", Description = "Открытые полки нижние", Markup = 30, Activity = true, CategoryId = 15, UpdateDate = now },

                // --- 7. Хранение и Офис ID 61-70 ---
                new() { Name = "Стеллаж 'Лофт' 1800x800", Description = "Металл и дерево", Markup = 40, Activity = true, CategoryId = 9, UpdateDate = now },
                new() { Name = "Стеллаж для документов узкий", Description = "Для папок, ширина 400мм", Markup = 30, Activity = true, CategoryId = 11, UpdateDate = now },
                new() { Name = "Тумба офисная на колесах Белая", Description = "3 ящика, мобильная", Markup = 25, Activity = true, CategoryId = 11, UpdateDate = now },
                new() { Name = "Тумба офисная на колесах Графит", Description = "3 ящика, мобильная", Markup = 25, Activity = true, CategoryId = 11, UpdateDate = now },
                new() { Name = "Тумба приставная к столу", Description = "Стационарная тумба 4 ящика", Markup = 30, Activity = true, CategoryId = 11, UpdateDate = now },
                new() { Name = "Полка-куб настенная", Description = "Квадратная полка", Markup = 50, Activity = true, CategoryId = 9, UpdateDate = now },
                new() { Name = "Комплект полок 'Соты'", Description = "Декоративные полки 3 шт", Markup = 50, Activity = true, CategoryId = 9, UpdateDate = now },
                new() { Name = "Полка для книг навесная 900мм", Description = "Длинная книжная полка", Markup = 35, Activity = true, CategoryId = 9, UpdateDate = now },
                new() { Name = "Шкаф-витрина 'Библиотека'", Description = "Для книг со стеклом", Markup = 45, Activity = true, CategoryId = 9, UpdateDate = now },
                new() { Name = "Стеллаж-перегородка ступенчатый", Description = "Для зонирования комнаты", Markup = 40, Activity = true, CategoryId = 9, UpdateDate = now },

                // --- 8. Разное (Прихожая, Детская, Ванная) ID 71-80 ---
                new() { Name = "Вешалка напольная 'Дерево'", Description = "Стойка для одежды", Markup = 40, Activity = true, CategoryId = 14, UpdateDate = now },
                new() { Name = "Обувница с сиденьем", Description = "Тумба для обуви мягкая", Markup = 35, Activity = true, CategoryId = 14, UpdateDate = now },
                new() { Name = "Шкаф для прихожей узкий", Description = "С зеркалом", Markup = 35, Activity = true, CategoryId = 14, UpdateDate = now },
                new() { Name = "Детский столик 'Творчество'", Description = "Низкий стол для рисования", Markup = 30, Activity = true, CategoryId = 13, UpdateDate = now },
                new() { Name = "Стульчик детский деревянный", Description = "Устойчивый стул", Markup = 30, Activity = true, CategoryId = 13, UpdateDate = now },
                new() { Name = "Ящик для игрушек на колесах", Description = "Большой ящик", Markup = 35, Activity = true, CategoryId = 13, UpdateDate = now },
                new() { Name = "Тумба под раковину 600мм", Description = "Влагостойкая для ванной", Markup = 45, Activity = true, CategoryId = 8, UpdateDate = now },
                new() { Name = "Зеркало в раме 'Дуб'", Description = "Настенное зеркало", Markup = 50, Activity = true, CategoryId = 14, UpdateDate = now },
                new() { Name = "Комод пеленальный", Description = "С откидной доской", Markup = 35, Activity = true, CategoryId = 13, UpdateDate = now },
                new() { Name = "Кровать-чердак", Description = "Спальное место наверху", Markup = 45, Activity = true, CategoryId = 13, UpdateDate = now },

                // --- 9. Дополнительные столы и тумбы ID 81-90 ---
                new() { Name = "Стол консольный 'Лофт'", Description = "Узкий пристенный стол", Markup = 50, Activity = true, CategoryId = 10, UpdateDate = now },
                new() { Name = "Стол придиванный С-образный", Description = "Задвигается под диван", Markup = 50, Activity = true, CategoryId = 10, UpdateDate = now },
                new() { Name = "ТВ-тумба длинная Белая", Description = "Тумба под телевизор 1800мм", Markup = 35, Activity = true, CategoryId = 8, UpdateDate = now },
                new() { Name = "ТВ-тумба 'Бетон'", Description = "Стильная тумба лофт", Markup = 40, Activity = true, CategoryId = 8, UpdateDate = now },
                new() { Name = "Стол кухонный раздвижной", Description = "Увеличивается до 1600мм", Markup = 45, Activity = true, CategoryId = 1, UpdateDate = now },
                new() { Name = "Табурет кухонный 'Эконом'", Description = "Простой табурет", Markup = 20, Activity = true, CategoryId = 2, UpdateDate = now },
                new() { Name = "Табурет-стремянка", Description = "Деревянная лесенка", Markup = 35, Activity = true, CategoryId = 2, UpdateDate = now },
                new() { Name = "Сервант для посуды", Description = "Классический буфет", Markup = 45, Activity = true, CategoryId = 7, UpdateDate = now },
                new() { Name = "Полка для обуви металлическая", Description = "2 яруса", Markup = 30, Activity = true, CategoryId = 14, UpdateDate = now },
                new() { Name = "Комод высокий узкий (5 ящиков)", Description = "Лингерье", Markup = 40, Activity = true, CategoryId = 8, UpdateDate = now },

                // --- 10. Декор и комплектующие ID 91-100 ---
                new() { Name = "Панель настенная декоративная", Description = "Рейки деревянные", Markup = 60, Activity = true, CategoryId = 14, UpdateDate = now },
                new() { Name = "Полка угловая 3 яруса", Description = "Для икон или декора", Markup = 40, Activity = true, CategoryId = 9, UpdateDate = now },
                new() { Name = "Стеллаж для цветов", Description = "Лесенка для растений", Markup = 35, Activity = true, CategoryId = 16, UpdateDate = now },
                new() { Name = "Сканья садовая деревянная", Description = "Для улицы", Markup = 30, Activity = true, CategoryId = 16, UpdateDate = now },
                new() { Name = "Стол складной туристический", Description = "Алюминиевый каркас", Markup = 25, Activity = true, CategoryId = 16, UpdateDate = now },
                new() { Name = "Кресло-мешок 'Груша'", Description = "Бескаркасное кресло", Markup = 35, Activity = true, CategoryId = 4, UpdateDate = now },
                new() { Name = "Подушка на стул мягкая", Description = "Текстильная сидушка", Markup = 50, Activity = true, CategoryId = 6, UpdateDate = now },
                new() { Name = "Экран для батареи декоративный", Description = "МДФ решетка", Markup = 40, Activity = true, CategoryId = 14, UpdateDate = now },
                new() { Name = "Полка невидимая для книг", Description = "Металл скрытый", Markup = 55, Activity = true, CategoryId = 9, UpdateDate = now },
                new() { Name = "Органайзер настольный деревянный", Description = "Для канцелярии", Markup = 60, Activity = true, CategoryId = 11, UpdateDate = now },
            };

            context.Furniture.AddRange(furnitureItems);
            context.SaveChanges();
        }

        if (!context.Set<FurnitureComposition>().AsNoTracking().Any())
        {
            var items = new List<FurnitureComposition>();

            void AddFurnComp(int furnitureId, int partId, int count) =>
                items.Add(new FurnitureComposition
                {
                    Entity1Id = furnitureId,
                    Entity2Id = partId,
                    Count = count,
                    UpdateDate = now
                });

            // --- 1. Кухонные шкафы (ID 1-10) ---
            AddFurnComp(1, 21, 2); AddFurnComp(1, 23, 2); AddFurnComp(1, 26, 1); AddFurnComp(1, 2, 1); AddFurnComp(1, 71, 2); AddFurnComp(1, 76, 1); AddFurnComp(1, 80, 2); AddFurnComp(1, 82, 8);
            AddFurnComp(2, 21, 2); AddFurnComp(2, 23, 2); AddFurnComp(2, 26, 1); AddFurnComp(2, 3, 1); AddFurnComp(2, 71, 2); AddFurnComp(2, 76, 1); AddFurnComp(2, 80, 2); AddFurnComp(2, 82, 8);
            AddFurnComp(3, 22, 2); AddFurnComp(3, 24, 1); AddFurnComp(3, 26, 1); AddFurnComp(3, 4, 1); AddFurnComp(3, 71, 2); AddFurnComp(3, 76, 1); AddFurnComp(3, 80, 2); AddFurnComp(3, 82, 8);
            AddFurnComp(4, 22, 2); AddFurnComp(4, 24, 1); AddFurnComp(4, 26, 1); AddFurnComp(4, 5, 1); AddFurnComp(4, 71, 2); AddFurnComp(4, 76, 1); AddFurnComp(4, 80, 2); AddFurnComp(4, 82, 8);
            AddFurnComp(5, 21, 2); AddFurnComp(5, 23, 2); AddFurnComp(5, 26, 1); AddFurnComp(5, 6, 1); AddFurnComp(5, 71, 2); AddFurnComp(5, 77, 1); AddFurnComp(5, 80, 2); AddFurnComp(5, 82, 8);
            AddFurnComp(6, 21, 2); AddFurnComp(6, 23, 2); AddFurnComp(6, 26, 1); AddFurnComp(6, 7, 1); AddFurnComp(6, 71, 2); AddFurnComp(6, 77, 1); AddFurnComp(6, 80, 2); AddFurnComp(6, 82, 8);
            AddFurnComp(7, 21, 2); AddFurnComp(7, 25, 2); AddFurnComp(7, 26, 1); AddFurnComp(7, 8, 1); AddFurnComp(7, 71, 2); AddFurnComp(7, 76, 1); AddFurnComp(7, 80, 2); AddFurnComp(7, 84, 8);
            AddFurnComp(8, 21, 2); AddFurnComp(8, 25, 2); AddFurnComp(8, 26, 1); AddFurnComp(8, 9, 1); AddFurnComp(8, 71, 2); AddFurnComp(8, 76, 1); AddFurnComp(8, 80, 2); AddFurnComp(8, 84, 8);
            AddFurnComp(9, 21, 3); AddFurnComp(9, 23, 1); AddFurnComp(9, 26, 2); AddFurnComp(9, 1, 1); AddFurnComp(9, 71, 2); AddFurnComp(9, 76, 1); AddFurnComp(9, 80, 2); AddFurnComp(9, 82, 12);
            AddFurnComp(10, 21, 4); AddFurnComp(10, 23, 5); AddFurnComp(10, 27, 1); AddFurnComp(10, 10, 1); AddFurnComp(10, 71, 4); AddFurnComp(10, 76, 1); AddFurnComp(10, 47, 4); AddFurnComp(10, 82, 16);

            // --- 2. Нижние шкафы (ID 11-20) ---
            AddFurnComp(11, 21, 2); AddFurnComp(11, 23, 1); AddFurnComp(11, 11, 1); AddFurnComp(11, 47, 4); AddFurnComp(11, 29, 1); AddFurnComp(11, 71, 2); AddFurnComp(11, 76, 1); AddFurnComp(11, 82, 8);
            AddFurnComp(12, 21, 2); AddFurnComp(12, 12, 1); AddFurnComp(12, 47, 4); AddFurnComp(12, 29, 1); AddFurnComp(12, 71, 2); AddFurnComp(12, 76, 1); AddFurnComp(12, 82, 6);
            AddFurnComp(13, 21, 2); AddFurnComp(13, 13, 3); AddFurnComp(13, 28, 3); AddFurnComp(13, 75, 3); AddFurnComp(13, 47, 4); AddFurnComp(13, 29, 1); AddFurnComp(13, 76, 3); AddFurnComp(13, 82, 10);
            AddFurnComp(14, 21, 2); AddFurnComp(14, 14, 3); AddFurnComp(14, 28, 3); AddFurnComp(14, 73, 3); AddFurnComp(14, 47, 4); AddFurnComp(14, 29, 1); AddFurnComp(14, 76, 3); AddFurnComp(14, 82, 10);
            AddFurnComp(15, 22, 2); AddFurnComp(15, 15, 2); AddFurnComp(15, 28, 2); AddFurnComp(15, 74, 2); AddFurnComp(15, 47, 4); AddFurnComp(15, 99, 1); AddFurnComp(15, 76, 2); AddFurnComp(15, 82, 10);
            AddFurnComp(16, 21, 2); AddFurnComp(16, 16, 1); AddFurnComp(16, 12, 1); AddFurnComp(16, 47, 4); AddFurnComp(16, 30, 1); AddFurnComp(16, 77, 2); AddFurnComp(16, 82, 10);
            AddFurnComp(17, 21, 2); AddFurnComp(17, 17, 1); AddFurnComp(17, 18, 1); AddFurnComp(17, 47, 4); AddFurnComp(17, 30, 1); AddFurnComp(17, 77, 2); AddFurnComp(17, 82, 10);
            AddFurnComp(18, 21, 4); AddFurnComp(18, 23, 4); AddFurnComp(18, 27, 1); AddFurnComp(18, 19, 2); AddFurnComp(18, 47, 4); AddFurnComp(18, 71, 4); AddFurnComp(18, 76, 2);
            AddFurnComp(19, 21, 4); AddFurnComp(19, 23, 6); AddFurnComp(19, 27, 1); AddFurnComp(19, 20, 2); AddFurnComp(19, 47, 4); AddFurnComp(19, 71, 4); AddFurnComp(19, 77, 2);
            AddFurnComp(20, 21, 2); AddFurnComp(20, 13, 1); AddFurnComp(20, 73, 1); AddFurnComp(20, 47, 4); AddFurnComp(20, 76, 1);

            // --- 3. Столы (ID 21-30) ---
            AddFurnComp(21, 34, 1); AddFurnComp(21, 43, 2); AddFurnComp(21, 90, 1); AddFurnComp(21, 88, 4);
            AddFurnComp(22, 35, 1); AddFurnComp(22, 45, 1); AddFurnComp(22, 90, 1);
            AddFurnComp(23, 38, 1); AddFurnComp(23, 41, 1); AddFurnComp(23, 88, 1);
            AddFurnComp(24, 36, 1); AddFurnComp(24, 42, 3); AddFurnComp(24, 79, 3);
            AddFurnComp(25, 38, 1); AddFurnComp(25, 45, 1);
            AddFurnComp(26, 37, 1); AddFurnComp(26, 43, 2); AddFurnComp(26, 88, 4);
            AddFurnComp(27, 39, 1); AddFurnComp(27, 44, 2); AddFurnComp(27, 88, 4);
            AddFurnComp(28, 40, 1); AddFurnComp(28, 42, 4); AddFurnComp(28, 88, 4);
            AddFurnComp(29, 32, 2); AddFurnComp(29, 44, 4); AddFurnComp(29, 90, 2); AddFurnComp(29, 91, 2);
            AddFurnComp(30, 39, 1); AddFurnComp(30, 43, 2); AddFurnComp(30, 93, 1); AddFurnComp(30, 100, 1);

            // --- 4. Мягкая (ID 31-40) ---
            AddFurnComp(31, 61, 1); AddFurnComp(31, 62, 1); AddFurnComp(31, 45, 1); AddFurnComp(31, 82, 4);
            AddFurnComp(32, 63, 1); AddFurnComp(32, 64, 1); AddFurnComp(32, 41, 4); AddFurnComp(32, 82, 4);
            AddFurnComp(33, 61, 1); AddFurnComp(33, 43, 1);
            AddFurnComp(34, 67, 1); AddFurnComp(34, 68, 1); AddFurnComp(34, 69, 2); AddFurnComp(34, 46, 4); AddFurnComp(34, 65, 2);
            AddFurnComp(35, 67, 1); AddFurnComp(35, 68, 1); AddFurnComp(35, 69, 2); AddFurnComp(35, 46, 4); AddFurnComp(35, 66, 2);
            AddFurnComp(36, 61, 2); AddFurnComp(36, 62, 1); AddFurnComp(36, 69, 2); AddFurnComp(36, 46, 4);
            AddFurnComp(37, 63, 1); AddFurnComp(37, 64, 1); AddFurnComp(37, 48, 1); AddFurnComp(37, 82, 4);
            AddFurnComp(38, 61, 1); AddFurnComp(38, 65, 1); AddFurnComp(38, 46, 4);
            AddFurnComp(39, 63, 1); AddFurnComp(39, 49, 4); AddFurnComp(39, 91, 1);
            AddFurnComp(40, 67, 1); AddFurnComp(40, 68, 1); AddFurnComp(40, 43, 2);

            // --- 5. Кровати (ID 41-50) ---
            AddFurnComp(41, 51, 1); AddFurnComp(41, 56, 1); AddFurnComp(41, 52, 2); AddFurnComp(41, 53, 1); AddFurnComp(41, 55, 1); AddFurnComp(41, 59, 1); AddFurnComp(41, 60, 2); AddFurnComp(41, 89, 4);
            AddFurnComp(42, 57, 1); AddFurnComp(42, 58, 2); AddFurnComp(42, 53, 1); AddFurnComp(42, 55, 1); AddFurnComp(42, 59, 1); AddFurnComp(42, 89, 4);
            AddFurnComp(43, 52, 2); AddFurnComp(43, 53, 1); AddFurnComp(43, 54, 12); AddFurnComp(43, 49, 4); AddFurnComp(43, 82, 12);
            AddFurnComp(44, 51, 1); AddFurnComp(44, 52, 2); AddFurnComp(44, 14, 2); AddFurnComp(44, 73, 2); AddFurnComp(44, 55, 1);
            AddFurnComp(45, 70, 1);
            AddFurnComp(46, 70, 2);
            AddFurnComp(47, 21, 2); AddFurnComp(47, 13, 2); AddFurnComp(47, 28, 2); AddFurnComp(47, 75, 2); AddFurnComp(47, 49, 4); AddFurnComp(47, 39, 1);
            AddFurnComp(48, 21, 2); AddFurnComp(48, 13, 2); AddFurnComp(48, 28, 2); AddFurnComp(48, 75, 2); AddFurnComp(48, 47, 4); AddFurnComp(48, 23, 1);
            AddFurnComp(49, 21, 2); AddFurnComp(49, 27, 1); AddFurnComp(49, 10, 2); AddFurnComp(49, 23, 2); AddFurnComp(49, 71, 4); AddFurnComp(49, 76, 2); AddFurnComp(49, 94, 1);
            AddFurnComp(50, 21, 2); AddFurnComp(50, 26, 1); AddFurnComp(50, 14, 4); AddFurnComp(50, 28, 4); AddFurnComp(50, 73, 4); AddFurnComp(50, 47, 4); AddFurnComp(50, 23, 1);

            // --- 6. Кухня Спецмодули (ID 51-60) ---
            // 51. Горизонтальный 600 Белый (Используем 15 фасад ящика как горизонт)
            AddFurnComp(51, 21, 2); AddFurnComp(51, 23, 2); AddFurnComp(51, 26, 1); AddFurnComp(51, 15, 1); AddFurnComp(51, 80, 2); AddFurnComp(51, 71, 2);
            // 52. Горизонтальный 800 (Аналог)
            AddFurnComp(52, 21, 2); AddFurnComp(52, 24, 2); AddFurnComp(52, 26, 1); AddFurnComp(52, 15, 1); AddFurnComp(52, 80, 2);
            // 53. Горизонтальный Сонома
            AddFurnComp(53, 22, 2); AddFurnComp(53, 23, 2); AddFurnComp(53, 26, 1); AddFurnComp(53, 15, 1); AddFurnComp(53, 80, 2);
            // 54. Вытяжка (Короткие стенки)
            AddFurnComp(54, 21, 2); AddFurnComp(54, 23, 1); AddFurnComp(54, 13, 1); AddFurnComp(54, 80, 2);
            // 55. Под СВЧ (Ниша)
            AddFurnComp(55, 21, 2); AddFurnComp(55, 23, 3); AddFurnComp(55, 80, 2);
            // 56. Пенал под духовку (Высокие бока, полки усиленные)
            AddFurnComp(56, 21, 4); AddFurnComp(56, 23, 4); AddFurnComp(56, 47, 4); AddFurnComp(56, 14, 1);
            // 57. Фасад ПММ 45 (Только фасад)
            AddFurnComp(57, 18, 1); AddFurnComp(57, 76, 1);
            // 58. Фасад ПММ 60
            AddFurnComp(58, 12, 1); AddFurnComp(58, 76, 1);
            // 59. Угол верх (Полки радиусные - прокси полка 23)
            AddFurnComp(59, 21, 2); AddFurnComp(59, 23, 3); AddFurnComp(59, 80, 2);
            // 60. Угол низ
            AddFurnComp(60, 21, 2); AddFurnComp(60, 23, 3); AddFurnComp(60, 47, 4);

            // --- 7. Стеллажи и Офис (ID 61-70) ---
            // 61. Стеллаж Лофт (Металл ноги + полки)
            AddFurnComp(61, 44, 4); AddFurnComp(61, 23, 5); AddFurnComp(61, 82, 10);
            // 62. Узкий стеллаж
            AddFurnComp(62, 21, 2); AddFurnComp(62, 23, 5); AddFurnComp(62, 26, 1); AddFurnComp(62, 47, 4);
            // 63. Тумба мобильная Белая
            AddFurnComp(63, 21, 2); AddFurnComp(63, 13, 3); AddFurnComp(63, 48, 1); AddFurnComp(63, 75, 3);
            // 64. Тумба мобильная Графит
            AddFurnComp(64, 21, 2); AddFurnComp(64, 13, 3); AddFurnComp(64, 48, 1); AddFurnComp(64, 75, 3);
            // 65. Тумба приставная (4 ящика)
            AddFurnComp(65, 21, 2); AddFurnComp(65, 13, 4); AddFurnComp(65, 47, 4); AddFurnComp(65, 75, 4);
            // 66. Куб
            AddFurnComp(66, 21, 2); AddFurnComp(66, 23, 2); AddFurnComp(66, 80, 2);
            // 67. Соты (3 куба)
            AddFurnComp(67, 21, 6); AddFurnComp(67, 23, 6); AddFurnComp(67, 80, 6);
            // 68. Полка 900
            AddFurnComp(68, 24, 1); AddFurnComp(68, 79, 2);
            // 69. Витрина библиотека
            AddFurnComp(69, 21, 2); AddFurnComp(69, 23, 5); AddFurnComp(69, 8, 2); AddFurnComp(69, 71, 4);
            // 70. Стеллаж ступеньки
            AddFurnComp(70, 21, 4); AddFurnComp(70, 23, 6); AddFurnComp(70, 47, 6);

            // --- 8. Разное (ID 71-80) ---
            // 71. Вешалка
            AddFurnComp(71, 45, 4); AddFurnComp(71, 90, 1); AddFurnComp(71, 76, 4); // Крючки из ручек
                                                                                    // 72. Обувница (Сидушка + ящик)
            AddFurnComp(72, 21, 2); AddFurnComp(72, 61, 1); AddFurnComp(72, 12, 1); AddFurnComp(72, 72, 2);
            // 73. Шкаф прихожая
            AddFurnComp(73, 21, 2); AddFurnComp(73, 19, 1); AddFurnComp(73, 27, 1); AddFurnComp(73, 71, 4);
            // 74. Стол детский
            AddFurnComp(74, 38, 1); AddFurnComp(74, 45, 1);
            // 75. Стул детский
            AddFurnComp(75, 61, 1); AddFurnComp(75, 45, 1);
            // 76. Ящик игрушки
            AddFurnComp(76, 21, 2); AddFurnComp(76, 14, 2); AddFurnComp(76, 48, 1);
            // 77. Тумба ванная
            AddFurnComp(77, 21, 2); AddFurnComp(77, 12, 1); AddFurnComp(77, 47, 4);
            // 78. Зеркало (Фасад глухой как подложка)
            AddFurnComp(78, 10, 1); AddFurnComp(78, 80, 2);
            // 79. Пеленальный комод
            AddFurnComp(79, 21, 2); AddFurnComp(79, 14, 3); AddFurnComp(79, 23, 1); AddFurnComp(79, 73, 3);
            // 80. Кровать чердак
            AddFurnComp(80, 52, 4); AddFurnComp(80, 53, 2); AddFurnComp(80, 54, 10); AddFurnComp(80, 49, 4);

            // --- 9. Столы/Тумбы доп (ID 81-90) ---
            // 81. Консоль (Ноги + узкая столешка)
            AddFurnComp(81, 24, 1); AddFurnComp(81, 44, 2);
            // 82. Придиванный (Металл + стекло)
            AddFurnComp(82, 36, 1); AddFurnComp(82, 43, 1);
            // 83. ТВ тумба
            AddFurnComp(83, 21, 3); AddFurnComp(83, 14, 2); AddFurnComp(83, 47, 6);
            // 84. ТВ бетон
            AddFurnComp(84, 22, 3); AddFurnComp(84, 15, 2); AddFurnComp(84, 43, 2);
            // 85. Стол раздвижной (2 столешки)
            AddFurnComp(85, 39, 2); AddFurnComp(85, 41, 4);
            // 86. Табурет
            AddFurnComp(86, 28, 1); AddFurnComp(86, 41, 4);
            // 87. Стремянка
            AddFurnComp(87, 21, 2); AddFurnComp(87, 23, 2);
            // 88. Сервант
            AddFurnComp(88, 21, 2); AddFurnComp(88, 9, 2); AddFurnComp(88, 25, 4);
            // 89. Полка обувь металл
            AddFurnComp(89, 43, 2); AddFurnComp(89, 93, 2); // Профили
                                                            // 90. Лингерье
            AddFurnComp(90, 21, 2); AddFurnComp(90, 13, 5); AddFurnComp(90, 75, 5); AddFurnComp(90, 45, 4);

            // --- 10. Декор (ID 91-100) ---
            // 91. Панель
            AddFurnComp(91, 19, 1); AddFurnComp(91, 94, 2);
            // 92. Угловая полка
            AddFurnComp(92, 23, 3); AddFurnComp(92, 80, 4);
            // 93. Стеллаж цветы
            AddFurnComp(93, 23, 3); AddFurnComp(93, 21, 2);
            // 94. Скамья
            AddFurnComp(94, 34, 1); AddFurnComp(94, 43, 2);
            // 95. Стол складной
            AddFurnComp(95, 39, 1); AddFurnComp(95, 42, 4);
            // 96. Кресло мешок (Только наполнитель)
            AddFurnComp(96, 61, 5);
            // 97. Подушка
            AddFurnComp(97, 65, 1);
            // 98. Экран батареи (Фасад решетка - прокси фасад 1)
            AddFurnComp(98, 1, 1); AddFurnComp(98, 80, 2);
            // 99. Полка невидимая
            AddFurnComp(99, 81, 2); AddFurnComp(99, 82, 2);
            // 100. Органайзер
            AddFurnComp(100, 13, 1); AddFurnComp(100, 28, 1);

            context.Set<FurnitureComposition>().AddRange(items);
            context.SaveChanges();
        }

        if (!context.Clients.Any())
        {
            var clients = new Client[]
            {
            new() { FullName = "Иванов Александр Сергеевич", Phone = "+7 (916) 123-45-67", Email = "alex.ivanov88@gmail.com", UpdateDate = now },
            new() { FullName = "Смирнова Мария Владимировна", Phone = "+7 (903) 987-65-43", Email = "m.smirnova@yandex.ru", UpdateDate = now },
            new() { FullName = "Кузнецов Дмитрий Андреевич", Phone = "+7 (925) 555-01-01", Email = "kuznetsov.dima@mail.ru", UpdateDate = now },
            new() { FullName = "Попова Елена Игоревна", Phone = "+7 (910) 444-22-33", Email = "elena.popova@bk.ru", UpdateDate = now },
            new() { FullName = "Соколов Максим Юрьевич", Phone = "+7 (965) 777-88-99", Email = "max.sokolov@gmail.com", UpdateDate = now },
            new() { FullName = "Лебедева Анна Петровна", Phone = "+7 (926) 111-22-33", Email = "annalebedeva90@yandex.ru", UpdateDate = now },
            new() { FullName = "Козлов Артем Викторович", Phone = "+7 (999) 000-11-22", Email = "artem.kozlov@icloud.com", UpdateDate = now },
            new() { FullName = "Новикова Ольга Дмитриевна", Phone = "+7 (905) 333-44-55", Email = "o.novikova@mail.ru", UpdateDate = now },
            new() { FullName = "Морозов Иван Николаевич", Phone = "+7 (915) 222-33-44", Email = "morozov.ivan@gmail.com", UpdateDate = now },
            new() { FullName = "Петрова Татьяна Александровна", Phone = "+7 (909) 555-66-77", Email = "tanya.petrova@list.ru", UpdateDate = now },

            new() { FullName = "Волков Сергей Михайлович", Phone = "+7 (916) 789-01-23", Email = "volkov_sergey@yandex.ru", UpdateDate = now },
            new() { FullName = "Соловьева Екатерина Павловна", Phone = "+7 (926) 456-78-90", Email = "ekaterina.soloveva@gmail.com", UpdateDate = now },
            new() { FullName = "Васильев Андрей Олегович", Phone = "+7 (903) 123-98-76", Email = "vasilyev.andrey@mail.ru", UpdateDate = now },
            new() { FullName = "Зайцева Наталья Евгеньевна", Phone = "+7 (910) 654-32-10", Email = "nataly.zaytseva@bk.ru", UpdateDate = now },
            new() { FullName = "Павлов Роман Алексеевич", Phone = "+7 (925) 321-09-87", Email = "roman.pavlov@outlook.com", UpdateDate = now },
            new() { FullName = "Семенова Юлия Сергеевна", Phone = "+7 (915) 987-65-43", Email = "julia.semenova@yandex.ru", UpdateDate = now },
            new() { FullName = "Голубев Павел Константинович", Phone = "+7 (968) 111-00-11", Email = "pavel.golubev@gmail.com", UpdateDate = now },
            new() { FullName = "Виноградова Светлана Викторовна", Phone = "+7 (906) 222-33-44", Email = "sveta.vinogradova@mail.ru", UpdateDate = now },
            new() { FullName = "Богданов Илья Романович", Phone = "+7 (977) 333-44-55", Email = "ilya.bogdanov@icloud.com", UpdateDate = now },
            new() { FullName = "Воробьева Анастасия Денисовна", Phone = "+7 (917) 555-66-77", Email = "nastya.vorobyeva@yandex.ru", UpdateDate = now },

            new() { FullName = "Федоров Кирилл Андреевич", Phone = "+7 (929) 777-88-99", Email = "kirill.fedorov@gmail.com", UpdateDate = now },
            new() { FullName = "Михайлова Ирина Владимировна", Phone = "+7 (903) 111-22-33", Email = "irina.mikhailova@list.ru", UpdateDate = now },
            new() { FullName = "Беляев Антон Максимович", Phone = "+7 (916) 444-55-66", Email = "anton.belyaev@mail.ru", UpdateDate = now },
            new() { FullName = "Тарасова Вероника Игоревна", Phone = "+7 (926) 777-00-11", Email = "nika.tarasova@yandex.ru", UpdateDate = now },
            new() { FullName = "Белов Владимир Сергеевич", Phone = "+7 (915) 888-99-00", Email = "vladimir.belov@gmail.com", UpdateDate = now },
            new() { FullName = "Комарова Дарья Алексеевна", Phone = "+7 (905) 123-45-67", Email = "dasha.komarova@bk.ru", UpdateDate = now },
            new() { FullName = "Орлов Никита Дмитриевич", Phone = "+7 (910) 987-65-43", Email = "nikita.orlov@outlook.com", UpdateDate = now },
            new() { FullName = "Киселева Марина Павловна", Phone = "+7 (925) 234-56-78", Email = "marina.kiseleva@yandex.ru", UpdateDate = now },
            new() { FullName = "Макаров Егор Вячеславович", Phone = "+7 (963) 876-54-32", Email = "egor.makarov@gmail.com", UpdateDate = now },
            new() { FullName = "Андреева Ксения Николаевна", Phone = "+7 (909) 345-67-89", Email = "ksenia.andreeva@mail.ru", UpdateDate = now },

            new() { FullName = "Ковалев Алексей Юрьевич", Phone = "+7 (916) 901-23-45", Email = "alexey.kovalev@yandex.ru", UpdateDate = now },
            new() { FullName = "Ильина Алина Викторовна", Phone = "+7 (926) 543-21-09", Email = "alina.ilyina@gmail.com", UpdateDate = now },
            new() { FullName = "Гусев Тимофей Александрович", Phone = "+7 (903) 678-90-12", Email = "timofey.gusev@mail.ru", UpdateDate = now },
            new() { FullName = "Титова Виктория Андреевна", Phone = "+7 (910) 210-98-76", Email = "v.titova@inbox.ru", UpdateDate = now },
            new() { FullName = "Кузьмин Даниил Сергеевич", Phone = "+7 (925) 345-67-89", Email = "daniil.kuzmin@gmail.com", UpdateDate = now },
            new() { FullName = "Кудрявцева Полина Дмитриевна", Phone = "+7 (915) 987-65-43", Email = "polina.kudryavtseva@yandex.ru", UpdateDate = now },
            new() { FullName = "Баранов Матвей Игоревич", Phone = "+7 (905) 123-45-67", Email = "matvey.baranov@icloud.com", UpdateDate = now },
            new() { FullName = "Куликова София Алексеевна", Phone = "+7 (916) 765-43-21", Email = "sofia.kulikova@mail.ru", UpdateDate = now },
            new() { FullName = "Алексеев Григорий Павлович", Phone = "+7 (926) 001-12-23", Email = "grigory.alexeev@gmail.com", UpdateDate = now },
            new() { FullName = "Степанова Варвара Николаевна", Phone = "+7 (903) 332-21-10", Email = "varvara.stepanova@yandex.ru", UpdateDate = now },

            new() { FullName = "Яковлев Олег Владимирович", Phone = "+7 (910) 445-56-67", Email = "oleg.yakovlev@bk.ru", UpdateDate = now },
            new() { FullName = "Сорокина Елизавета Михайловна", Phone = "+7 (925) 778-89-90", Email = "liza.sorokina@gmail.com", UpdateDate = now },
            new() { FullName = "Сергеев Вадим Александрович", Phone = "+7 (915) 112-23-34", Email = "vadim.sergeev@mail.ru", UpdateDate = now },
            new() { FullName = "Романова Александра Денисовна", Phone = "+7 (905) 998-87-76", Email = "alexandra.romanova@yandex.ru", UpdateDate = now },
            new() { FullName = "Захаров Владислав Игоревич", Phone = "+7 (968) 554-43-32", Email = "vlad.zakharov@gmail.com", UpdateDate = now },
            new() { FullName = "Борисова Евгения Андреевна", Phone = "+7 (977) 221-10-09", Email = "evgeniya.borisova@list.ru", UpdateDate = now },
            new() { FullName = "Королев Станислав Юрьевич", Phone = "+7 (916) 667-78-89", Email = "stas.korolev@yandex.ru", UpdateDate = now },
            new() { FullName = "Герасимова Кристина Павловна", Phone = "+7 (926) 990-01-12", Email = "kristina.gerasimova@mail.ru", UpdateDate = now },
            new() { FullName = "Пономарев Денис Викторович", Phone = "+7 (903) 443-32-21", Email = "denis.ponomarev@gmail.com", UpdateDate = now },
            new() { FullName = "Григорьева Маргарита Сергеевна", Phone = "+7 (910) 887-76-65", Email = "margarita.grigoryeva@icloud.com", UpdateDate = now }
            };

            context.Clients.AddRange(clients);
            context.SaveChanges();
        }

        if (!context.Orders.Any())
        {
            var orders = new Order[]
            {
                new() { ClientId = 1, Discount = null, UpdateDate = now },
                new() { ClientId = 2, Discount = null, UpdateDate = now },
                new() { ClientId = 3, Discount = null, UpdateDate = now },
                new() { ClientId = 4, Discount = null, UpdateDate = now },
                new() { ClientId = 5, Discount = null, UpdateDate = now },
                new() { ClientId = 6, Discount = null, UpdateDate = now },
                new() { ClientId = 7, Discount = null, UpdateDate = now },
                new() { ClientId = 8, Discount = null, UpdateDate = now },
                new() { ClientId = 9, Discount = null, UpdateDate = now },
                new() { ClientId = 10, Discount = null, UpdateDate = now },
                new() { ClientId = 11, Discount = null, UpdateDate = now },
                new() { ClientId = 12, Discount = null, UpdateDate = now },
                new() { ClientId = 13, Discount = null, UpdateDate = now },
                new() { ClientId = 14, Discount = null, UpdateDate = now },
                new() { ClientId = 15, Discount = null, UpdateDate = now },
                new() { ClientId = 16, Discount = null, UpdateDate = now },
                new() { ClientId = 17, Discount = null, UpdateDate = now },
                new() { ClientId = 18, Discount = null, UpdateDate = now },
                new() { ClientId = 19, Discount = null, UpdateDate = now },
                new() { ClientId = 20, Discount = null, UpdateDate = now },
                new() { ClientId = 21, Discount = null, UpdateDate = now },
                new() { ClientId = 22, Discount = null, UpdateDate = now },
                new() { ClientId = 23, Discount = null, UpdateDate = now },
                new() { ClientId = 24, Discount = null, UpdateDate = now },
                new() { ClientId = 25, Discount = null, UpdateDate = now },
                new() { ClientId = 26, Discount = null, UpdateDate = now },
                new() { ClientId = 27, Discount = null, UpdateDate = now },
                new() { ClientId = 28, Discount = null, UpdateDate = now },
                new() { ClientId = 29, Discount = null, UpdateDate = now },
                new() { ClientId = 30, Discount = null, UpdateDate = now },
                new() { ClientId = 31, Discount = null, UpdateDate = now },
                new() { ClientId = 32, Discount = null, UpdateDate = now },
                new() { ClientId = 33, Discount = null, UpdateDate = now },
                new() { ClientId = 34, Discount = null, UpdateDate = now },
                new() { ClientId = 35, Discount = null, UpdateDate = now },
                new() { ClientId = 36, Discount = null, UpdateDate = now },
                new() { ClientId = 37, Discount = null, UpdateDate = now },
                new() { ClientId = 38, Discount = null, UpdateDate = now },
                new() { ClientId = 39, Discount = null, UpdateDate = now },
                new() { ClientId = 40, Discount = null, UpdateDate = now },
                new() { ClientId = 41, Discount = null, UpdateDate = now },
                new() { ClientId = 42, Discount = null, UpdateDate = now },
                new() { ClientId = 43, Discount = null, UpdateDate = now },
                new() { ClientId = 44, Discount = null, UpdateDate = now },
                new() { ClientId = 45, Discount = null, UpdateDate = now },
                new() { ClientId = 46, Discount = null, UpdateDate = now },
                new() { ClientId = 47, Discount = null, UpdateDate = now },
                new() { ClientId = 48, Discount = null, UpdateDate = now },
                new() { ClientId = 49, Discount = null, UpdateDate = now },
                new() { ClientId = 50, Discount = null, UpdateDate = now },

                new() { ClientId = 1, Discount = 15, UpdateDate = now },
                new() { ClientId = 2, Discount = 10, UpdateDate = now },
                new() { ClientId = 3, Discount = 20, UpdateDate = now },
                new() { ClientId = 4, Discount = 12, UpdateDate = now },
                new() { ClientId = 5, Discount = 18, UpdateDate = now },
                new() { ClientId = 6, Discount = 10, UpdateDate = now },
                new() { ClientId = 7, Discount = 14, UpdateDate = now },
                new() { ClientId = 8, Discount = 19, UpdateDate = now },
                new() { ClientId = 9, Discount = 11, UpdateDate = now },
                new() { ClientId = 10, Discount = 16, UpdateDate = now },
            };

            context.Orders.AddRange(orders);
            context.SaveChanges();
        }

        if (!context.Set<StatusChange>().Any())
        {
            var history = new List<StatusChange>();

            void AddStChange(int orderId, int statusId, int daysAgo)
            {
                history.Add(new StatusChange
                {
                    Entity1Id = orderId,
                    Entity2Id = statusId,
                    Date = now.AddDays(-daysAgo),
                    UpdateDate = now
                });
            }

            // Заказ 1
            AddStChange(1, 1, 30); AddStChange(1, 2, 29); AddStChange(1, 3, 28); AddStChange(1, 4, 27); AddStChange(1, 5, 20); AddStChange(1, 6, 5); AddStChange(1, 7, 4); AddStChange(1, 8, 2);
            // Заказ 2
            AddStChange(2, 1, 30); AddStChange(2, 2, 29); AddStChange(2, 3, 28); AddStChange(2, 4, 27); AddStChange(2, 5, 20); AddStChange(2, 6, 5); AddStChange(2, 7, 4); AddStChange(2, 8, 2);
            // Заказ 3
            AddStChange(3, 1, 28); AddStChange(3, 2, 27); AddStChange(3, 3, 26); AddStChange(3, 4, 25); AddStChange(3, 5, 18); AddStChange(3, 6, 4); AddStChange(3, 7, 3); AddStChange(3, 8, 1);
            // Заказ 4
            AddStChange(4, 1, 28); AddStChange(4, 2, 27); AddStChange(4, 3, 26); AddStChange(4, 4, 25); AddStChange(4, 5, 18); AddStChange(4, 6, 4); AddStChange(4, 7, 3); AddStChange(4, 8, 1);
            // Заказ 5
            AddStChange(5, 1, 25); AddStChange(5, 2, 24); AddStChange(5, 3, 23); AddStChange(5, 4, 22); AddStChange(5, 5, 15); AddStChange(5, 6, 3); AddStChange(5, 7, 2); AddStChange(5, 8, 0);
            // Заказ 6
            AddStChange(6, 1, 25); AddStChange(6, 2, 24); AddStChange(6, 3, 23); AddStChange(6, 4, 22); AddStChange(6, 5, 15); AddStChange(6, 6, 3); AddStChange(6, 7, 2); AddStChange(6, 8, 0);
            // Заказ 7
            AddStChange(7, 1, 30); AddStChange(7, 2, 29); AddStChange(7, 3, 25); AddStChange(7, 4, 24); AddStChange(7, 5, 10); AddStChange(7, 6, 4); AddStChange(7, 7, 3); AddStChange(7, 8, 1);
            // Заказ 8
            AddStChange(8, 1, 30); AddStChange(8, 2, 29); AddStChange(8, 3, 25); AddStChange(8, 4, 24); AddStChange(8, 5, 10); AddStChange(8, 6, 4); AddStChange(8, 7, 3); AddStChange(8, 8, 1);
            // Заказ 9
            AddStChange(9, 1, 20); AddStChange(9, 2, 19); AddStChange(9, 3, 18); AddStChange(9, 4, 17); AddStChange(9, 5, 10); AddStChange(9, 6, 5); AddStChange(9, 7, 4); AddStChange(9, 8, 2);
            // Заказ 10
            AddStChange(10, 1, 20); AddStChange(10, 2, 19); AddStChange(10, 3, 18); AddStChange(10, 4, 17); AddStChange(10, 5, 10); AddStChange(10, 6, 5); AddStChange(10, 7, 4); AddStChange(10, 8, 2);

            // Заказ 11
            AddStChange(11, 1, 15); AddStChange(11, 2, 14); AddStChange(11, 3, 13); AddStChange(11, 4, 12); AddStChange(11, 5, 5);
            // Заказ 12
            AddStChange(12, 1, 15); AddStChange(12, 2, 14); AddStChange(12, 3, 13); AddStChange(12, 4, 12); AddStChange(12, 5, 5);
            // Заказ 13
            AddStChange(13, 1, 14); AddStChange(13, 2, 13); AddStChange(13, 3, 12); AddStChange(13, 4, 11); AddStChange(13, 5, 4);
            // Заказ 14
            AddStChange(14, 1, 14); AddStChange(14, 2, 13); AddStChange(14, 3, 12); AddStChange(14, 4, 11); AddStChange(14, 5, 4);
            // Заказ 15
            AddStChange(15, 1, 12); AddStChange(15, 2, 11); AddStChange(15, 3, 10); AddStChange(15, 4, 9); AddStChange(15, 5, 2);
            // Заказ 16
            AddStChange(16, 1, 12); AddStChange(16, 2, 11); AddStChange(16, 3, 10); AddStChange(16, 4, 9); AddStChange(16, 5, 2);
            // Заказ 17
            AddStChange(17, 1, 10); AddStChange(17, 2, 9); AddStChange(17, 3, 8); AddStChange(17, 4, 7); AddStChange(17, 5, 1);
            // Заказ 18
            AddStChange(18, 1, 10); AddStChange(18, 2, 9); AddStChange(18, 3, 8); AddStChange(18, 4, 7); AddStChange(18, 5, 1);
            // Заказ 19
            AddStChange(19, 1, 8); AddStChange(19, 2, 7); AddStChange(19, 3, 6); AddStChange(19, 4, 5); AddStChange(19, 5, 0);
            // Заказ 20
            AddStChange(20, 1, 8); AddStChange(20, 2, 7); AddStChange(20, 3, 6); AddStChange(20, 4, 5); AddStChange(20, 5, 0);

            // Заказ 21
            AddStChange(21, 1, 5); AddStChange(21, 2, 4); AddStChange(21, 3, 3);
            // Заказ 22
            AddStChange(22, 1, 5); AddStChange(22, 2, 4); AddStChange(22, 3, 3);
            // Заказ 23
            AddStChange(23, 1, 4); AddStChange(23, 2, 3); AddStChange(23, 3, 2);
            // Заказ 24
            AddStChange(24, 1, 4); AddStChange(24, 2, 3); AddStChange(24, 3, 2);
            // Заказ 25
            AddStChange(25, 1, 3); AddStChange(25, 2, 2); AddStChange(25, 3, 1);

            // Заказ 26
            AddStChange(26, 1, 10); AddStChange(26, 2, 9); AddStChange(26, 3, 8); AddStChange(26, 4, 1);
            // Заказ 27
            AddStChange(27, 1, 10); AddStChange(27, 2, 9); AddStChange(27, 3, 8); AddStChange(27, 4, 1);
            // Заказ 28
            AddStChange(28, 1, 8); AddStChange(28, 2, 7); AddStChange(28, 3, 6); AddStChange(28, 4, 0);
            // Заказ 29
            AddStChange(29, 1, 8); AddStChange(29, 2, 7); AddStChange(29, 3, 6); AddStChange(29, 4, 0);
            // Заказ 30
            AddStChange(30, 1, 5); AddStChange(30, 2, 4); AddStChange(30, 3, 3); AddStChange(30, 4, 0);

            // Заказ 31
            AddStChange(31, 1, 20); AddStChange(31, 2, 19); AddStChange(31, 10, 18);
            // Заказ 32
            AddStChange(32, 1, 20); AddStChange(32, 2, 19); AddStChange(32, 10, 18);
            // Заказ 33
            AddStChange(33, 1, 15); AddStChange(33, 2, 14); AddStChange(33, 10, 13);
            // Заказ 34
            AddStChange(34, 1, 15); AddStChange(34, 2, 14); AddStChange(34, 10, 13);
            // Заказ 35
            AddStChange(35, 1, 10); AddStChange(35, 2, 9); AddStChange(35, 10, 8);

            // Заказ 36
            AddStChange(36, 1, 25); AddStChange(36, 2, 24); AddStChange(36, 3, 23); AddStChange(36, 10, 10);
            // Заказ 37
            AddStChange(37, 1, 25); AddStChange(37, 2, 24); AddStChange(37, 3, 23); AddStChange(37, 10, 10);
            // Заказ 38
            AddStChange(38, 1, 20); AddStChange(38, 2, 19); AddStChange(38, 3, 18); AddStChange(38, 10, 5);
            // Заказ 39
            AddStChange(39, 1, 20); AddStChange(39, 2, 19); AddStChange(39, 3, 18); AddStChange(39, 10, 5);
            // Заказ 40
            AddStChange(40, 1, 15); AddStChange(40, 2, 14); AddStChange(40, 3, 13); AddStChange(40, 10, 1);

            // Заказ 41
            AddStChange(41, 1, 20); AddStChange(41, 2, 19); AddStChange(41, 3, 18); AddStChange(41, 4, 17); AddStChange(41, 5, 10); AddStChange(41, 9, 2);
            // Заказ 42
            AddStChange(42, 1, 20); AddStChange(42, 2, 19); AddStChange(42, 3, 18); AddStChange(42, 4, 17); AddStChange(42, 5, 10); AddStChange(42, 9, 2);
            // Заказ 43
            AddStChange(43, 1, 15); AddStChange(43, 2, 14); AddStChange(43, 3, 13); AddStChange(43, 4, 12); AddStChange(43, 5, 5); AddStChange(43, 9, 1);
            // Заказ 44
            AddStChange(44, 1, 15); AddStChange(44, 2, 14); AddStChange(44, 3, 13); AddStChange(44, 4, 12); AddStChange(44, 5, 5); AddStChange(44, 9, 1);
            // Заказ 45
            AddStChange(45, 1, 10); AddStChange(45, 2, 9); AddStChange(45, 3, 8); AddStChange(45, 4, 7); AddStChange(45, 5, 2); AddStChange(45, 9, 0);

            // Заказ 46
            AddStChange(46, 1, 25); AddStChange(46, 2, 24); AddStChange(46, 3, 23); AddStChange(46, 4, 22); AddStChange(46, 5, 10); AddStChange(46, 6, 2);
            // Заказ 47
            AddStChange(47, 1, 25); AddStChange(47, 2, 24); AddStChange(47, 3, 23); AddStChange(47, 4, 22); AddStChange(47, 5, 10); AddStChange(47, 6, 2);
            // Заказ 48
            AddStChange(48, 1, 20); AddStChange(48, 2, 19); AddStChange(48, 3, 18); AddStChange(48, 4, 17); AddStChange(48, 5, 8); AddStChange(48, 6, 1);
            // Заказ 49
            AddStChange(49, 1, 20); AddStChange(49, 2, 19); AddStChange(49, 3, 18); AddStChange(49, 4, 17); AddStChange(49, 5, 8); AddStChange(49, 6, 1);
            // Заказ 50
            AddStChange(50, 1, 15); AddStChange(50, 2, 14); AddStChange(50, 3, 13); AddStChange(50, 4, 12); AddStChange(50, 5, 5); AddStChange(50, 6, 0);

            // Заказ 51 (Иванов) - Завершен
            AddStChange(51, 1, 15); AddStChange(51, 2, 14); AddStChange(51, 3, 13); AddStChange(51, 4, 12); AddStChange(51, 5, 8); AddStChange(51, 6, 3); AddStChange(51, 7, 2); AddStChange(51, 8, 1);

            // Заказ 52 (Смирнова) - Отгружен (в пути)
            AddStChange(52, 1, 12); AddStChange(52, 2, 11); AddStChange(52, 3, 10); AddStChange(52, 4, 9); AddStChange(52, 5, 4); AddStChange(52, 6, 2); AddStChange(52, 7, 0);

            // Заказ 53 (Кузнецов) - В производстве
            AddStChange(53, 1, 10); AddStChange(53, 2, 9); AddStChange(53, 3, 8); AddStChange(53, 4, 7); AddStChange(53, 5, 1);

            // Заказ 54 (Попова) - В обработке
            AddStChange(54, 1, 2); AddStChange(54, 2, 1);

            // Заказ 55 (Соколов) - Новый
            AddStChange(55, 1, 0);

            // Заказ 56 (Лебедева) - Был на паузе в "Ожидает оплаты", сейчас Оплачен
            AddStChange(56, 1, 20); AddStChange(56, 2, 19); AddStChange(56, 3, 18); AddStChange(56, 9, 10); AddStChange(56, 4, 1);

            // Заказ 57 (Козлов) - Отменен сразу
            AddStChange(57, 1, 5); AddStChange(57, 10, 5);

            // Заказ 58 (Новикова) - Долгий заказ, сейчас на паузе в производстве
            AddStChange(58, 1, 28); AddStChange(58, 2, 27); AddStChange(58, 3, 26); AddStChange(58, 4, 25); AddStChange(58, 5, 15); AddStChange(58, 9, 5);

            // Заказ 59 (Морозов) - Завершен (быстро)
            AddStChange(59, 1, 10); AddStChange(59, 2, 9); AddStChange(59, 3, 9); AddStChange(59, 4, 9); AddStChange(59, 5, 5); AddStChange(59, 6, 2); AddStChange(59, 7, 1); AddStChange(59, 8, 0);

            // Заказ 60 (Петрова) - Ожидает оплаты (давно)
            AddStChange(60, 1, 15); AddStChange(60, 2, 14); AddStChange(60, 3, 13);

            context.Set<StatusChange>().AddRange(history);
            context.SaveChanges();
        }

        if (!context.Set<OrderComposition>().AsNoTracking().Any())
        {
            var items = new List<OrderComposition>();

            // Вспомогательный метод для добавления строки заказа
            void AddOrderComp(int orderId, int furnitureId, int count) =>
                items.Add(new OrderComposition
                {
                    Entity1Id = orderId, // OrderId
                    Entity2Id = furnitureId, // FurnitureId
                    Count = count,
                    UpdateDate = now
                });

            // --- Блок 1: Кухни (Заказы 1-10) ---

            // Заказ 1: Классическая белая кухня
            AddOrderComp(1, 1, 2);  // Шкаф навесной 400 Белый
            AddOrderComp(1, 2, 1);  // Шкаф навесной 600 Белый
            AddOrderComp(1, 7, 1);  // Шкаф-витрина 400
            AddOrderComp(1, 12, 1); // Стол под мойку 600
            AddOrderComp(1, 14, 1); // Тумба с ящиками 600
            AddOrderComp(1, 11, 2); // Стол рабочий 400
            AddOrderComp(1, 54, 1); // Шкаф под вытяжку

            // Заказ 2: Кухня Дуб Сонома + Обеденная зона
            AddOrderComp(2, 3, 2);  // Шкаф навесной 400 Сонома
            AddOrderComp(2, 4, 2);  // Шкаф навесной 600 Сонома
            AddOrderComp(2, 15, 2); // Тумба с глубокими ящиками
            AddOrderComp(2, 12, 1); // Стол под мойку
            AddOrderComp(2, 22, 1); // Стол обеденный Сканди
            AddOrderComp(2, 32, 4); // Стул кухонный Престиж

            // Заказ 3: Кухня Лофт (Бетон)
            AddOrderComp(3, 5, 3);  // Шкаф навесной 400 Бетон
            AddOrderComp(3, 6, 1);  // Шкаф навесной 600 Бетон
            AddOrderComp(3, 16, 2); // Тумба нижняя Графит
            AddOrderComp(3, 17, 1); // Тумба нижняя 450
            AddOrderComp(3, 26, 1); // Стойка барная Тик
            AddOrderComp(3, 33, 2); // Стул барный Лофт

            // Заказ 4: Маленькая кухня + хранение
            AddOrderComp(4, 2, 2);  // Шкаф навесной 600 Белый
            AddOrderComp(4, 12, 1); // Стол под мойку
            AddOrderComp(4, 13, 1); // Тумба с ящиками 400
            AddOrderComp(4, 18, 1); // Шкаф хозяйственный

            // Заказ 5: Угловая кухня
            AddOrderComp(5, 9, 1);  // Шкаф угловой
            AddOrderComp(5, 1, 2);  // Шкаф 400
            AddOrderComp(5, 60, 1); // Угловое окончание (низ)
            AddOrderComp(5, 59, 1); // Угловое окончание (верх)
            AddOrderComp(5, 14, 2); // Тумба 600

            // Заказ 6: Кухня с пеналами
            AddOrderComp(6, 10, 1); // Пенал кухонный
            AddOrderComp(6, 19, 1); // Шкаф продуктовый
            AddOrderComp(6, 56, 1); // Пенал под духовку
            AddOrderComp(6, 15, 2); // Тумба с ящиками

            // Заказ 7: Бюджетная кухня
            AddOrderComp(7, 1, 3);  // 3 шкафа навесных
            AddOrderComp(7, 11, 3); // 3 стола рабочих
            AddOrderComp(7, 86, 1); // Табурет эконом

            // Заказ 8: Докомплектация кухни
            AddOrderComp(8, 20, 1); // Бутылочница
            AddOrderComp(8, 57, 1); // Фасад ПММ 45
            AddOrderComp(8, 95, 1); // Карниз

            // Заказ 9: Большая обеденная группа
            AddOrderComp(9, 21, 1); // Стол Лофт Дуб
            AddOrderComp(9, 31, 6); // Стул мягкий Комфорт
            AddOrderComp(9, 88, 1); // Сервант

            // Заказ 10: Кофейная зона
            AddOrderComp(10, 25, 1); // Столик кофейный Прованс
            AddOrderComp(10, 36, 2); // Кресло Релакс

            // --- Блок 2: Спальни (Заказы 11-20) ---

            // Заказ 11: Спальня Кантри
            AddOrderComp(11, 41, 1); // Кровать Кантри
            AddOrderComp(11, 46, 1); // Матрас Кинг
            AddOrderComp(11, 47, 2); // Тумба прикроватная Дуб

            // Заказ 12: Спальня Современная
            AddOrderComp(12, 42, 1); // Кровать Софт (мягкая)
            AddOrderComp(12, 46, 1); // Матрас Кинг
            AddOrderComp(12, 48, 2); // Тумба Белая
            AddOrderComp(12, 49, 1); // Шкаф платяной

            // Заказ 13: Детская спальня
            AddOrderComp(13, 43, 1); // Кровать односпальная
            AddOrderComp(13, 45, 1); // Матрас Стандарт
            AddOrderComp(13, 76, 1); // Ящик для игрушек

            // Заказ 14: Гостевая спальня
            AddOrderComp(14, 44, 1); // Кровать с ящиками
            AddOrderComp(14, 46, 1); // Матрас Кинг
            AddOrderComp(14, 50, 1); // Комод бельевой

            // Заказ 15: Только матрасы (обновление)
            AddOrderComp(15, 45, 2); // Матрас Стандарт
            AddOrderComp(15, 97, 2); // Подушка на стул

            // Заказ 16: Спальня + Рабочее место
            AddOrderComp(16, 41, 1); // Кровать
            AddOrderComp(16, 46, 1); // Матрас
            AddOrderComp(16, 28, 1); // Стол рабочий Домашний
            AddOrderComp(16, 31, 1); // Стул Комфорт

            // Заказ 17: Система хранения в спальню
            AddOrderComp(17, 49, 2); // 2 шкафа платяных
            AddOrderComp(17, 50, 1); // Комод
            AddOrderComp(17, 78, 1); // Зеркало в раме

            // Заказ 18: Кровать-чердак
            AddOrderComp(18, 80, 1); // Кровать-чердак
            AddOrderComp(18, 45, 1); // Матрас
            AddOrderComp(18, 96, 1); // Кресло-мешок

            // Заказ 19: Бюджетная спальня
            AddOrderComp(19, 43, 1); // Кровать Дачная
            AddOrderComp(19, 45, 1); // Матрас
            AddOrderComp(19, 47, 1); // Тумба

            // Заказ 20: Спальня Люкс
            AddOrderComp(20, 42, 1); // Кровать Софт
            AddOrderComp(20, 46, 1); // Матрас Кинг
            AddOrderComp(20, 38, 1); // Пуф Зефир
            AddOrderComp(20, 77, 1); // Тумба под раковину (видимо в мастер-ванную)

            // --- Блок 3: Гостиные (Заказы 21-30) ---

            // Заказ 21: ТВ-зона
            AddOrderComp(21, 34, 1); // Диван Фемили
            AddOrderComp(21, 83, 1); // ТВ-тумба Белая
            AddOrderComp(21, 24, 1); // Стол журнальный Гласс

            // Заказ 22: Зона отдыха
            AddOrderComp(22, 35, 1); // Диван Модерн
            AddOrderComp(22, 82, 1); // Стол придиванный
            AddOrderComp(22, 61, 1); // Стеллаж Лофт

            // Заказ 23: Библиотека
            AddOrderComp(23, 69, 2); // Шкаф-витрина Библиотека
            AddOrderComp(23, 36, 1); // Кресло Релакс
            AddOrderComp(23, 99, 1); // Полка невидимая

            // Заказ 24: Большая гостиная
            AddOrderComp(24, 34, 1); // Диван
            AddOrderComp(24, 36, 2); // 2 кресла
            AddOrderComp(24, 84, 1); // ТВ-тумба Бетон
            AddOrderComp(24, 67, 1); // Полки Соты

            // Заказ 25: Лофт гостиная
            AddOrderComp(25, 34, 1); // Диван
            AddOrderComp(25, 21, 1); // Стол Лофт
            AddOrderComp(25, 81, 1); // Стол консольный

            // Заказ 26: Мягкая мебель
            AddOrderComp(26, 40, 1); // Диван уличный (для террасы)
            AddOrderComp(26, 94, 1); // Скамья садовая

            // Заказ 27: Мелочи для гостиной
            AddOrderComp(27, 38, 2); // Пуф
            AddOrderComp(27, 66, 1); // Полка куб
            AddOrderComp(27, 92, 1); // Полка угловая

            // Заказ 28: Декор
            AddOrderComp(28, 91, 3); // Панель настенная
            AddOrderComp(28, 98, 2); // Экран для батареи

            // Заказ 29: Зонирование
            AddOrderComp(29, 70, 2); // Стеллаж-перегородка
            AddOrderComp(29, 93, 1); // Стеллаж для цветов

            // Заказ 30: Комплект столов
            AddOrderComp(30, 23, 1); // Стол Бистро
            AddOrderComp(30, 33, 2); // Стул барный

            // --- Блок 4: Офис и Кабинет (Заказы 31-40) ---

            // Заказ 31: Кабинет руководителя
            AddOrderComp(31, 29, 1); // Стол переговоров
            AddOrderComp(31, 27, 1); // Стол Офис-Про
            AddOrderComp(31, 37, 1); // Кресло Менеджер

            // Заказ 32: Рабочие места сотрудников
            AddOrderComp(32, 28, 2); // Стол Домашний (как офисный)
            AddOrderComp(32, 31, 2); // Стул мягкий
            AddOrderComp(32, 63, 2); // Тумба офисная

            // Заказ 33: Архив
            AddOrderComp(33, 62, 4); // Стеллаж для документов узкий
            AddOrderComp(33, 65, 2); // Тумба приставная

            // Заказ 34: Геймерское место
            AddOrderComp(34, 30, 1); // Стол Кибер
            AddOrderComp(34, 37, 1); // Кресло офисное
            AddOrderComp(34, 66, 2); // Полка куб

            // Заказ 35: Домашний офис
            AddOrderComp(35, 81, 1); // Стол консольный (для ноутбука)
            AddOrderComp(35, 31, 1); // Стул
            AddOrderComp(35, 100, 1); // Органайзер

            // Заказ 36: Переговорная
            AddOrderComp(36, 29, 1); // Стол переговоров
            AddOrderComp(36, 32, 8); // Стул Престиж

            // Заказ 37: Офисная кухня
            AddOrderComp(37, 11, 2); // Стол рабочий
            AddOrderComp(37, 2, 2);  // Шкаф навесной
            AddOrderComp(37, 23, 1); // Стол Бистро
            AddOrderComp(37, 86, 4); // Табурет

            // Заказ 38: Зона ожидания
            AddOrderComp(38, 34, 1); // Диван
            AddOrderComp(38, 24, 1); // Стол журнальный
            AddOrderComp(38, 71, 1); // Вешалка

            // Заказ 39: Маленький офис
            AddOrderComp(39, 27, 1); // Стол
            AddOrderComp(39, 64, 1); // Тумба Графит
            AddOrderComp(39, 68, 1); // Полка для книг

            // Заказ 40: Склад мебели (закупка)
            AddOrderComp(40, 87, 2); // Табурет стремянка
            AddOrderComp(40, 61, 2); // Стеллаж Лофт

            // --- Блок 5: Разное (Заказы 41-50) ---

            // Заказ 41: Прихожая
            AddOrderComp(41, 71, 1); // Вешалка напольная
            AddOrderComp(41, 72, 1); // Обувница
            AddOrderComp(41, 78, 1); // Зеркало
            AddOrderComp(41, 39, 1); // Банкетка

            // Заказ 42: Ванная
            AddOrderComp(42, 77, 1); // Тумба под раковину
            AddOrderComp(42, 78, 1); // Зеркало

            // Заказ 43: Детская игровая
            AddOrderComp(43, 74, 1); // Столик Творчество
            AddOrderComp(43, 75, 2); // Стульчик детский
            AddOrderComp(43, 76, 2); // Ящик для игрушек

            // Заказ 44: Для новорожденного
            AddOrderComp(44, 79, 1); // Комод пеленальный
            AddOrderComp(44, 50, 1); // Комод обычный

            // Заказ 45: Сад и дача
            AddOrderComp(45, 94, 1); // Скамья
            AddOrderComp(45, 95, 1); // Стол складной

            // Заказ 46: Балкон
            AddOrderComp(46, 93, 1); // Стеллаж цветы
            AddOrderComp(46, 38, 1); // Пуф
            AddOrderComp(46, 82, 1); // Стол придиванный

            // Заказ 47: Узкий коридор
            AddOrderComp(47, 73, 1); // Шкаф узкий
            AddOrderComp(47, 89, 1); // Полка для обуви металл

            // Заказ 48: Гардеробная
            AddOrderComp(48, 90, 2); // Комод высокий (Лингерье)
            AddOrderComp(48, 71, 1); // Вешалка

            // Заказ 49: Туристический набор
            AddOrderComp(49, 95, 1); // Стол складной
            AddOrderComp(49, 87, 1); // Табурет-стремянка (почему бы и нет)

            // Заказ 50: Мелочевка
            AddOrderComp(50, 66, 3); // Полка куб
            AddOrderComp(50, 67, 1); // Соты

            // --- Блок 6: Специфические заказы (51-60) ---

            // Заказ 51 (Иванов - Завершен): Полная обстановка студии
            AddOrderComp(51, 1, 2); AddOrderComp(51, 11, 2); // Мини-кухня
            AddOrderComp(51, 34, 1); // Диван
            AddOrderComp(51, 49, 1); // Шкаф
            AddOrderComp(51, 21, 1); // Стол
            AddOrderComp(51, 31, 2); // Стулья

            // Заказ 52 (Смирнова - Отгружен): Столовая группа
            AddOrderComp(52, 22, 1); // Стол Сканди
            AddOrderComp(52, 32, 4); // 4 стула

            // Заказ 53 (Кузнецов - В производстве): Кухня под заказ
            AddOrderComp(53, 56, 1); // Пенал под духовку
            AddOrderComp(53, 55, 1); // Шкаф под СВЧ
            AddOrderComp(53, 5, 4);  // Шкафы бетон

            // Заказ 54 (Попова - В обработке): Детская
            AddOrderComp(54, 80, 1); // Кровать чердак
            AddOrderComp(54, 74, 1); // Столик

            // Заказ 55 (Соколов - Новый): Только создал
            AddOrderComp(55, 30, 1); // Стол геймерский

            // Заказ 56 (Лебедева - Оплачен): Офис
            AddOrderComp(56, 29, 1); // Стол переговоров
            AddOrderComp(56, 37, 6); // Кресла

            // Заказ 57 (Козлов - Отменен): Передумал брать дорогой диван
            AddOrderComp(57, 35, 1); // Диван Модерн Терракот

            // Заказ 58 (Новикова - На паузе): Сложная кухня
            AddOrderComp(58, 59, 1); // Угол верх
            AddOrderComp(58, 60, 1); // Угол низ
            AddOrderComp(58, 20, 2); // Бутылочницы
            AddOrderComp(58, 3, 5);  // Фасады дерево

            // Заказ 59 (Морозов - Завершен): Мелочь
            AddOrderComp(59, 86, 2); // Табуреты
            AddOrderComp(59, 92, 1); // Полка

            // Заказ 60 (Петрова - Ожидает оплаты): Спальня
            AddOrderComp(60, 41, 1); // Кровать
            AddOrderComp(60, 46, 1); // Матрас

            context.Set<OrderComposition>().AddRange(items);
            context.SaveChanges();
        }
    }
}

