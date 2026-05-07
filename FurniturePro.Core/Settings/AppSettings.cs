using System.ComponentModel.DataAnnotations;

namespace FurniturePro.Core;

public class AppSettings
{
    public const string AppSettingsName = "AppSettings";

    [Required(ErrorMessage = $"Блок с настройками {nameof(DbOptions)} не найден")]
    public required DbOptions DbOptions { get; set; }

    [Required(ErrorMessage = $"Блок с настройками {nameof(Swagger)} не найден")]
    public required SwaggerConfiguration Swagger { get; set; }

    [Required(ErrorMessage = $"Блок с настройками Jwt не найден")]
    public required JwtSettings Jwt { get; set; }
}