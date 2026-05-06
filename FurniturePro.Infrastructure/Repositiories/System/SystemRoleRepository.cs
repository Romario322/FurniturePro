using FurniturePro.Core.Entities.System;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Repositories.System;
using FurniturePro.Infrastructure.Data;
using FurniturePro.Infrastructure.Repositiories.Abstractions;

namespace FurniturePro.Infrastructure.Repositiories.System;

public class SystemRoleRepository(AppDbContext context)
    : BaseRepository<SystemRole, SystemRoleEnum, AppDbContext>(context), ISystemRoleRepository
{ }
