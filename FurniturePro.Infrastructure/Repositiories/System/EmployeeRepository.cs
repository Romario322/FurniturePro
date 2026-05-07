using FurniturePro.Core.Entities.System;
using FurniturePro.Core.Interfaces.Repositories.System;
using FurniturePro.Infrastructure.Data;
using FurniturePro.Infrastructure.Repositiories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace FurniturePro.Infrastructure.Repositiories.System;

public class EmployeeRepository(AppDbContext context)
    : BaseRepository<Employee, int, AppDbContext>(context), IEmployeeRepository
{
    public async Task<Employee?> GetByLoginWithRoleAsync(string login, CancellationToken ct = default)
    {
        return await _dbSet
            .Include(e => e.SystemRole)
            .FirstOrDefaultAsync(e => e.Login == login, ct);
    }
}
