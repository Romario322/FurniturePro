using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.FurnitureEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Orders;

public class OrderComposition : BaseEntity<int>
{
    public required int Quantity { get; set; }
    public required decimal Cost { get; set; }
    public int? Length { get; set; }
    public int? Width { get; set; }
    public int? Depth { get; set; }
    public required decimal Weight { get; set; }

    public required int OrderId { get; set; }
    public Order? Order { get; set; }
    public required int FurnitureId { get; set; }
    public Furniture? Furniture { get; set; }

    public List<OrderPartDetail>? OrderPartDetails { get; set; }
}