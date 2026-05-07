namespace FurniturePro.Core.Models.Dto.Abstractions.Update;

public abstract class UpdateCatalogDto
{
    public required string Sku { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}