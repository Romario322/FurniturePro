using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Abstractions;

public abstract class BaseEntity<TId> : IEntity<TId> where TId : notnull
{
    [Column(TypeName = "integer")]
    public TId Id { get; set; } = default!;

    [Column(TypeName = "timestamp")]
    public required DateTime UpdateDate { get; set; }
}
