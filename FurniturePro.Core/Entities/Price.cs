using FurniturePro.Core.Entities.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities;

public class Price : BaseEntity<int>
{
    [Column(TypeName = "integer")]
    public required int Value { get; set; }

    [Column(TypeName = "timestamp")]
    public required DateTime Date { get; set; }

    [Column(TypeName = "integer")]
    public required int PartId { get; set; }
    public required Part Part { get; set; }
}
