using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurniturePro.Core.Interfaces.Services.System;

public interface ICurrentUserService
{
    int? GetUserId();
}
