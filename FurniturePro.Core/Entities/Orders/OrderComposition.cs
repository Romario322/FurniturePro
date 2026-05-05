using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Furniture;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Orders;

public class OrderComposition : BaseEntity<int>
{
    [Column(TypeName = "integer")]
    public required int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public required decimal Cost { get; set; }

    public int? Length { get; set; }
    public int? Width { get; set; }
    public int? Depth { get; set; }

    [Column(TypeName = "decimal(10, 3)")]
    public required decimal Weight { get; set; }

    [Column(TypeName = "integer")]
    public required int OrderId { get; set; }
    public Order? Order { get; set; }

    [Column(TypeName = "integer")]
    public required int FurnitureId { get; set; }
    public Furniture.Furniture? Furniture { get; set; }

    public List<OrderPartDetail>? OrderPartDetails { get; set; }
}