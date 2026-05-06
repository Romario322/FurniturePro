using FurniturePro.Core.Models.Dto.Abstractions.Read;

namespace FurniturePro.Core.Models.Dto.System.Read;

public class DeletedIdDto : BaseDto<int>
{
    public required string TableName { get; set; }
    public string? Description { get; set; }
    public required int EntityId { get; set; }

    public required int ResponsibleEmployeeId { get; set; }
}