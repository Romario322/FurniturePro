using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Services.System;
using FurniturePro.Core.Models.Dto.System.Passwords;
using FurniturePro.Core.Services.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniturePro.Controllers.System;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PasswordsController : ControllerBase
{
    private readonly IPasswordService _passwordService;

    public PasswordsController(IPasswordService passwordService)
    {
        _passwordService = passwordService;
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto, CancellationToken ct = default)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();

        var userId = int.Parse(userIdClaim);

        await _passwordService.ChangePasswordAsync(userId, dto, ct);

        return Ok(new { message = "Пароль успешно изменен" });
    }

    [HttpPost("{id}/reset-password")]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator))]
    public async Task<IActionResult> ResetPasswordByAdmin(int id, [FromBody] AdminResetPasswordDto dto, CancellationToken ct = default)
    {
        await _passwordService.ResetPasswordByAdminAsync(id, dto.NewPassword, ct);

        return Ok(new { message = $"Пароль для сотрудника с ID {id} успешно сброшен." });
    }
}
