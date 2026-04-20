using FurniturePro.Core.Entities.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Connections;

public class OrderComposition : ConnectionEntity<int, Order, Furniture>
{
    [Column(TypeName = "integer")]
    public required int Count { get; set; }
}
