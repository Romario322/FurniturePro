using ClosedXML.Excel;
using FurniturePro.Core.Models.DTO.Colors;
using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Models.DTO.Materials;
using FurniturePro.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

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

        public async Task<IActionResult> OnPostImportAsync(IFormFile excelFile, [FromForm] string existingNamesStr, CancellationToken ct)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                return new JsonResult(new { success = false, message = "Файл не выбран или пуст." });
            }

            var materialsToCreate = new List<CreateMaterialDTO>();

            var existingNamesList = string.IsNullOrEmpty(existingNamesStr) ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(existingNamesStr);

            var existingNames = new HashSet<string>(existingNamesList, StringComparer.OrdinalIgnoreCase);

            using (var stream = excelFile.OpenReadStream())
            using (var workbook = new XLWorkbook(stream))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

                foreach (var row in rows)
                {
                    var name = row.Cell(1).GetString().Trim();
                    var description = row.Cell(2).GetString().Trim();

                    if (string.IsNullOrEmpty(name) || existingNames.Contains(name))
                        continue;

                    existingNames.Add(name);

                    materialsToCreate.Add(new CreateMaterialDTO
                    {
                        Name = name,
                        Description = description
                    });
                }
            }

            if (!materialsToCreate.Any())
            {
                return new JsonResult(new { success = false, message = "В файле нет новых записей для добавления. Все данные уже существуют." });
            }

            await _materialService.CreateRangeAsync(materialsToCreate, ct);

            return new JsonResult(new { success = true, count = materialsToCreate.Count });
        }
    }
}