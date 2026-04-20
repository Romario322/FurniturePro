using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Models.DTO.Parts;
using FurniturePro.Core.Models.DTO.Prices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace FurniturePro.WebAdmin.Pages.Secondary
{
    public class PartsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public PartsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }
        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostCreatePartAsync([FromForm] CreatePartDTO dto, [FromForm] string weight, CancellationToken ct)
        {
            try
            {
                dto.Weight = decimal.Parse(weight, CultureInfo.InvariantCulture);
                var response = await _httpClient.PostAsJsonAsync("/api/parts", dto, ct);
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

        public async Task<JsonResult> OnPostCreatePriceAsync([FromForm] CreatePriceDTO dto, [FromForm] string value, CancellationToken ct)
        {
            try
            {
                dto.Value = decimal.Parse(value, CultureInfo.InvariantCulture);
                var response = await _httpClient.PostAsJsonAsync("/api/prices", dto, ct);
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

        public async Task<JsonResult> OnPostUpdatePartAsync([FromForm] int id, [FromForm] UpdatePartDTO dto, [FromForm] string weight, CancellationToken ct)
        {
            try
            {
                dto.Weight = decimal.Parse(weight, CultureInfo.InvariantCulture);
                var response = await _httpClient.PutAsJsonAsync($"/api/parts/{id}", dto, ct);
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

        public async Task<JsonResult> OnPostDeletePartAsync([FromForm] int id, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/parts/{id}", ct);
                if (response.IsSuccessStatusCode)
                {
                    var delId = new CreateDeletedIdDTO() { EntityId = id, TableName = "parts" };
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
    }
}
