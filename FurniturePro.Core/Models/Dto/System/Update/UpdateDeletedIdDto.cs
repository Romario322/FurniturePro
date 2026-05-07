using FurniturePro.Core.Models.Dto.Abstractions.Update;

namespace FurniturePro.Core.Models.Dto.System.Update;

public class UpdateDeletedIdDto
{
    public required string TableName { get; set; }
    public string? Description { get; set; }
    public required int EntityId { get; set; }

    public required int ResponsibleEmployeeId { get; set; }
}