namespace FurniturePro.Core.Models.DTO.Orders;

public class CreateOrderDTO
{
    public required string Address { get; set; }
    public required int ClientId { get; set; }
}
