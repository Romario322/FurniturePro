using FurniturePro.Controllers.Abstractions;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Services.Constructor;
using FurniturePro.Core.Models.Dto.Catalog.Read;
using FurniturePro.Core.Models.Dto.Constructor.Create;
using FurniturePro.Core.Models.Dto.Constructor.Read;
using FurniturePro.Core.Models.Dto.Constructor.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurniturePro.Controllers.Constructor;

[Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.Constructor))]
public class ReplacementGroupItemsController : BaseController<int, ReplacementGroupItemDto, CreateReplacementGroupItemDto, UpdateReplacementGroupItemDto>
{
    public ReplacementGroupItemsController(IReplacementGroupItemService service) : base(service) { }

    [HttpGet]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.Constructor) + "," + nameof(SystemRoleEnum.SalesManager))]
    public override Task<ActionResult<List<ReplacementGroupItemDto>>> GetAll(CancellationToken ct = default) => base.GetAll(ct);

    [HttpGet("{id}")]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.Constructor) + "," + nameof(SystemRoleEnum.SalesManager))]
    public override Task<ActionResult<ReplacementGroupItemDto>> GetById(int id, CancellationToken ct = default) => base.GetById(id, ct);

    [HttpGet("sync")]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.Constructor) + "," + nameof(SystemRoleEnum.SalesManager))]
    public override Task<ActionResult<List<ReplacementGroupItemDto>>> Sync([FromQuery] DateTime since, CancellationToken ct = default) => base.Sync(since, ct);
}