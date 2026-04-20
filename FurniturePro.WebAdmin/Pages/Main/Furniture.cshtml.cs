using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Models.DTO.FurnitureCompositions;
using FurniturePro.Core.Models.DTO.Furnitures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurniturePro.WebAdmin.Pages.Main
{
    public class FurnitureModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public FurnitureModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }
        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostCreateFurnitureAsync([FromForm] CreateFurnitureDTO dto, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/furniture", dto, ct);
                if (response.IsSuccessStatusCode)
                {
                    return new JsonResult(new { success = true });
                }
                return new JsonResult(new { success = false, message = "Ошибка при создании" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostUpdateFurnitureAsync([FromForm] int id, [FromForm] UpdateFurnitureDTO dto, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/furniture/{id}", dto, ct);
                if (response.IsSuccessStatusCode)
                {
                    return new JsonResult(new { success = true });
                }
                return new JsonResult(new { success = false, message = "Ошибка при изменении" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostDeleteFurnitureAsync([FromForm] int id, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/furniture/{id}", ct);
                if (response.IsSuccessStatusCode)
                {
                    var delId = new CreateDeletedIdDTO() { EntityId = id, TableName = "furniture" };
                    response = await _httpClient.PostAsJsonAsync($"/api/deletedIds", delId, ct);
                    if (response.IsSuccessStatusCode)
                    {
                        return new JsonResult(new { success = true });
                    }
                    return new JsonResult(new { success = false, message = "Ошибка при логировании удаления" });
                }
                return new JsonResult(new { success = false, message = "Ошибка при удалении" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostCreateCompositionRangeAsync([FromBody] List<FurnitureCompositionDTO> dtos, CancellationToken ct)
        {
            try
            {
                // dtos теперь придут корректно, так как имена в JSON совпадут с классом
                var response = await _httpClient.PostAsJsonAsync("/api/furnitureCompositions/range", dtos, ct);
                if (response.IsSuccessStatusCode) return new JsonResult(new { success = true });
                return new JsonResult(new { success = false, message = "Ошибка API (Create)" });
            }
            catch (Exception ex) { return new JsonResult(new { success = false, message = ex.Message }); }
        }

        // 2. Обновление
        public async Task<JsonResult> OnPostUpdateCompositionRangeAsync([FromBody] List<FurnitureCompositionDTO> dtos, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("/api/furnitureCompositions/range", dtos, ct);
                if (response.IsSuccessStatusCode) return new JsonResult(new { success = true });
                return new JsonResult(new { success = false, message = "Ошибка API (Update)" });
            }
            catch (Exception ex) { return new JsonResult(new { success = false, message = ex.Message }); }
        }

        // 3. Удаление
        public async Task<JsonResult> OnPostDeleteCompositionRangeAsync([FromBody] List<SimpleKey> ids, CancellationToken ct)
        {
            try
            {
                // Преобразуем входящие IdFurniture/IdPart в формат кортежей (Item1, Item2) для API
                var apiPayload = ids.Select(x => new { Item1 = x.IdFurniture, Item2 = x.IdPart }).ToList();

                var request = new HttpRequestMessage(HttpMethod.Delete, "/api/furnitureCompositions/range");
                request.Content = JsonContent.Create(apiPayload);

                var response = await _httpClient.SendAsync(request, ct);

                if (response.IsSuccessStatusCode)
                {
                    return new JsonResult(new { success = true });
                }
                return new JsonResult(new { success = false, message = "Ошибка API (Delete)" });
            }
            catch (Exception ex) { return new JsonResult(new { success = false, message = ex.Message }); }
        }

        // Вспомогательный класс с правильными именами
        public class SimpleKey
        {
            public int IdFurniture { get; set; } // Было FurnitureId
            public int IdPart { get; set; }      // Было PartId
        }
    }
}
