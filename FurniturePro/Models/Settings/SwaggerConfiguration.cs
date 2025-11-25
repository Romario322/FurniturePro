using System.ComponentModel.DataAnnotations;

namespace FurniturePro.Models.Settings;

public class SwaggerConfiguration
{
    [Required(ErrorMessage = $"{nameof(UseSwagger)} не найден")]
    public bool UseSwagger { get; set; }

    [Required(ErrorMessage = $"{nameof(Title)} не найден")]
    public required string Title { get; set; }

    [Required(ErrorMessage = $"{nameof(Description)} не найден")]
    public required string Description { get; set; }

    public string? ProxyPass { get; set; }
}
