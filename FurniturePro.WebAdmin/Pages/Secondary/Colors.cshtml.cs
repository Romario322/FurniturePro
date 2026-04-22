using FurniturePro.Core.Models.DTO.Colors; // Предполагается наличие этих DTO
using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurniturePro.WebAdmin.Pages.Secondary
{
    public class ColorsModel : PageModel
    {
        private readonly IColorService _colorService;
        private readonly IDeletedIdService _deletedIdService;

        public ColorsModel(IColorService colorService, IDeletedIdService deletedIdService)
        {
            _colorService = colorService;
            _deletedIdService = deletedIdService;
        }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostCreateColorAsync([FromForm] CreateColorDTO dto, CancellationToken ct)
        {
            try
            {
                // Вызываем метод создания напрямую из сервиса
                await _colorService.CreateAsync(dto, ct); // Имя метода может отличаться (например, AddAsync)

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostUpdateColorAsync([FromForm] int id, [FromForm] UpdateColorDTO dto, CancellationToken ct)
        {
            try
            {
                // Вызываем метод обновления напрямую из сервиса
                await _colorService.UpdateAsync(id, dto, ct); // Имя метода может отличаться

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostDeleteColorAsync([FromForm] int id, CancellationToken ct)
        {
            try
            {
                // Вызываем метод удаления
                await _colorService.DeleteAsync(id, ct);

                // Логируем удаление через сервис deletedId
                var delId = new CreateDeletedIdDTO() { EntityId = id, TableName = "colors" };
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