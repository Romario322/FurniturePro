using FurniturePro.Core.Entities.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities;

public class Snapshot : BaseEntity<int>
{
    [Column(TypeName = "integer")]
    public required int Value { get; set; }

    [Column(TypeName = "timestamptz")]
    public required DateTime Date { get; set; }

    [Column(TypeName = "integer")]
    public required int PartId { get; set; }
    public Part? Part { get; set; }
}
