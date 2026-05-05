using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Parts;
using FurniturePro.Core.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Parts;

public class Price : BaseEntity<int>
{
    [Column(TypeName = "decimal(18, 2)")]
    public required decimal Value { get; set; }

    [Column(TypeName = "timestamptz")]
    public required DateTime Date { get; set; }

    [Column(TypeName = "integer")]
    public int PartId { get; set; }
    public Part? Part { get; set; }

    [Column(TypeName = "integer")]
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}