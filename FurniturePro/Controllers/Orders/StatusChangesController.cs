using FurniturePro.Controllers.Abstractions;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Services.Orders;
using FurniturePro.Core.Models.Dto.Orders.Create;
using FurniturePro.Core.Models.Dto.Orders.Read;
using FurniturePro.Core.Models.Dto.Orders.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurniturePro.Controllers.Orders;

[Authorize(Roles = nameof(SystemRoleEnum.Administrator))]
public class StatusChangesController : BaseController<int, StatusChangeDto, CreateStatusChangeDto, UpdateStatusChangeDto>
{
    public StatusChangesController(IStatusChangeService service) : base(service) { }

    [HttpGet]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.SalesManager) + "," + nameof(SystemRoleEnum.WorkshopMaster))]
    public override Task<ActionResult<List<StatusChangeDto>>> GetAll(CancellationToken ct = default) => base.GetAll(ct);

    [HttpGet("{id}")]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.SalesManager) + "," + nameof(SystemRoleEnum.WorkshopMaster))]
    public override Task<ActionResult<StatusChangeDto>> GetById(int id, CancellationToken ct = default) => base.GetById(id, ct);

    [HttpPost]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.SalesManager) + "," + nameof(SystemRoleEnum.WorkshopMaster))]
    public override Task<ActionResult<StatusChangeDto>> Create([FromBody] CreateStatusChangeDto dto, CancellationToken ct = default)
        => base.Create(dto, ct);

    [HttpGet("sync")]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.SalesManager) + "," + nameof(SystemRoleEnum.WorkshopMaster))]
    public override Task<ActionResult<List<StatusChangeDto>>> Sync([FromQuery] DateTime since, CancellationToken ct = default) => base.Sync(since, ct);
}