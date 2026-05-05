using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Parts;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Orders;

public class OrderPartDetail : BaseEntity<int>
{
    [Column(TypeName = "integer")]
    public required int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public required decimal CostPerUnit { get; set; }
    [Column(TypeName = "decimal(10, 3)")]
    public required decimal Weight { get; set; }

    [Column(TypeName = "integer")]
    public required int SawingLength { get; set; }

    [Column(TypeName = "integer")]
    public required int SawingWidth { get; set; }

    [Column(TypeName = "integer")]
    public required int OrderCompositionId { get; set; }
    public OrderComposition? OrderComposition { get; set; }

    [Column(TypeName = "integer")]
    public required int PartId { get; set; }
    public Part? Part { get; set; }
}