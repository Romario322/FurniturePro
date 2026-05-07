using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Repositories.System;
using FurniturePro.Core.Interfaces.Services.System;
using FurniturePro.Core.Models.Dto.System.Auth;
using FurniturePro.Core.Models.Dto.System.Passwords;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FurniturePro.Core.Services.System;

public class PasswordService : IPasswordService
{
    private readonly IEmployeeRepository _employeeRepository;

    public PasswordService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task ChangePasswordAsync(int employeeId, ChangePasswordDto dto, CancellationToken ct = default)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId, ct);
        if (employee == null) throw new Exception("Сотрудник не найден");

        if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, employee.HashPassword))
        {
            throw new Exception("Старый пароль указан неверно");
        }

        employee.HashPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        await _employeeRepository.UpdateAsync(employee, ct);
    }

    public async Task ResetPasswordByAdminAsync(int employeeId, string newPassword, CancellationToken ct = default)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId, ct);
        if (employee == null) throw new Exception("Сотрудник не найден");

        employee.HashPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

        await _employeeRepository.UpdateAsync(employee, ct);
    }
}
