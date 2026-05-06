using FurniturePro.Core.Models.Dto.Abstractions.Read;

namespace FurniturePro.Core.Models.Dto.Orders.Read;

public class OrderDto : BaseDto<int>
{
    public required string OrderNumber { get; set; }
    public required decimal TotalAmount { get; set; }
    public required decimal TotalWeight { get; set; }

    public required int ClientId { get; set; }
    public required int ResponsibleEmployeeId { get; set; }
}