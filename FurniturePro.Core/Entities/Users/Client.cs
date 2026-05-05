using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Constructor;
using FurniturePro.Core.Entities.Orders;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Users;

public class Client : BaseEntity<int>
{
    [Column(TypeName = "varchar(200)")]
    public required string FullName { get; set; }
    [Column(TypeName = "varchar(200)")]
    public required string Phone { get; set; }
    [Column(TypeName = "varchar(200)")]
    public required string Email { get; set; }
    public List<Order>? Orders { get; set; }
}
