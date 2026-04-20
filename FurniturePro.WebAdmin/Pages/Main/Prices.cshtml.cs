using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Models.DTO.Prices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace FurniturePro.WebAdmin.Pages.Main
{
    public class PricesModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public PricesModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }
        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostUpdatePriceAsync([FromForm] int id, [FromForm] UpdatePriceDTO dto, [FromForm] string value, CancellationToken ct)
        {
            try
            {
                dto.Value = decimal.Parse(value, CultureInfo.InvariantCulture);
                var response = await _httpClient.PutAsJsonAsync($"/api/prices/{id}", dto, ct);
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

        public async Task<JsonResult> OnPostDeletePriceAsync([FromForm] int id, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/prices/{id}", ct);
                if (response.IsSuccessStatusCode)
                {
                    var delId = new CreateDeletedIdDTO() { EntityId = id, TableName = "prices" };
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
