using FurniturePro.Core.Entities.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities;

public class Count : BaseEntity<int>
{
    [Column(TypeName = "integer")]
    public required int Remaining { get; set; }
    [Column(TypeName = "integer")]
    public required int WrittenOff { get; set; }
    [Column(TypeName = "integer")]
    public required int Updated { get; set; }
    [Column(TypeName = "timestamp")]
    public required DateTime Date { get; set; }

    [Column(TypeName = "integer")]
    public required int PartId { get; set; }
    public required Part Part { get; set; }
}
