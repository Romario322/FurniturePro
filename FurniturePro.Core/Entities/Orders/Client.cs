using FurniturePro.Core.Entities.Abstractions;
using System.Xml.Linq;

namespace FurniturePro.Core.Entities.Orders;

public class Client : BaseEntity<int>
{
    public required string FullName { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }

    public List<Order>? Orders { get; set; }

    public override string ToString()
    {
        return $"Клиент: {FullName} (Телефон: {Phone}, Почта: {Email})";
    }
}

