namespace FurniturePro.Core.Models.Dto.Abstractions.Update;

public abstract class UpdateDictionaryDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}