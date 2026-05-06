namespace FurniturePro.Core.Models.Dto.Abstractions.Update;

public abstract class UpdateCatalogDto<TId> : UpdateBaseDto<TId> where TId : notnull
{
    public required string Sku { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}