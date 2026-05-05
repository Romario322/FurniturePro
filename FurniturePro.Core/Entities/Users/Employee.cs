using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Orders;
using FurniturePro.Core.Entities.Parts;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Users;

public class Employee : BaseEntity<int>
{
    public required string FullName { get; set; }
    public required string Login { get; set; }
    public required string HashPassword { get; set; }
    public bool Activity { get; set; }

    public required int SystemRoleId { get; set; }
    public SystemRole? SystemRole { get; set; }

    public List<Order>? ResponsibleForOrders { get; set; }
    public List<Price>? SetPrices { get; set; }
}