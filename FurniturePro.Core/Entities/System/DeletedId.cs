using FurniturePro.Core.Entities.Abstractions;

namespace FurniturePro.Core.Entities.System;

public class DeletedId : BaseEntity<int>
{
    public required string TableName { get; set; }
    public string? Description { get; set; }
    public required string EntityId { get; set; }
    public required DateTime DeletedAt { get; set; }

    public required int ResponsibleEmployeeId { get; set; }
    public Employee? ResponsibleEmployee { get; set; }
}
