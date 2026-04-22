using FurniturePro.Core.Models.DTO.Colors;
using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Models.DTO.Materials;
using FurniturePro.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurniturePro.WebAdmin.Pages.Secondary
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

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostCreateMaterialAsync([FromForm] CreateMaterialDTO dto, CancellationToken ct)
        {
            try
            {
                // Вызываем метод создания напрямую из сервиса
                await _materialService.CreateAsync(dto, ct); // Имя метода может отличаться (например, AddAsync)

                return new JsonResult(new { success = true });
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
                // Вызываем метод обновления напрямую из сервиса
                await _materialService.UpdateAsync(id, dto, ct); // Имя метода может отличаться

                return new JsonResult(new { success = true });
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
                // Вызываем метод удаления
                await _materialService.DeleteAsync(id, ct);

                // Логируем удаление через сервис deletedId
                var delId = new CreateDeletedIdDTO() { EntityId = id, TableName = "materials" };
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