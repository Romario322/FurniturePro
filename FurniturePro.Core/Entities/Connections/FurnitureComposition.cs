using FurniturePro.Core.Entities.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Connections;

public class FurnitureComposition : ConnectionEntity<int, Furniture, Part>
{
    [Column(TypeName = "integer")]
    public required int Count { get; set; }
}
