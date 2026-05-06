namespace FurniturePro.Core.Models.Dto.Abstractions.Read;

public abstract class CatalogDto<TId> : BaseDto<TId> where TId : notnull
{
    public required string Sku { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}