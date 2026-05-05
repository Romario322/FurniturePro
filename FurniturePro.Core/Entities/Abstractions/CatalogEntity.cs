namespace FurniturePro.Core.Entities.Abstractions;

public abstract class CatalogEntity<TId> : IEntity<TId> where TId : notnull
{
    public TId Id { get; set; } = default!;
    public required string Sku { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required DateTime UpdateDate { get; set; }
}