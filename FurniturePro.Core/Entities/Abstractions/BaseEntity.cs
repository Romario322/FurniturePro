namespace FurniturePro.Core.Entities.Abstractions;

public abstract class BaseEntity<TId> : IEntity<TId> where TId : notnull
{
    public TId Id { get; set; } = default!;
    public required DateTime UpdateDate { get; set; }
}
