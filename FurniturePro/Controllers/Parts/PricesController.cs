using FurniturePro.Controllers.Abstractions;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Services.Parts;
using FurniturePro.Core.Models.Dto.Parts.Create;
using FurniturePro.Core.Models.Dto.Parts.Read;
using FurniturePro.Core.Models.Dto.Parts.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurniturePro.Controllers.Parts;

[Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.SalesManager))]
public class PricesController : BaseController<int, PriceDto, CreatePriceDto, UpdatePriceDto>
{
    public PricesController(IPriceService service) : base(service) { }

    [HttpGet]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.SalesManager) + "," + nameof(SystemRoleEnum.Constructor))]
    public override Task<ActionResult<List<PriceDto>>> GetAll(CancellationToken ct = default) => base.GetAll(ct);

    [HttpGet("sync")]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.SalesManager) + "," + nameof(SystemRoleEnum.Constructor))]
    public override Task<ActionResult<List<PriceDto>>> Sync([FromQuery] DateTime since, CancellationToken ct = default) => base.Sync(since, ct);
}
