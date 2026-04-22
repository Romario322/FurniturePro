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
        private readonly IDeletedIdService _deletedIdService;

        // Внедряем 4 необходимых сервиса вместо IHttpClientFactory
        public OrdersModel(
            IOrderService orderService,
            IOrderCompositionService orderCompositionService,
            IStatusChangeService statusChangeService,
            IDeletedIdService deletedIdService)
        {
            _orderService = orderService;
            _orderCompositionService = orderCompositionService;
            _statusChangeService = statusChangeService;
            _deletedIdService = deletedIdService;
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

        public class SimpleKey
        {
            public int IdOrder { get; set; }
            public int IdFurniture { get; set; }
        }
    }
}