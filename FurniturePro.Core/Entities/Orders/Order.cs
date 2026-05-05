using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Orders;

public class Order : BaseEntity<int>
{
    [Column(TypeName = "varchar(50)")]
    public required string OrderNumber { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public required decimal TotalAmount { get; set; }

    [Column(TypeName = "decimal(10, 3)")]
    public required decimal TotalWeight { get; set; }

    [Column(TypeName = "integer")]
    public required int ClientId { get; set; }
    public Client? Client { get; set; }

    [Column(TypeName = "integer")]
    public required int ResponsibleEmployeeId { get; set; }
    public Employee? ResponsibleEmployee { get; set; }

    public List<OrderComposition>? OrderCompositions { get; set; }
    public List<StatusChange>? StatusChanges { get; set; }
}