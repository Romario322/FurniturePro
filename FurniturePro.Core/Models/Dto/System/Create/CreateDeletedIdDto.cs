namespace FurniturePro.Core.Models.Dto.System.Create;

public class CreateDeletedIdDto
{
    public required string TableName { get; set; }
    public string? Description { get; set; }
    public required int EntityId { get; set; }

    public required int ResponsibleEmployeeId { get; set; }
}