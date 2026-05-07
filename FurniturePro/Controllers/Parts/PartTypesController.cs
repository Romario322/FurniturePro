using FurniturePro.Controllers.Abstractions;
using FurniturePro.Core.Entities.Parts;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Services.Parts;
using FurniturePro.Core.Models.Dto.Parts.Create;
using FurniturePro.Core.Models.Dto.Parts.Read;
using FurniturePro.Core.Models.Dto.Parts.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurniturePro.Controllers.Parts;

[Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.Constructor))]
public class PartTypesController : BaseController<PartTypeEnum, PartTypeDto, CreatePartTypeDto, UpdatePartTypeDto>
{
    public PartTypesController(IPartTypeService service) : base(service) { }

    [HttpGet]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.Constructor) + "," + nameof(SystemRoleEnum.SalesManager) + "," + nameof(SystemRoleEnum.WorkshopMaster))]
    public override Task<ActionResult<List<PartTypeDto>>> GetAll(CancellationToken ct = default) => base.GetAll(ct);

    [HttpGet("sync")]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.Constructor) + "," + nameof(SystemRoleEnum.SalesManager) + "," + nameof(SystemRoleEnum.WorkshopMaster))]
    public override Task<ActionResult<List<PartTypeDto>>> Sync([FromQuery] DateTime since, CancellationToken ct = default) => base.Sync(since, ct);
}