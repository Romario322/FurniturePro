using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.DeletedIds;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FurniturePro.Core.Services.Interfaces;

namespace FurniturePro.WebAdmin.Pages.API
{
    public class CategoriesModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IDeletedIdService _deletedIdService;

        public CategoriesModel(ICategoryService categoryService, IDeletedIdService deletedIdService)
        {
            _categoryService = categoryService;
            _deletedIdService = deletedIdService;
        }

        public async Task<JsonResult> OnGetAsync(string dateTime, CancellationToken ct)
        {
            List<CategoryDTO> items = new();
            List<DeletedIdDTO> deletedItems = new();

            try
            {
                items = await _categoryService.GetAfterDateAsync(dateTime, ct);
                deletedItems = await _deletedIdService.GetAfterDateAsync(dateTime, "categories", ct);
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
