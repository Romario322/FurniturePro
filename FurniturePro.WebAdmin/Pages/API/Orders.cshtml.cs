using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Models.DTO.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurniturePro.WebAdmin.Pages.API
{
    public class OrdersModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public OrdersModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<JsonResult> OnGetAsync(string dateTime, CancellationToken ct)
        {
            List<OrderDTO> items = new();
            List<DeletedIdDTO> deletedItems = new();

            try
            {
                var itemsUrl = $"api/orders/after {dateTime}";
                var deletedUrl = $"api/deletedIds/after {dateTime} orders";

                var itemsTask = _httpClient.GetAsync(itemsUrl, ct);
                var deletedTask = _httpClient.GetAsync(deletedUrl, ct);

                await Task.WhenAll(itemsTask, deletedTask);

                var itemsResponse = await itemsTask;
                var deletedResponse = await deletedTask;

                if (itemsResponse.IsSuccessStatusCode)
                {
                    items = await itemsResponse.Content
                        .ReadFromJsonAsync<List<OrderDTO>>(cancellationToken: ct) ?? new();
                }

                if (deletedResponse.IsSuccessStatusCode)
                {
                    deletedItems = await deletedResponse.Content
                        .ReadFromJsonAsync<List<DeletedIdDTO>>(cancellationToken: ct) ?? new();
                }
            }
            catch (Exception ex)
            {

            }

            DateTime date1 = items.Count > 0
                ? items.Max(ent => ent.UpdateDate)
                : DateTime.MinValue;

            DateTime date2 = deletedItems.Count > 0
                ? deletedItems.Max(ent => ent.UpdateDate)
                : DateTime.MinValue;

            DateTime syncDate = date1 > date2 ? date1 : date2;

            return new JsonResult(new { items, deletedItems, syncDate });
        }
    }
}
