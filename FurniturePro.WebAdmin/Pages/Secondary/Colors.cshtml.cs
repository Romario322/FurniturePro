using FurniturePro.Core.Models.DTO.Colors; // Предполагается наличие этих DTO
using FurniturePro.Core.Models.DTO.DeletedIds;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurniturePro.WebAdmin.Pages.Secondary
{
    public class ColorsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public ColorsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostCreateColorAsync([FromForm] CreateColorDTO dto, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/colors", dto, ct);
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

        public async Task<JsonResult> OnPostUpdateColorAsync([FromForm] int id, [FromForm] UpdateColorDTO dto, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/colors/{id}", dto, ct);
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

        public async Task<JsonResult> OnPostDeleteColorAsync([FromForm] int id, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/colors/{id}", ct);
                if (response.IsSuccessStatusCode)
                {
                    var delId = new CreateDeletedIdDTO() { EntityId = id, TableName = "colors" };
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