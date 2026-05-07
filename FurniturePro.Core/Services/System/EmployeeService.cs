using AutoMapper;
using FurniturePro.Core.Entities.System;
using FurniturePro.Core.Interfaces.Repositories.System;
using FurniturePro.Core.Interfaces.Services.System;
using FurniturePro.Core.Models.Dto.System.Create;
using FurniturePro.Core.Models.Dto.System.Passwords;
using FurniturePro.Core.Models.Dto.System.Read;
using FurniturePro.Core.Models.Dto.System.Update;
using FurniturePro.Core.Services.Abstractions;

namespace FurniturePro.Core.Services.System;

public class EmployeeService : BaseService<Employee, int, EmployeeDto, CreateEmployeeDto, UpdateEmployeeDto>, IEmployeeService
{
    public EmployeeService(IEmployeeRepository repository, ICurrentUserService currentUserService, IDeletedIdRepository deletedIdRepository, IMapper mapper)
        : base(repository, currentUserService, deletedIdRepository, mapper)
    {
    }

    public override async Task<EmployeeDto> CreateAsync(CreateEmployeeDto createDto, CancellationToken ct = default)
    {
        createDto.HashPassword = BCrypt.Net.BCrypt.HashPassword(createDto.HashPassword);

        return await base.CreateAsync(createDto, ct);
    }
}
