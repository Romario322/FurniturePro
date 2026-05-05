using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Orders;

namespace FurniturePro.Core.Entities.Users;

public class SystemRole : DictionaryEntity<int>
{
    public List<Employee>? Employees { get; set; }
}
