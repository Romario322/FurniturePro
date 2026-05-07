using System.ComponentModel.DataAnnotations;

namespace FurniturePro.Core;

public class JwtSettings
{
    [Required(ErrorMessage = "Секретный ключ JWT не задан")]
    [MinLength(16, ErrorMessage = "Ключ должен быть не короче 16 символов")]
    public required string Key { get; set; }

    public required string Issuer { get; set; }

    public required string Audience { get; set; }

    public int ExpireDays { get; set; } = 1;
}

