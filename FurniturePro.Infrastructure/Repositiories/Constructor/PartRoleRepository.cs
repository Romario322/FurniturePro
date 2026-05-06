using FurniturePro.Core.Entities.Constructor;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Repositories.Constructor;
using FurniturePro.Infrastructure.Data;
using FurniturePro.Infrastructure.Repositiories.Abstractions;

namespace FurniturePro.Infrastructure.Repositiories.Constructor;

public class PartRoleRepository(AppDbContext context)
    : BaseRepository<PartRole, PartRoleEnum, AppDbContext>(context), IPartRoleRepository
{ }
