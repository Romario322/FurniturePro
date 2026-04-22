using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Models.DTO.FurnitureCompositions;
using FurniturePro.Core.Models.DTO.Furnitures;
using FurniturePro.Core.Services.Interfaces; // Обязательно добавляем using для интерфейсов
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurniturePro.WebAdmin.Pages.Main
{
    public class FurnitureModel : PageModel
    {
        private readonly IFurnitureService _furnitureService;
        private readonly IFurnitureCompositionService _furnitureCompositionService;
        private readonly IDeletedIdService _deletedIdService;

        // Внедряем три необходимых сервиса вместо IHttpClientFactory
        public FurnitureModel(IFurnitureService furnitureService, IFurnitureCompositionService furnitureCompositionService, 
            IDeletedIdService deletedIdService)
        {
            _furnitureService = furnitureService;
            _furnitureCompositionService = furnitureCompositionService;
            _deletedIdService = deletedIdService;
        }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostCreateFurnitureAsync([FromForm] CreateFurnitureDTO dto, CancellationToken ct)
        {
            try
            {
                await _furnitureService.CreateAsync(dto, ct);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostUpdateFurnitureAsync([FromForm] int id, [FromForm] UpdateFurnitureDTO dto, CancellationToken ct)
        {
            try
            {
                await _furnitureService.UpdateAsync(id, dto, ct);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostDeleteFurnitureAsync([FromForm] int id, CancellationToken ct)
        {
            try
            {
                // 1. Удаляем мебель
                await _furnitureService.DeleteAsync(id, ct);

                // 2. Логируем удаление
                var delId = new CreateDeletedIdDTO() { EntityId = id, TableName = "furniture" };
                await _deletedIdService.CreateAsync(delId, ct);

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostCreateCompositionRangeAsync([FromBody] List<FurnitureCompositionDTO> dtos, CancellationToken ct)
        {
            try
            {
                await _furnitureCompositionService.CreateRangeAsync(dtos, ct);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        // 2. Обновление
        public async Task<JsonResult> OnPostUpdateCompositionRangeAsync([FromBody] List<FurnitureCompositionDTO> dtos, CancellationToken ct)
        {
            try
            {
                await _furnitureCompositionService.UpdateRangeAsync(dtos, ct);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        // 3. Удаление
        public async Task<JsonResult> OnPostDeleteCompositionRangeAsync([FromBody] List<SimpleKey> ids, CancellationToken ct)
        {
            try
            {
                // Преобразуем входящие IdFurniture/IdPart в список кортежей (Item1, Item2), который принимает сервис
                var servicePayload = ids.Select(x => (x.IdFurniture, x.IdPart)).ToList();

                // Вызываем метод сервиса напрямую
                await _furnitureCompositionService.DeleteRangeAsync(servicePayload, ct);

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        // Вспомогательный класс с правильными именами
        public class SimpleKey
        {
            public int IdFurniture { get; set; } // Было FurnitureId
            public int IdPart { get; set; }      // Было PartId
        }
    }
}