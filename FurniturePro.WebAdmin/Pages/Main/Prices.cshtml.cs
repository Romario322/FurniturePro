using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Models.DTO.Prices;
using FurniturePro.Core.Services.Interfaces; // Добавляем using для сервисов
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace FurniturePro.WebAdmin.Pages.Main
{
    public class PricesModel : PageModel
    {
        private readonly IPriceService _priceService;
        private readonly IDeletedIdService _deletedIdService;

        // Внедряем интерфейсы сервисов вместо IHttpClientFactory
        public PricesModel(IPriceService priceService, IDeletedIdService deletedIdService)
        {
            _priceService = priceService;
            _deletedIdService = deletedIdService;
        }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostUpdatePriceAsync([FromForm] int id, [FromForm] UpdatePriceDTO dto, [FromForm] string value, CancellationToken ct)
        {
            try
            {
                // Оставляем парсинг значения
                dto.Value = decimal.Parse(value, CultureInfo.InvariantCulture);

                // Вызываем метод обновления напрямую из сервиса
                await _priceService.UpdateAsync(id, dto, ct);

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostDeletePriceAsync([FromForm] int id, CancellationToken ct)
        {
            try
            {
                // 1. Удаляем цену
                await _priceService.DeleteAsync(id, ct);

                // 2. Логируем удаление
                var delId = new CreateDeletedIdDTO() { EntityId = id, TableName = "prices" };
                await _deletedIdService.CreateAsync(delId, ct);

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }
    }
}