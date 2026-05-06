using FurniturePro.Core.Entities.System;
using FurniturePro.Core.Interfaces.Repositories.System;
using FurniturePro.Infrastructure.Data;
using FurniturePro.Infrastructure.Repositiories.Abstractions;

namespace FurniturePro.Infrastructure.Repositiories.System;

public class EmployeeRepository(AppDbContext context)
    : BaseRepository<Employee, int, AppDbContext>(context), IEmployeeRepository
{ }
