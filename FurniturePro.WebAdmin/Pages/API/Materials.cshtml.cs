using FurniturePro.Core.Models.DTO.Colors;
using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Models.DTO.Materials;
using FurniturePro.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurniturePro.WebAdmin.Pages.API
{
    public class MaterialsModel : PageModel
    {
        private readonly IMaterialService _materialService;
        private readonly IDeletedIdService _deletedIdService;

        public MaterialsModel(IMaterialService materialService, IDeletedIdService deletedIdService)
        {
            _materialService = materialService;
            _deletedIdService = deletedIdService;
        }

        public async Task<JsonResult> OnGetAsync(string dateTime, CancellationToken ct)
        {
            List<MaterialDTO> items = new();
            List<DeletedIdDTO> deletedItems = new();

            try
            {
                items = await _materialService.GetAfterDateAsync(dateTime, ct);
                deletedItems = await _deletedIdService.GetAfterDateAsync(dateTime, "materials", ct);
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
