using ClosedXML.Excel;
using FurniturePro.Core.Models.DTO.Clients;
using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FurniturePro.WebAdmin.Pages.Main
{
    public class ClientsModel : PageModel
    {
        private readonly IClientService _clientService;
        private readonly IDeletedIdService _deletedIdService;

        public ClientsModel(IClientService clientService, IDeletedIdService deletedIdService)
        {
            _clientService = clientService;
            _deletedIdService = deletedIdService;
        }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostCreateClientAsync([FromForm] CreateClientDTO dto, CancellationToken ct)
        {
            try
            {
                // Вызываем метод создания напрямую из сервиса
                await _clientService.CreateAsync(dto, ct); // Имя метода может отличаться (например, AddAsync)

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostUpdateClientAsync([FromForm] int id, [FromForm] UpdateClientDTO dto, CancellationToken ct)
        {
            try
            {
                // Вызываем метод обновления напрямую из сервиса
                await _clientService.UpdateAsync(id, dto, ct); // Имя метода может отличаться

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<JsonResult> OnPostDeleteClientAsync([FromForm] int id, CancellationToken ct)
        {
            try
            {
                // Вызываем метод удаления
                await _clientService.DeleteAsync(id, ct);

                // Логируем удаление через сервис deletedId
                var delId = new CreateDeletedIdDTO() { EntityId = id, TableName = "clients" };
                await _deletedIdService.CreateAsync(delId, ct); // Имя метода может отличаться

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }

        public async Task<IActionResult> OnPostImportAsync(IFormFile excelFile, [FromForm] string existingPhonesStr, CancellationToken ct)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                return new JsonResult(new { success = false, message = "Файл не выбран или пуст." });
            }

            var clientsToCreate = new List<CreateClientDTO>();

            // Получаем телефоны существующих клиентов для предотвращения дубликатов
            var existingPhonesList = string.IsNullOrEmpty(existingPhonesStr) ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(existingPhonesStr);

            var existingPhones = new HashSet<string>(existingPhonesList, StringComparer.OrdinalIgnoreCase);

            using (var stream = excelFile.OpenReadStream())
            using (var workbook = new XLWorkbook(stream))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Пропуск заголовков

                foreach (var row in rows)
                {
                    var fullName = row.Cell(1).GetString().Trim();
                    var rawPhone = row.Cell(2).GetString().Trim();
                    var email = row.Cell(3).GetString().Trim();

                    // Минимум нужны Имя, Телефон и Email
                    if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(rawPhone) || string.IsNullOrEmpty(email))
                        continue;

                    // --- 1. Валидация и форматирование Email ---
                    int atIndex = email.IndexOf('@');
                    // Если '@' нет, либо она стоит первой (нет имени ящика), либо последней (нет домена) — пропускаем
                    if (atIndex <= 0 || atIndex == email.Length - 1)
                    {
                        continue;
                    }

                    // --- 2. Форматирование телефона к виду +7 (___) ___-__-__ ---
                    // Извлекаем из строки только цифры
                    string digitsOnly = new string(rawPhone.Where(char.IsDigit).ToArray());
                    string formattedPhone = string.Empty;

                    // Приводим номера с кодом страны '7' или '8' к единому знаменателю (10 цифр)
                    if (digitsOnly.Length == 11 && (digitsOnly.StartsWith("7") || digitsOnly.StartsWith("8")))
                    {
                        digitsOnly = digitsOnly.Substring(1);
                    }

                    // Если после отсечения кода у нас осталось ровно 10 цифр — собираем формат
                    if (digitsOnly.Length == 10)
                    {
                        formattedPhone = $"+7 ({digitsOnly.Substring(0, 3)}) {digitsOnly.Substring(3, 3)}-{digitsOnly.Substring(6, 2)}-{digitsOnly.Substring(8, 2)}";
                    }

                    // Если формат собрать не удалось (цифр больше/меньше) — пропускаем строку
                    if (string.IsNullOrEmpty(formattedPhone))
                    {
                        continue;
                    }

                    // Проверяем на дубликаты по отформатированному номеру
                    if (existingPhones.Contains(formattedPhone))
                        continue;

                    existingPhones.Add(formattedPhone);

                    clientsToCreate.Add(new CreateClientDTO
                    {
                        FullName = fullName,
                        Phone = formattedPhone,
                        Email = email
                    });
                }
            }

            if (!clientsToCreate.Any())
            {
                return new JsonResult(new { success = false, message = "В файле нет новых записей для добавления. Возможные причины: дубликаты, неверный формат телефона/почты." });
            }

            await _clientService.CreateRangeAsync(clientsToCreate, ct);

            return new JsonResult(new { success = true, count = clientsToCreate.Count });
        }
    }
}