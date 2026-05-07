using FurniturePro.Controllers.Abstractions;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Services.Orders;
using FurniturePro.Core.Models.Dto.Constructor.Read;
using FurniturePro.Core.Models.Dto.Orders.Create;
using FurniturePro.Core.Models.Dto.Orders.Read;
using FurniturePro.Core.Models.Dto.Orders.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurniturePro.Controllers.Orders;

[Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.SalesManager))]
public class OrderCompositionsController : BaseController<int, OrderCompositionDto, CreateOrderCompositionDto, UpdateOrderCompositionDto>
{
    public OrderCompositionsController(IOrderCompositionService service) : base(service) { }

    [HttpGet]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.SalesManager) + "," + nameof(SystemRoleEnum.WorkshopMaster))]
    public override Task<ActionResult<List<OrderCompositionDto>>> GetAll(CancellationToken ct = default) => base.GetAll(ct);

    [HttpGet("{id}")]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.SalesManager) + "," + nameof(SystemRoleEnum.WorkshopMaster))]
    public override Task<ActionResult<OrderCompositionDto>> GetById(int id, CancellationToken ct = default) => base.GetById(id, ct);

    [HttpGet("sync")]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.SalesManager) + "," + nameof(SystemRoleEnum.WorkshopMaster))]
    public override Task<ActionResult<List<OrderCompositionDto>>> Sync([FromQuery] DateTime since, CancellationToken ct = default) => base.Sync(since, ct);
}