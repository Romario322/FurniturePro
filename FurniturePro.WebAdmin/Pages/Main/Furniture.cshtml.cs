using ClosedXML.Excel;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Colors;
using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Models.DTO.FurnitureCompositions;
using FurniturePro.Core.Models.DTO.Furnitures;
using FurniturePro.Core.Models.DTO.Materials;
using FurniturePro.Core.Models.DTO.Parts;
using FurniturePro.Core.Models.DTO.Prices;
using FurniturePro.Core.Services;
using FurniturePro.Core.Services.Interfaces; // Обязательно добавляем using для интерфейсов
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FurniturePro.WebAdmin.Pages.Main
{
    public class FurnitureModel : PageModel
    {
        private readonly IFurnitureService _furnitureService;
        private readonly IFurnitureCompositionService _furnitureCompositionService;
        private readonly IDeletedIdService _deletedIdService;
        private readonly ICategoryService _categoryService;

        // Внедряем три необходимых сервиса вместо IHttpClientFactory
        public FurnitureModel(IFurnitureService furnitureService, IFurnitureCompositionService furnitureCompositionService, 
            IDeletedIdService deletedIdService, ICategoryService categoryService)
        {
            _furnitureService = furnitureService;
            _furnitureCompositionService = furnitureCompositionService;
            _deletedIdService = deletedIdService;
            _categoryService = categoryService;
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

        public class ImportFurnitureItem
        {
            public string Name { get; set; }
            public string Category { get; set; }
            public string Markup { get; set; } // В формате строки, т.к. может содержать "%"
            public string Description { get; set; }
            public string Composition { get; set; } // Строка состава "Деталь - 1, Деталь2 - 2"
        }

        public async Task<JsonResult> OnPostImportFurnitureAsync(
            IFormFile excelFile,
            [FromForm] string existingFurnitureStr,
            [FromForm] string categoriesCacheStr,
            [FromForm] string partsCacheStr,
            CancellationToken ct)
        {

            if (excelFile == null || excelFile.Length == 0)
            {
                return new JsonResult(new { success = false, message = "Файл не выбран или пуст." });
            }

            // Десериализация кэшей (ожидается формат словаря: { "Дерево": 1, "Металл": 2 })
            var categoriesCache = DeserializeCache(categoriesCacheStr);
            var partsCache = DeserializeCache(partsCacheStr);

            var existingFurnitureList = string.IsNullOrEmpty(existingFurnitureStr) ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(existingFurnitureStr);
            var existingNames = new HashSet<string>(existingFurnitureList ?? new List<string>(), StringComparer.OrdinalIgnoreCase);

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
                    var activityStr = row.Cell(6).GetString().Trim().ToLower();
                    bool isActive = activityStr == "да" || activityStr == "1" || activityStr == "true" || activityStr == "+" || activityStr == "активен" || activityStr == "";

                    // Создаем записи только если они активны
                    if (!isActive) continue;

                    var description = row.Cell(2).GetString().Trim();
                    var markupStr = new string(row.Cell(3).GetString().Trim().Where(char.IsDigit).ToArray());
                    var categoryName = row.Cell(4).GetString().Trim();
                    var compositionStr = row.Cell(5).GetString().Trim();

                    // --- Поиск Id в кэше ---
                    int categoryId = categoriesCache.TryGetValue(categoryName, out int cid) ? cid : -1;
                    if (categoryId == -1)
                    {
                        var colorDto = new CreateCategoryDTO { Name = categoryName };
                        categoryId = await _categoryService.CreateAsync(colorDto, ct);
                        categoriesCache.Add(categoryName, categoryId);
                    }

                    int? markup = int.TryParse(markupStr, NumberStyles.Any, CultureInfo.InvariantCulture, out int w) ? w : null;

                    // Создание DTO детали
                    var furnitureDto = new CreateFurnitureDTO
                    {
                        Name = name,
                        Description = string.IsNullOrEmpty(description) ? null : description,
                        Markup = markup,
                        CategoryId = categoryId,
                        Activity = true // Так как мы уже отфильтровали активные выше
                    };

                    bool skipFurniture = false;
                    var compositionList = compositionStr.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                    var compositionsToCreate = new List<FurnitureCompositionDTO>();
                    foreach (var composition in compositionList)
                    {
                        var nameAndCount = composition.Split('-');
                        if (nameAndCount.Length >= 2)
                        {
                            var partName = nameAndCount[0].Trim();
                            var partCountStr = nameAndCount[1].Trim();

                            // Ищем деталь в кеше
                            int partId = partsCache.TryGetValue(partName, out int pid) ? pid : -1;

                            if (partId == -1)
                            {
                                skipFurniture = true; // Деталь отсутствует в кеше -> пропускаем эту мебель
                                break;
                            }

                            if (int.TryParse(partCountStr, out int count))
                            {
                                var compositionDto = new FurnitureCompositionDTO()
                                {
                                    Count = count,
                                    IdPart = partId,
                                    IdFurniture = -1,
                                    UpdateDate = DateTime.Now
                                };
                                compositionsToCreate.Add(compositionDto);
                            }
                        }
                    }
                    if (skipFurniture)
                        continue;
                    // Создаем деталь (метод CreateAsync возвращает сгенерированный ID)
                    int newfurnitureId = await _furnitureService.CreateAsync(furnitureDto, ct);
                    existingNames.Add(name);
                    createdCount++;

                    compositionsToCreate.ForEach(composition => composition.IdFurniture = newfurnitureId);
                    await _furnitureCompositionService.CreateRangeAsync(compositionsToCreate, ct);
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

        // Вспомогательный класс с правильными именами
        public class SimpleKey
        {
            public int IdFurniture { get; set; } // Было FurnitureId
            public int IdPart { get; set; }      // Было PartId
        }
    }
}