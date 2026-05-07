using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurniturePro.Core.Models.Dto.System.Passwords;

public class AdminResetPasswordDto
{
    public required string NewPassword { get; set; }
}
