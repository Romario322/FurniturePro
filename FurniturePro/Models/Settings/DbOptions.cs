using System.ComponentModel.DataAnnotations;

namespace FurniturePro.Models.Settings;

public class DbOptions
{
    [Required(ErrorMessage = $"{nameof(ConnectionString)} не найден")]
    public required string ConnectionString { get; set; }

    public bool? UseInMemory { get; set; }
}
