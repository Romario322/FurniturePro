using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities;

public class DeletedId : BaseEntity<int>
{
    public required string TableName { get; set; }
    public string? Description { get; set; }
    public required int EntityId { get; set; }

    public required int ResponsibleEmployeeId { get; set; }
    public Employee? ResponsibleEmployee { get; set; }
}
