using ClosedXML.Excel;
using FurniturePro.Core.Models.DTO.Clients;
using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Models.DTO.OrderCompositions;
using FurniturePro.Core.Models.DTO.Orders;
using FurniturePro.Core.Models.DTO.StatusChanges;
using FurniturePro.Core.Services.Interfaces; // Добавляем using для сервисов
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json; // Этот using можно удалить, так как он больше не нужен для парсинга

namespace FurniturePro.WebAdmin.Pages.Main
{
    public class OrdersModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IOrderCompositionService _orderCompositionService;
        private readonly IStatusChangeService _statusChangeService;
        private readonly IClientService _clientService;
        private readonly IDeletedIdService _deletedIdService;

        // Внедряем 4 необходимых сервиса вместо IHttpClientFactory
        public OrdersModel(
            IOrderService orderService,
            IOrderCompositionService orderCompositionService,
            IStatusChangeService statusChangeService,
            IDeletedIdService deletedIdService,
            IClientService clientService)
        {
            _orderService = orderService;
            _orderCompositionService = orderCompositionService;
            _statusChangeService = statusChangeService;
            _deletedIdService = deletedIdService;
            _clientService = clientService;
        }

        public void OnGet()
        {
        }

        // --- CRUD ЗАКАЗОВ ---

