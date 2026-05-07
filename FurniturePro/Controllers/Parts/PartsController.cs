using FurniturePro.Controllers.Abstractions;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Services.Parts;
using FurniturePro.Core.Models.Dto.Parts.Create;
using FurniturePro.Core.Models.Dto.Parts.Read;
using FurniturePro.Core.Models.Dto.Parts.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurniturePro.Controllers.Parts;

[Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.Constructor))]
public class PartsController : BaseController<int, PartDto, CreatePartDto, UpdatePartDto>
{
    public PartsController(IPartService service) : base(service) { }

    [HttpGet]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.Constructor) + "," + nameof(SystemRoleEnum.SalesManager) + "," + nameof(SystemRoleEnum.WorkshopMaster))]
    public override Task<ActionResult<List<PartDto>>> GetAll(CancellationToken ct = default) => base.GetAll(ct);

    [HttpGet("{id}")]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.Constructor) + "," + nameof(SystemRoleEnum.SalesManager) + "," + nameof(SystemRoleEnum.WorkshopMaster))]
    public override Task<ActionResult<PartDto>> GetById(int id, CancellationToken ct = default) => base.GetById(id, ct);

    [HttpGet("sync")]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.Constructor) + "," + nameof(SystemRoleEnum.SalesManager) + "," + nameof(SystemRoleEnum.WorkshopMaster))]
    public override Task<ActionResult<List<PartDto>>> Sync([FromQuery] DateTime since, CancellationToken ct = default) => base.Sync(since, ct);
}