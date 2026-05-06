namespace FurniturePro.Core.Models.Dto.Orders.Create;

public class CreateOrderPartDetailDto
{
    public required int Quantity { get; set; }
    public required decimal CostPerUnit { get; set; }
    public required decimal Weight { get; set; }
    public required int SawingLength { get; set; }
    public required int SawingWidth { get; set; }

    public required int OrderCompositionId { get; set; }
    public required int PartId { get; set; }
}