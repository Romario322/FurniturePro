namespace FurniturePro.Core.Entities.Abstractions;

public abstract class DictionaryEntity<TId> : BaseEntity<TId> where TId : notnull
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}
