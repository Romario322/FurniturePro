// Orders.cshtml.cs
using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Models.DTO.OrderCompositions;
using FurniturePro.Core.Models.DTO.Orders; // Предполагаем наличие DTO
using FurniturePro.Core.Models.DTO.StatusChanges;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FurniturePro.WebAdmin.Pages.Main
{
    public class OrdersModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public OrdersModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
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
                // 1. Отправляем запрос на создание заказа
                var orderResponse = await _httpClient.PostAsJsonAsync("/api/orders", orderDto, ct);

                if (!orderResponse.IsSuccessStatusCode)
                {
                    return new JsonResult(new { success = false, message = "Ошибка API при создании заказа" });
                }

                // 2. Получаем ответ
                var jsonResponse = await orderResponse.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
                int newOrderId = 0;

                // ИСПРАВЛЕНИЕ: Проверяем тип ответа (Число или Объект)
                if (jsonResponse.ValueKind == JsonValueKind.Number)
                {
                    // Сценарий А: API вернул просто ID (например: 123)
                    newOrderId = jsonResponse.GetInt32();
                }
                else if (jsonResponse.ValueKind == JsonValueKind.Object)
                {
                    // Сценарий Б: API вернул объект заказа (например: {"id": 123, ...})
                    if (jsonResponse.TryGetProperty("id", out var idProp) ||
                        jsonResponse.TryGetProperty("Id", out idProp))
                    {
                        newOrderId = idProp.GetInt32();
                    }
                }

                // 3. Если ID получен, создаем начальный статус
                if (newOrderId > 0 && statusId > 0)
                {
                    var statusDto = new StatusChangeDTO
                    {
                        IdOrder = newOrderId,
                        IdStatus = statusId,
                        Date = date,
                        UpdateDate = DateTime.UtcNow
                    };

                    await _httpClient.PostAsJsonAsync("/api/statusChanges", statusDto, ct);
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
                // Редактирование заказа (в данном случае только скидки, но DTO может содержать и другое)
                var response = await _httpClient.PutAsJsonAsync($"/api/orders/{id}", dto, ct);
                if (response.IsSuccessStatusCode)
                {
                    return new JsonResult(new { success = true });
                }
                return new JsonResult(new { success = false, message = "Ошибка при обновлении заказа" });
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
                var response = await _httpClient.DeleteAsync($"/api/orders/{id}", ct);
                if (response.IsSuccessStatusCode)
                {
                    // Логируем удаление (опционально, как в Мебели)
                    var delId = new CreateDeletedIdDTO() { EntityId = id, TableName = "orders" };
                    await _httpClient.PostAsJsonAsync($"/api/deletedIds", delId, ct);

                    return new JsonResult(new { success = true });
                }
                return new JsonResult(new { success = false, message = "Ошибка при удалении заказа" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        // --- STATUS & COMPOSITION (Существующий код) ---

        public async Task<JsonResult> OnPostCreateStatusChangeAsync([FromForm] StatusChangeDTO dto, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/statusChanges", dto, ct);
                if (response.IsSuccessStatusCode) return new JsonResult(new { success = true });
                return new JsonResult(new { success = false, message = "Ошибка при создании статуса" });
            }
            catch (Exception ex) { return new JsonResult(new { success = false, message = ex.Message }); }
        }

        public async Task<JsonResult> OnPostCreateCompositionRangeAsync([FromBody] List<OrderCompositionDTO> dtos, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/orderCompositions/range", dtos, ct);
                if (response.IsSuccessStatusCode) return new JsonResult(new { success = true });
                return new JsonResult(new { success = false, message = "Ошибка API (Create)" });
            }
            catch (Exception ex) { return new JsonResult(new { success = false, message = ex.Message }); }
        }

        public async Task<JsonResult> OnPostUpdateCompositionRangeAsync([FromBody] List<OrderCompositionDTO> dtos, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("/api/orderCompositions/range", dtos, ct);
                if (response.IsSuccessStatusCode) return new JsonResult(new { success = true });
                return new JsonResult(new { success = false, message = "Ошибка API (Update)" });
            }
            catch (Exception ex) { return new JsonResult(new { success = false, message = ex.Message }); }
        }

        public async Task<JsonResult> OnPostDeleteCompositionRangeAsync([FromBody] List<SimpleKey> ids, CancellationToken ct)
        {
            try
            {
                var apiPayload = ids.Select(x => new { Item1 = x.IdOrder, Item2 = x.IdFurniture }).ToList();
                var request = new HttpRequestMessage(HttpMethod.Delete, "/api/orderCompositions/range");
                request.Content = JsonContent.Create(apiPayload);
                var response = await _httpClient.SendAsync(request, ct);

                if (response.IsSuccessStatusCode) return new JsonResult(new { success = true });
                return new JsonResult(new { success = false, message = "Ошибка API (Delete)" });
            }
            catch (Exception ex) { return new JsonResult(new { success = false, message = ex.Message }); }
        }

        public class SimpleKey
        {
            public int IdOrder { get; set; }
            public int IdFurniture { get; set; }
        }
    }
}