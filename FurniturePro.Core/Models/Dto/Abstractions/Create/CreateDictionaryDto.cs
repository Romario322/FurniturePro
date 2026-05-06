namespace FurniturePro.Core.Models.Dto.Abstractions.Create;

public abstract class CreateDictionaryDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}
