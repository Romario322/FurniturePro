namespace FurniturePro.Core.Entities.Abstractions;

public abstract class CatalogEntity<TId> : BaseEntity<TId> where TId : notnull
{
    public required string Sku { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}