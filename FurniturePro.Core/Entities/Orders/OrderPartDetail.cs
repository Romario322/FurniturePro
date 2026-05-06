using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Parts;

namespace FurniturePro.Core.Entities.Orders;

public class OrderPartDetail : BaseEntity<int>
{
    public required int Quantity { get; set; }
    public required decimal CostPerUnit { get; set; }
    public required decimal Weight { get; set; }
    public required int SawingLength { get; set; }
    public required int SawingWidth { get; set; }

    public required int OrderCompositionId { get; set; }
    public OrderComposition? OrderComposition { get; set; }
    public required int PartId { get; set; }
    public Part? Part { get; set; }
}