using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Abstractions;

public abstract class ConnectionEntity<TId, TEntity1, TEntity2> : IConnectionEntity<TId, TEntity1, TEntity2> where TId : notnull where TEntity1 : IEntity<TId> where TEntity2 : IEntity<TId>
{
    [Column(TypeName = "integer")]
    public required TId Entity1Id { get; set; } = default!;
    public TEntity1? Entity1 { get; set; }

    [Column(TypeName = "integer")]
    public required TId Entity2Id { get; set; } = default!;
    public TEntity2? Entity2 { get; set; }

    [Column(TypeName = "timestamptz")]
    public required DateTime UpdateDate { get; set; }
}
