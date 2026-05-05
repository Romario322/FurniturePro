using FurniturePro.Core.Entities.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities;

public class DeletedId : BaseEntity<int>
{
    [Column(TypeName = "varchar(200)")]
    public required string TableName { get; set; }
    [Column(TypeName = "text")]
    public string? Description { get; set; }
    [Column(TypeName = "int")]
    public required int EntityId { get; set; }
}
