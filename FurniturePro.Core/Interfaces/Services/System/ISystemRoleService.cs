using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Services.Abstractions;
using FurniturePro.Core.Models.Dto.System.Create;
using FurniturePro.Core.Models.Dto.System.Read;
using FurniturePro.Core.Models.Dto.System.Update;

namespace FurniturePro.Core.Interfaces.Services.System;

public interface ISystemRoleService : IBaseService<SystemRoleEnum, SystemRoleDto, CreateSystemRoleDto, UpdateSystemRoleDto>
{
}