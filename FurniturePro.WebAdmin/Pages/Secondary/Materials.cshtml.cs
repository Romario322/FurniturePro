using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Models.DTO.Materials;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurniturePro.WebAdmin.Pages.Secondary
{
    public class MaterialsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public MaterialsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostCreateMaterialAsync([FromForm] CreateMaterialDTO dto, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/materials", dto, ct);
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

        public async Task<JsonResult> OnPostUpdateMaterialAsync([FromForm] int id, [FromForm] UpdateMaterialDTO dto, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/materials/{id}", dto, ct);
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

        public async Task<JsonResult> OnPostDeleteMaterialAsync([FromForm] int id, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/materials/{id}", ct);
                if (response.IsSuccessStatusCode)
                {
                    // Logging the deletion for the frontend sync system
                    var delId = new CreateDeletedIdDTO() { EntityId = id, TableName = "materials" };
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