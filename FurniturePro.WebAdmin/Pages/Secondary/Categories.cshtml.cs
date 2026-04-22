using FurniturePro.Core.Models.DTO.Categories; // Предполагаемые DTO
using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurniturePro.WebAdmin.Pages.Secondary
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

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostCreateCategoryAsync([FromForm] CreateCategoryDTO dto, CancellationToken ct)
        {
            try
            {
                // Вызываем метод создания напрямую из сервиса
                await _categoryService.CreateAsync(dto, ct); // Имя метода может отличаться (например, AddAsync)

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostUpdateCategoryAsync([FromForm] int id, [FromForm] UpdateCategoryDTO dto, CancellationToken ct)
        {
            try
            {
                // Вызываем метод обновления напрямую из сервиса
                await _categoryService.UpdateAsync(id, dto, ct); // Имя метода может отличаться

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostDeleteCategoryAsync([FromForm] int id, CancellationToken ct)
        {
            try
            {
                // Вызываем метод удаления
                await _categoryService.DeleteAsync(id, ct);

                // Логируем удаление через сервис deletedId
                var delId = new CreateDeletedIdDTO() { EntityId = id, TableName = "categories" };
                await _deletedIdService.CreateAsync(delId, ct); // Имя метода может отличаться

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }
    }
}