        public async Task<JsonResult> OnPostCreateOrderAsync(
            [FromForm] CreateOrderDTO orderDto,
            [FromForm] DateTime date,
            [FromForm] int statusId,
            CancellationToken ct)
        {
            try
            {
                // 1. Создаем заказ. Метод CreateAsync напрямую возвращает созданный ID!
                int newOrderId = await _orderService.CreateAsync(orderDto, ct);

                // 2. Если ID получен (а он будет получен, иначе выбросится Exception), создаем начальный статус
                if (newOrderId > 0 && statusId > 0)
                {
                    var statusDto = new StatusChangeDTO
                    {
                        IdOrder = newOrderId,
                        IdStatus = statusId,
                        Date = date,
                        UpdateDate = DateTime.UtcNow
                    };

                    await _statusChangeService.CreateAsync(statusDto, ct);
                }

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostCreateClientAsync([FromForm] CreateClientDTO dto, CancellationToken ct)
        {
            try
            {
                // Вызываем метод создания напрямую из сервиса
                var id = await _clientService.CreateAsync(dto, ct); // Имя метода может отличаться (например, AddAsync)

                return new JsonResult(new { success = true,  });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostUpdateOrderAsync([FromForm] int id, [FromForm] UpdateOrderDTO dto, CancellationToken ct)
        {
            try
            {
                await _orderService.UpdateAsync(id, dto, ct);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostDeleteOrderAsync([FromForm] int id, CancellationToken ct)
        {
            try
            {
                // 1. Удаляем заказ
                await _orderService.DeleteAsync(id, ct);

                // 2. Логируем удаление
                var delId = new CreateDeletedIdDTO() { EntityId = id, TableName = "orders" };
                await _deletedIdService.CreateAsync(delId, ct);

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        // --- STATUS & COMPOSITION ---

        public async Task<JsonResult> OnPostCreateStatusChangeAsync([FromForm] StatusChangeDTO dto, CancellationToken ct)
        {
            try
            {
                await _statusChangeService.CreateAsync(dto, ct);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        public async Task<JsonResult> OnPostCreateCompositionRangeAsync([FromBody] List<OrderCompositionDTO> dtos, CancellationToken ct)
        {
            try
            {
                await _orderCompositionService.CreateRangeAsync(dtos, ct);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        public async Task<JsonResult> OnPostUpdateCompositionRangeAsync([FromBody] List<OrderCompositionDTO> dtos, CancellationToken ct)
        {
            try
            {
                await _orderCompositionService.UpdateRangeAsync(dtos, ct);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        public async Task<JsonResult> OnPostDeleteCompositionRangeAsync([FromBody] List<SimpleKey> ids, CancellationToken ct)
        {
            try
            {
                // Преобразуем список ключей в формат кортежей (Id1, Id2), который требует сервис
                var servicePayload = ids.Select(x => (x.IdOrder, x.IdFurniture)).ToList();

                await _orderCompositionService.DeleteRangeAsync(servicePayload, ct);

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        public async Task<JsonResult> OnPostImportAsync(
            IFormFile excelFile,
            [FromForm] string existingOrdersStr,
            [FromForm] string clientsCacheStr,
            [FromForm] string statusesCacheStr,
            [FromForm] string furnituresCacheStr,
            CancellationToken ct)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                return new JsonResult(new { success = false, message = "Файл не выбран или пуст." });
            }

            var clientsCache = DeserializeCache(clientsCacheStr);
            var statusesCache = DeserializeCache(statusesCacheStr);
            var furnituresCache = DeserializeCache(furnituresCacheStr);

            var existingOrdersList = string.IsNullOrEmpty(existingOrdersStr) ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(existingOrdersStr);
            var existingAddresses = new HashSet<string>(existingOrdersList ?? new List<string>(), StringComparer.OrdinalIgnoreCase);

            int createdCount = 0;

            using (var stream = excelFile.OpenReadStream())
            using (var workbook = new XLWorkbook(stream))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

                foreach (var row in rows)
                {
                    // Новая структура: 1-Адрес, 2-Клиент, 3-Состав, 4-История статусов
                    var address = row.Cell(1).GetString().Trim();

                    if (string.IsNullOrEmpty(address) || existingAddresses.Contains(address))
                        continue;

                    var clientName = row.Cell(2).GetString().Trim();
                    var compositionStr = row.Cell(3).GetString().Trim();
                    var statusChangesStr = row.Cell(4).GetString().Trim();

                    int clientId = clientsCache.TryGetValue(clientName, out int cid) ? cid : -1;
                    if (clientId == -1) continue;

                    // --- 1. ПАРСИНГ ИСТОРИИ СТАТУСОВ ---
                    var statusChangeList = statusChangesStr.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                    var parsedStatuses = new List<(string Name, DateTime Date)>();
                    bool isStatusesValid = true;

                    foreach (var sc in statusChangeList)
                    {
                        int dashIndex = sc.LastIndexOf('-'); // Ищем последний дефис (на случай если он есть в самом статусе)
                        if (dashIndex > 0)
                        {
                            string sName = sc.Substring(0, dashIndex).Trim();
                            string sDate = sc.Substring(dashIndex + 1).Trim();

                            // Пытаемся распарсить дату. Формат даты должен поддерживаться текущей культурой
                            if (DateTime.TryParse(sDate, out DateTime dt))
                            {
                                parsedStatuses.Add((sName, dt));
                            }
                            else
                            {
                                isStatusesValid = false;
                                break;
                            }
                        }
                        else
                        {
                            isStatusesValid = false;
                            break;
                        }
                    }

                    if (!isStatusesValid || !parsedStatuses.Any()) continue;

                    // --- 2. ПРОВЕРКА ХРОНОЛОГИИ ---
                    for (int i = 0; i < parsedStatuses.Count - 1; i++)
                    {
                        // Каждое следующее событие должно быть позже или в то же время
                        if (parsedStatuses[i].Date > parsedStatuses[i + 1].Date)
                        {
                            isStatusesValid = false;
                            break;
                        }
                    }
                    if (!isStatusesValid) continue;

                    // --- 3. ПРОВЕРКА ЛОГИКИ СТАТУСОВ ---
                    var statusNames = parsedStatuses.Select(x => x.Name).ToList();
                    if (!IsValidStatusSequence(statusNames)) continue;

                    // --- 4. ПРОВЕРКА НАЛИЧИЯ СТАТУСОВ В БАЗЕ ---
                    var statusesToCreate = new List<StatusChangeDTO>();
                    foreach (var ps in parsedStatuses)
                    {
                        int statusId = statusesCache.TryGetValue(ps.Name, out int sid) ? sid : -1;
                        if (statusId == -1)
                        {
                            isStatusesValid = false;
                            break;
                        }

                        statusesToCreate.Add(new StatusChangeDTO
                        {
                            IdStatus = statusId,
                            Date = ps.Date,
                            UpdateDate = DateTime.UtcNow,
                            IdOrder = -1 // Заполним после создания заказа
                        });
                    }
                    if (!isStatusesValid) continue;


                    // --- 5. СОСТАВ ЗАКАЗА ---
                    bool skipOrder = false;
                    var compositionList = compositionStr.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                    var compositionsToCreate = new List<OrderCompositionDTO>();

                    foreach (var composition in compositionList)
                    {
                        var nameAndCount = composition.Split('-');
                        if (nameAndCount.Length >= 2)
                        {
                            var furnitureName = nameAndCount[0].Trim();
                            var countStr = nameAndCount[1].Trim();

                            int furnitureId = furnituresCache.TryGetValue(furnitureName, out int fid) ? fid : -1;

                            if (furnitureId == -1)
                            {
                                skipOrder = true;
                                break;
                            }

                            if (int.TryParse(countStr, out int count))
                            {
                                var compositionDto = new OrderCompositionDTO()
                                {
                                    Count = count,
                                    IdFurniture = furnitureId,
                                    IdOrder = -1,
                                    UpdateDate = DateTime.UtcNow
                                };
                                compositionsToCreate.Add(compositionDto);
                            }
                        }
                    }

                    if (skipOrder) continue;

                    // --- 6. СОЗДАНИЕ В БД ---
                    var orderDto = new CreateOrderDTO
                    {
                        Address = address,
                        ClientId = clientId
                    };
                    int newOrderId = await _orderService.CreateAsync(orderDto, ct);
                    existingAddresses.Add(address);
                    createdCount++;

                    // Сохраняем историю статусов по порядку
                    foreach (var st in statusesToCreate)
                    {
                        st.IdOrder = newOrderId;
                        await _statusChangeService.CreateAsync(st, ct);
                    }

                    compositionsToCreate.ForEach(composition => composition.IdOrder = newOrderId);
                    if (compositionsToCreate.Any())
                    {
                        await _orderCompositionService.CreateRangeAsync(compositionsToCreate, ct);
                    }
                }
            }

            if (createdCount == 0)
            {
                return new JsonResult(new { success = false, message = "В файле нет новых записей (или присутствуют логические ошибки переходов статусов)." });
            }

            return new JsonResult(new { success = true, count = createdCount });
        }

        private bool IsValidStatusSequence(List<string> sequence)
        {
            if (sequence == null || !sequence.Any()) return false;

            var mainStages = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "Создан", 1 },
                { "В обработке", 2 },
                { "Ожидает оплаты", 3 },
                { "Оплачен", 4 },
                { "Собирается", 5 },
                { "Передан в доставку", 6 },
                { "Предан в доставку", 6 },
                { "В пути", 7 },
                { "Доставлен", 8 }
            };

            int highestMainStage = 0;
            bool isCancelled = false;
            bool isReturnedToWarehouse = false;
            bool isRefunded = false;
            bool isCompleted = false;

            foreach (var statusName in sequence)
            {
                string status = statusName.Trim();

                if (isCompleted) return false; // После статуса Завершен изменений быть не может

                if (mainStages.TryGetValue(status, out int stage))
                {
                    if (isCancelled || isReturnedToWarehouse || isRefunded) return false;
                    if (highestMainStage == 0 && stage != 1) return false;

                    // Строгий переход ровно на 1 шаг вперед
                    if (stage != highestMainStage + 1) return false;

                    highestMainStage = stage;
                }
                else if (status.Equals("Отменен", StringComparison.OrdinalIgnoreCase))
                {
                    if (isCancelled) return false;
                    isCancelled = true;
                }
                else if (status.Equals("Возврат на склад", StringComparison.OrdinalIgnoreCase))
                {
                    if (!isCancelled) return false; // ПРАВИЛО: Только после отмены
                    if (highestMainStage < 6) return false; // ПРАВИЛО: Только если уже отдано в доставку
                    if (isReturnedToWarehouse) return false;
                    isReturnedToWarehouse = true;
                }
                else if (status.Equals("Средства возвращены", StringComparison.OrdinalIgnoreCase))
                {
                    if (!isCancelled) return false; // ПРАВИЛО: Только после отмены
                    if (highestMainStage < 4) return false; // ПРАВИЛО: Нельзя вернуть, если не оплачено

                    // ПРАВИЛО: Если товар ушел в доставку, нельзя вернуть средства, пока не вернут на склад
                    if (highestMainStage >= 6 && !isReturnedToWarehouse) return false;

                    if (isRefunded) return false;
                    isRefunded = true;
                }
                else if (status.Equals("Завершен", StringComparison.OrdinalIgnoreCase))
                {
                    bool canComplete = false;

                    // ПРАВИЛО: Доставлен (и не отменен)
                    if (!isCancelled && highestMainStage == 8) canComplete = true;
                    // ПРАВИЛО: Отменен до оплаты
                    if (isCancelled && highestMainStage < 4) canComplete = true;
                    // ПРАВИЛО: Средства возвращены
                    if (isRefunded) canComplete = true;

                    if (!canComplete) return false;
                    isCompleted = true;
                }
                else
                {
                    return false; // Неизвестный статус или мусор
                }
            }

            return true;
        }

        private Dictionary<string, int> DeserializeCache(string json)
        {
            if (string.IsNullOrEmpty(json))
                return new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            try
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, int>>(json);
                return dict == null
                    ? new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
                    : new Dictionary<string, int>(dict, StringComparer.OrdinalIgnoreCase);
            }
            catch
            {
                return new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            }
        }

        public class SimpleKey
        {
            public int IdOrder { get; set; }
            public int IdFurniture { get; set; }
        }
    }
}