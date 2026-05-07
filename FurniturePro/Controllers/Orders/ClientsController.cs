using FurniturePro.Controllers.Abstractions;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Services.Orders;
using FurniturePro.Core.Models.Dto.Orders.Create;
using FurniturePro.Core.Models.Dto.Orders.Read;
using FurniturePro.Core.Models.Dto.Orders.Update;
using Microsoft.AspNetCore.Authorization;

namespace FurniturePro.Controllers.Orders;

[Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.SalesManager))]
public class ClientsController : BaseController<int, ClientDto, CreateClientDto, UpdateClientDto>
{
    public ClientsController(IClientService service) : base(service) { }
}
