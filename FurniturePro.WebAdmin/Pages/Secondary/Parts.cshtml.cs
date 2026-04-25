using ClosedXML.Excel;
using FurniturePro.Core.Models.DTO.Colors;
using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Models.DTO.Materials;
using FurniturePro.Core.Models.DTO.Parts;
using FurniturePro.Core.Models.DTO.Prices;
using FurniturePro.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FurniturePro.WebAdmin.Pages.Secondary
{
    public class PartsModel : PageModel
    {
        private readonly IPartService _partService;
        private readonly IDeletedIdService _deletedIdService;
        private readonly IPriceService _priceService;
        private readonly IColorService _colorService;
        private readonly IMaterialService _materialService;

        public PartsModel(IPartService partService, IDeletedIdService deletedIdService, IPriceService priceService,
            IColorService colorService, IMaterialService materialService)
        {
            _partService = partService;
            _deletedIdService = deletedIdService;
            _priceService = priceService;
            _colorService = colorService;
            _materialService = materialService;
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

        public async Task<IActionResult> OnPostImportAsync(
            IFormFile excelFile,
            [FromForm] string existingPartsStr,
            [FromForm] string materialsCacheStr,
            [FromForm] string colorsCacheStr,
            CancellationToken ct)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                return new JsonResult(new { success = false, message = "Файл не выбран или пуст." });
            }

            // Десериализация кэшей (ожидается формат словаря: { "Дерево": 1, "Металл": 2 })
            var materialsCache = DeserializeCache(materialsCacheStr);
            var colorsCache = DeserializeCache(colorsCacheStr);

            var existingPartsList = string.IsNullOrEmpty(existingPartsStr) ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(existingPartsStr);
            var existingNames = new HashSet<string>(existingPartsList ?? new List<string>(), StringComparer.OrdinalIgnoreCase);

            int createdCount = 0;

            using (var stream = excelFile.OpenReadStream())
            using (var workbook = new XLWorkbook(stream))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Пропуск заголовков

                foreach (var row in rows)
                {
                    var name = row.Cell(1).GetString().Trim();

                    // Если имени нет или такая деталь уже есть в базе — пропускаем
                    if (string.IsNullOrEmpty(name) || existingNames.Contains(name))
                        continue;

                    // --- Проверка Активности ---
                    var activityStr = row.Cell(8).GetString().Trim().ToLower();
                    bool isActive = activityStr == "да" || activityStr == "1" || activityStr == "true" || activityStr == "+" || activityStr == "активен" || activityStr == "";

                    // Создаем записи только если они активны
                    if (!isActive) continue;

                    var description = row.Cell(2).GetString().Trim();
                    var colorName = row.Cell(3).GetString().Trim();
                    var materialName = row.Cell(4).GetString().Trim();
                    var dimensionsStr = row.Cell(5).GetString().Trim().ToLower();
                    var weightStr = new string(row.Cell(6).GetString().Trim().Where(char.IsDigit).ToArray());
                    var priceStr = new string(row.Cell(7).GetString().Trim().Where(char.IsDigit).ToArray());

                    // --- Парсинг габаритов ---
                    int? length = null, width = null, height = null, diameter = null;

                    if (!string.IsNullOrEmpty(dimensionsStr))
                    {
                        if (dimensionsStr.Contains("ø"))
                        {
                            var dimParts = dimensionsStr.Split('ø');
                            if (dimParts.Length >= 2)
                            {
                                length = ExtractIntFromStr(dimParts[0]);
                                diameter = ExtractIntFromStr(dimParts[1]);
                            }
                        }
                        else if (dimensionsStr.Contains("x") || dimensionsStr.Contains("х")) // Английский 'x' и русский 'х'
                        {
                            var dimParts = dimensionsStr.Split(new char[] { 'x', 'х' }, StringSplitOptions.RemoveEmptyEntries);
                            if (dimParts.Length >= 3)
                            {
                                length = ExtractIntFromStr(dimParts[0]);
                                width = ExtractIntFromStr(dimParts[1]);
                                height = ExtractIntFromStr(dimParts[2]);
                            }
                        }
                        else
                        {
                            length = ExtractIntFromStr(dimensionsStr);
                        }
                    }

                    // --- Поиск Id в кэше ---
                    int colorId = colorsCache.TryGetValue(colorName, out int cid) ? cid : -1;
                    if (colorId == -1)
                    {
                        var colorDto = new CreateColorDTO { Name = colorName };
                        colorId = await _colorService.CreateAsync(colorDto, ct);
                        colorsCache.Add(colorName, colorId);
                    }
                    int materialId = materialsCache.TryGetValue(materialName, out int mid) ? mid : -1;
                    if (materialId == -1)
                    {
                        var materialDto = new CreateMaterialDTO { Name = materialName };
                        materialId = await _materialService.CreateAsync(materialDto, ct);
                        Console.Write(materialsCache.TryAdd(materialName, materialId));
                    }

                    decimal? weight = decimal.TryParse(weightStr.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal w) ? w : null;

                    // Создание DTO детали
                    var partDto = new CreatePartDTO
                    {
                        Name = name,
                        Description = string.IsNullOrEmpty(description) ? null : description,
                        ColorId = colorId,
                        MaterialId = materialId,
                        Length = length,
                        Width = width,
                        Height = height,
                        Diameter = diameter,
                        Weight = weight,
                        Activity = true // Так как мы уже отфильтровали активные выше
                    };

                    // Создаем деталь (метод CreateAsync возвращает сгенерированный ID)
                    int newPartId = await _partService.CreateAsync(partDto, ct);
                    existingNames.Add(name);
                    createdCount++;

                    // --- Создание цены ---
                    if (!string.IsNullOrEmpty(priceStr) && decimal.TryParse(priceStr.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal priceValue))
                    {
                        var priceDto = new CreatePriceDTO
                        {
                            PartId = newPartId,
                            Value = priceValue,
                            Date = DateTime.UtcNow // Или использовать дату из файла, если есть
                        };

                        await _priceService.CreateAsync(priceDto, ct);
                    }
                }
            }

            if (createdCount == 0)
            {
                return new JsonResult(new { success = false, message = "В файле нет новых активных записей для добавления или все данные уже существуют." });
            }

            return new JsonResult(new { success = true, count = createdCount });
        }

        // Вспомогательный метод для извлечения только цифр из строки (например, "100 мм" -> 100)
        private int? ExtractIntFromStr(string input)
        {
            var match = Regex.Match(input, @"\d+");
            return match.Success ? int.Parse(match.Value) : null;
        }

        // Вспомогательный метод для безопасной десериализации словарей кэша (Игнорирует регистр символов)
        private Dictionary<string, int> DeserializeCache(string json)
        {
            if (string.IsNullOrEmpty(json))
                return new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            try
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, int>>(json);
                return dict == null
                    ? new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
                    : new Dictionary<string, int>(dict, StringComparer.OrdinalIgnoreCase);
            }
            catch
            {
                return new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            }
        }
    }
}
