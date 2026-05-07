using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurniturePro.Core.Models.Dto.System.Auth;

public class LoginDto
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}
