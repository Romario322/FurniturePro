using FurniturePro.Controllers.Abstractions;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Services.System;
using FurniturePro.Core.Models.Dto.System.Create;
using FurniturePro.Core.Models.Dto.System.Read;
using FurniturePro.Core.Models.Dto.System.Update;
using Microsoft.AspNetCore.Authorization;

namespace FurniturePro.Controllers.System;

[Authorize(Roles = nameof(SystemRoleEnum.Administrator))]
public class SystemRolesController : BaseController<SystemRoleEnum, SystemRoleDto, CreateSystemRoleDto, UpdateSystemRoleDto>
{
    public SystemRolesController(ISystemRoleService service) : base(service)
    {
    }
}
