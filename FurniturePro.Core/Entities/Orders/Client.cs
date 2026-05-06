using FurniturePro.Core.Entities.Abstractions;

namespace FurniturePro.Core.Entities.Orders;

public class Client : BaseEntity<int>
{
    public required string FullName { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }

    public List<Order>? Orders { get; set; }
}
