using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Repositories.System;
using FurniturePro.Core.Interfaces.Services.System;
using FurniturePro.Core.Models.Dto.System.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FurniturePro.Core.Services.System;

public class AuthService : IAuthService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly AppSettings _appSettings;

    public AuthService(IEmployeeRepository employeeRepository, AppSettings appSettings)
    {
        _employeeRepository = employeeRepository;
        _appSettings = appSettings;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken ct = default)
    {
        var employee = await _employeeRepository.GetByLoginWithRoleAsync(dto.Login, ct);

        if (employee == null)
        {
            throw new Exception("Неверный логин или пароль");
        }

        if (!employee.Activity)
        {
            throw new Exception("Пользователь неактивен");
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, employee.HashPassword);

        if (!isPasswordValid)
        {
            throw new Exception("Неверный логин или пароль");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_appSettings.Jwt.Key);

        var roleName = ((SystemRoleEnum)employee.SystemRoleId).ToString();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Name, employee.Login),
                new Claim(ClaimTypes.Role, roleName)
            }),
            Expires = DateTime.UtcNow.AddDays(_appSettings.Jwt.ExpireDays),
            Issuer = _appSettings.Jwt.Issuer,
            Audience = _appSettings.Jwt.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthResponseDto
        {
            Token = tokenHandler.WriteToken(token),
            EmployeeId = employee.Id,
            FullName = employee.FullName,
            Role = roleName
        };
    }
}
