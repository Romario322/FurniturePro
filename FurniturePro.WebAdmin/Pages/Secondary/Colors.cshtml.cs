using ClosedXML.Excel;
using FurniturePro.Core.Models.DTO.Colors; // Предполагается наличие этих DTO
using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

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

        public async Task<IActionResult> OnPostImportAsync(IFormFile excelFile, [FromForm] string existingNamesStr, CancellationToken ct)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                return new JsonResult(new { success = false, message = "Файл не выбран или пуст." });
            }

            var colorsToCreate = new List<CreateColorDTO>();

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

                    colorsToCreate.Add(new CreateColorDTO
                    {
                        Name = name,
                        Description = description
                    });
                }
            }

            if (!colorsToCreate.Any())
            {
                return new JsonResult(new { success = false, message = "В файле нет новых записей для добавления. Все данные уже существуют." });
            }

            await _colorService.CreateRangeAsync(colorsToCreate, ct);

            return new JsonResult(new { success = true, count = colorsToCreate.Count });
        }
    }
}