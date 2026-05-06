using FurniturePro.Core.Models.Dto.Abstractions.Read;

namespace FurniturePro.Core.Models.Dto.Orders.Read;

public class StatusChangeDto : BaseDto<int>
{
    public required DateTime Date { get; set; }

    public required int OrderId { get; set; }
    public required int StatusId { get; set; }
}