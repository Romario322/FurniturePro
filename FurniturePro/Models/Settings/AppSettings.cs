using System.ComponentModel.DataAnnotations;

namespace FurniturePro.Models.Settings;

public class AppSettings
{
    public const string AppSettingsName = "AppSettings";

    [Required(ErrorMessage = $"Блок с настройками {nameof(DbOptions)} не найден")]
    public required DbOptions DbOptions { get; set; }

    [Required(ErrorMessage = $"Блок с настройками {nameof(Swagger)} не найден")]
    public required SwaggerConfiguration Swagger { get; set; }
}
