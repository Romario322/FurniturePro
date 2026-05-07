using FurniturePro.Core.Models.Dto.System.Auth;
using FurniturePro.Core.Models.Dto.System.Passwords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurniturePro.Core.Interfaces.Services.System;

public interface IPasswordService
{
    Task ChangePasswordAsync(int employeeId, ChangePasswordDto dto, CancellationToken ct = default);

    Task ResetPasswordByAdminAsync(int employeeId, string newPassword, CancellationToken ct = default);
}
