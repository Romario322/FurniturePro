using AutoMapper;
using FurniturePro.Core.Entities.System;
using FurniturePro.Core.Models.Dto.System.Create;
using FurniturePro.Core.Models.Dto.System.Read;
using FurniturePro.Core.Models.Dto.System.Update;

namespace FurniturePro.Core.Mapper;

public class SystemProfile : Profile
{
    public SystemProfile()
    {
        CreateMap<SystemRole, SystemRoleDto>();
        CreateMap<CreateSystemRoleDto, SystemRole>();
        CreateMap<UpdateSystemRoleDto, SystemRole>();

        CreateMap<Employee, EmployeeDto>();
        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<UpdateEmployeeDto, Employee>();

        CreateMap<DeletedId, DeletedIdDto>();
        CreateMap<CreateDeletedIdDto, DeletedId>();
        CreateMap<UpdateDeletedIdDto, DeletedId>();
    }
}