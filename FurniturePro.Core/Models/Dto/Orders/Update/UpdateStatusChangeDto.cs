using FurniturePro.Core.Models.Dto.Abstractions.Update;

namespace FurniturePro.Core.Models.Dto.Orders.Update;

public class UpdateStatusChangeDto : UpdateBaseDto<int>
{
    public required DateTime Date { get; set; }

    public required int OrderId { get; set; }
    public required int StatusId { get; set; }
}