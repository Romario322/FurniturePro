using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Models.DTO.Parts;
using FurniturePro.Core.Models.DTO.StatusChanges;
using FurniturePro.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurniturePro.WebAdmin.Pages.API
{
    public class StatusChangesModel : PageModel
    {
        private readonly IStatusChangeService _statusChangeService;
        private readonly IDeletedIdService _deletedIdService;

        public StatusChangesModel(IStatusChangeService statusChangeService, IDeletedIdService deletedIdService)
        {
            _statusChangeService = statusChangeService;
            _deletedIdService = deletedIdService;
        }

        public async Task<JsonResult> OnGetAsync(string dateTime, CancellationToken ct)
        {
            List<StatusChangeDTO> items = new();
            List<DeletedIdDTO> deletedItems = new();

            try
            {
                items = await _statusChangeService.GetAfterDateAsync(dateTime, ct);
                deletedItems = await _deletedIdService.GetAfterDateAsync(dateTime, "statusChanges", ct);
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