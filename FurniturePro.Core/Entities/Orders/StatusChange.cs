using FurniturePro.Core.Entities.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Orders;

public class StatusChange : BaseEntity<int>
{
    [Column(TypeName = "timestamptz")]
    public required DateTime Date { get; set; }

    [Column(TypeName = "integer")]
    public required int OrderId { get; set; }
    public Order? Order { get; set; }

    [Column(TypeName = "integer")]
    public required int StatusId { get; set; }
    public Status? Status { get; set; }
}