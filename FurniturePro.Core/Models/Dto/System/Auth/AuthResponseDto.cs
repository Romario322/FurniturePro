using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurniturePro.Core.Models.Dto.System.Auth;

public class AuthResponseDto
{
    public required string Token { get; set; }
    public required int EmployeeId { get; set; }
    public required string FullName { get; set; }
    public required string Role { get; set; }
}
