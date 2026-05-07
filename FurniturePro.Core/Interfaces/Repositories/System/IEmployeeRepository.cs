using FurniturePro.Core.Entities.System;
using FurniturePro.Core.Interfaces.Repositories.Abstractions;

namespace FurniturePro.Core.Interfaces.Repositories.System;

public interface IEmployeeRepository : IBaseRepository<Employee, int> 
{
    Task<Employee?> GetByLoginWithRoleAsync(string login, CancellationToken ct = default);
}
