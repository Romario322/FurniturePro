using FurniturePro.Core.Models.DTO.Categories; // Предполагаемые DTO
using FurniturePro.Core.Models.DTO.DeletedIds;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurniturePro.WebAdmin.Pages.Secondary
{
    public class CategoriesModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CategoriesModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostCreateCategoryAsync([FromForm] CreateCategoryDTO dto, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/categories", dto, ct);
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

        public async Task<JsonResult> OnPostUpdateCategoryAsync([FromForm] int id, [FromForm] UpdateCategoryDTO dto, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/categories/{id}", dto, ct);
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

        public async Task<JsonResult> OnPostDeleteCategoryAsync([FromForm] int id, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/categories/{id}", ct);
                if (response.IsSuccessStatusCode)
                {
                    // Логируем удаление
                    var delId = new CreateDeletedIdDTO() { EntityId = id, TableName = "categories" };
                    await _httpClient.PostAsJsonAsync($"/api/deletedIds", delId, ct);

                    return new JsonResult(new { success = true });
                }
                return new JsonResult(new { success = false, message = "Ошибка при удалении" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }
    }
}