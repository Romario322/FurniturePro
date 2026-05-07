using FurniturePro.Controllers.Abstractions;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Services.System;
using FurniturePro.Core.Models.Dto.System.Create;
using FurniturePro.Core.Models.Dto.System.Passwords;
using FurniturePro.Core.Models.Dto.System.Read;
using FurniturePro.Core.Models.Dto.System.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniturePro.Controllers.System;

[Authorize(Roles = nameof(SystemRoleEnum.Administrator))]
public class EmployeesController : BaseController<int, EmployeeDto, CreateEmployeeDto, UpdateEmployeeDto>
{
    public EmployeesController(IEmployeeService service) : base(service)
    {
    }
}
