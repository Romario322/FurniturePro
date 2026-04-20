namespace FurniturePro.Core.Entities.Abstractions;

public interface IConnectionEntity<TId, TEntity1, TEntity2>
{
    public TId Entity1Id { get; set; }
    public TEntity1 Entity1 { get; set; }

    public TId Entity2Id { get; set; }
    public TEntity2 Entity2 { get; set; }

    DateTime UpdateDate { get; set; }
}
