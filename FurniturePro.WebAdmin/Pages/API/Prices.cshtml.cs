using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Models.DTO.Orders;
using FurniturePro.Core.Models.DTO.Prices;
using FurniturePro.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurniturePro.WebAdmin.Pages.API
{
    public class PricesModel : PageModel
    {
        private readonly IPriceService _priceService;
        private readonly IDeletedIdService _deletedIdService;

        public PricesModel(IPriceService priceService, IDeletedIdService deletedIdService)
        {
            _priceService = priceService;
            _deletedIdService = deletedIdService;
        }

        public async Task<JsonResult> OnGetAsync(string dateTime, CancellationToken ct)
        {
            List<PriceDTO> items = new();
            List<DeletedIdDTO> deletedItems = new();

            try
            {
                items = await _priceService.GetAfterDateAsync(dateTime, ct);
                deletedItems = await _deletedIdService.GetAfterDateAsync(dateTime, "prices", ct);
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
