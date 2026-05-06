namespace FurniturePro.Core.Models.Dto.Orders.Create;

public class CreateOrderCompositionDto
{
    public required int Quantity { get; set; }
    public required decimal Cost { get; set; }
    public int? Length { get; set; }
    public int? Width { get; set; }
    public int? Depth { get; set; }
    public required decimal Weight { get; set; }

    public required int OrderId { get; set; }
    public required int FurnitureId { get; set; }
}