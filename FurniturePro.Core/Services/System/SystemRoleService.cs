using AutoMapper;
using FurniturePro.Core.Entities.System;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Repositories.System;
using FurniturePro.Core.Interfaces.Services.System;
using FurniturePro.Core.Models.Dto.System.Create;
using FurniturePro.Core.Models.Dto.System.Read;
using FurniturePro.Core.Models.Dto.System.Update;
using FurniturePro.Core.Services.Abstractions;

namespace FurniturePro.Core.Services.System;

public class SystemRoleService : BaseService<SystemRole, SystemRoleEnum, SystemRoleDto, CreateSystemRoleDto, UpdateSystemRoleDto>, ISystemRoleService
{
    public SystemRoleService(ISystemRoleRepository repository, ICurrentUserService currentUserService, IDeletedIdRepository deletedIdRepository, IMapper mapper)
        : base(repository, currentUserService, deletedIdRepository, mapper)
    {
    }
}
