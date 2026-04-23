using ClosedXML.Excel;
using FurniturePro.Core.Models.DTO.Categories; // Предполагаемые DTO
using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

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

        public async Task<IActionResult> OnPostImportAsync(IFormFile excelFile, [FromForm] string existingNamesStr, CancellationToken ct)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                return new JsonResult(new { success = false, message = "Файл не выбран или пуст." });
            }

            var categoriesToCreate = new List<CreateCategoryDTO>();

            var existingNamesList = string.IsNullOrEmpty(existingNamesStr) ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(existingNamesStr);

            var existingNames = new HashSet<string>(existingNamesList, StringComparer.OrdinalIgnoreCase);

            // Открываем поток файла и передаем его в парсер ClosedXML
            using (var stream = excelFile.OpenReadStream())
            using (var workbook = new XLWorkbook(stream))
            {
                var worksheet = workbook.Worksheet(1); // Берем 1-й лист
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Пропускаем строку с заголовками

                foreach (var row in rows)
                {
                    var name = row.Cell(1).GetString().Trim();
                    var description = row.Cell(2).GetString().Trim();

                    if (string.IsNullOrEmpty(name))
                        continue;

                    // 2. Проверяем, существует ли уже такая категория в кеше
                    if (existingNames.Contains(name))
                        continue;

                    // 3. Добавляем имя в HashSet, чтобы избежать дубликатов внутри самого Excel файла
                    existingNames.Add(name);

                    categoriesToCreate.Add(new CreateCategoryDTO
                    {
                        Name = name,
                        Description = description
                    });
                }
            }

            // 4. Информируем пользователя, если новых записей не найдено
            if (!categoriesToCreate.Any())
            {
                return new JsonResult(new { success = false, message = "В файле нет новых записей для добавления. Все данные уже существуют." });
            }

            // Передаем готовый List<DTO> в слой бизнес-логики (Core)
            await _categoryService.CreateRangeAsync(categoriesToCreate, ct);

            // Возвращаем JSON-ответ для fetch, чтобы JS понял, что все прошло успешно
            return new JsonResult(new { success = true, count = categoriesToCreate.Count });
        }
    }
}