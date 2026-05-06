using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Enums;

namespace FurniturePro.Core.Entities.System;

public class SystemRole : DictionaryEntity<SystemRoleEnum>
{
    public List<Employee>? Employees { get; set; }
}
