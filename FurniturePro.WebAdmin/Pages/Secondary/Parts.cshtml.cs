using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Models.DTO.Parts;
using FurniturePro.Core.Models.DTO.Prices;
using FurniturePro.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace FurniturePro.WebAdmin.Pages.Secondary
{
    public class PartsModel : PageModel
    {
        private readonly IPartService _partService;
        private readonly IDeletedIdService _deletedIdService;
        private readonly IPriceService _priceService;

        public PartsModel(IPartService partService, IDeletedIdService deletedIdService, IPriceService priceService)
        {
            _partService = partService;
            _deletedIdService = deletedIdService;
            _priceService = priceService;
        }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostCreatePartAsync([FromForm] CreatePartDTO dto, CancellationToken ct)
        {
            try
            {
                // Вызываем метод создания напрямую из сервиса
                await _partService.CreateAsync(dto, ct); // Имя метода может отличаться (например, AddAsync)

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }
        public async Task<JsonResult> OnPostCreatePriceAsync([FromForm] CreatePriceDTO dto, [FromForm] string value, CancellationToken ct)
        {
            try
            {
                dto.Value = decimal.Parse(value, CultureInfo.InvariantCulture);
                // Вызываем метод создания напрямую из сервиса
                await _priceService.CreateAsync(dto, ct); // Имя метода может отличаться (например, AddAsync)

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostUpdatePartAsync([FromForm] int id, [FromForm] UpdatePartDTO dto, CancellationToken ct)
        {
            try
            {
                // Вызываем метод обновления напрямую из сервиса
                await _partService.UpdateAsync(id, dto, ct); // Имя метода может отличаться

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostDeletePartAsync([FromForm] int id, CancellationToken ct)
        {
            try
            {
                // Вызываем метод удаления
                await _partService.DeleteAsync(id, ct);

                // Логируем удаление через сервис deletedId
                var delId = new CreateDeletedIdDTO() { EntityId = id, TableName = "parts" };
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
