namespace FurniturePro.Core.Models.DTO.Orders;

public class CreateOrderDTO
{
    public int? Discount { get; set; }
    public required int ClientId { get; set; }
}
