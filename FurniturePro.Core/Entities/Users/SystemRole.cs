using FurniturePro.Core.Entities.Abstractions;

namespace FurniturePro.Core.Entities.Users;

public class SystemRole : DictionaryEntity<int>
{
    public List<Employee>? Employees { get; set; }
}
