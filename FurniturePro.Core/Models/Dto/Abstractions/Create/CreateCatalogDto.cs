namespace FurniturePro.Core.Models.Dto.Abstractions.Create;

public abstract class CreateCatalogDto
{
    public required string Sku { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}