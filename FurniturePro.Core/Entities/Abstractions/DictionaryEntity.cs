using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Abstractions;

public abstract class DictionaryEntity<TId> : IEntity<TId> where TId : notnull
{
    [Column(TypeName = "integer")]
    public TId Id { get; set; } = default!;

    [Column(TypeName = "varchar(200)")]
    public required string Name { get; set; }
    [Column(TypeName = "text")]
    public string? Description { get; set; }

    [Column(TypeName = "timestamp")]
    public required DateTime UpdateDate { get; set; }
}
