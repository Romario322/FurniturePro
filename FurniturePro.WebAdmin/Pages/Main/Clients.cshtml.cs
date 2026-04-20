using FurniturePro.Core.Models.DTO.Clients;
using FurniturePro.Core.Models.DTO.DeletedIds;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurniturePro.WebAdmin.Pages.Main
{
    public class ClientsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public ClientsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostCreateClientAsync([FromForm] CreateClientDTO dto, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/clients", dto, ct);
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

        public async Task<JsonResult> OnPostUpdateClientAsync([FromForm] int id, [FromForm] UpdateClientDTO dto, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/clients/{id}", dto, ct);
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

        public async Task<JsonResult> OnPostDeleteClientAsync([FromForm] int id, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/clients/{id}", ct);
                if (response.IsSuccessStatusCode)
                {
                    var delId = new CreateDeletedIdDTO() { EntityId = id, TableName = "clients" };
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