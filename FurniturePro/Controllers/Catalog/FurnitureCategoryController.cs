using FurniturePro.Controllers.Abstractions;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Services.Catalog;
using FurniturePro.Core.Models.Dto.Catalog.Create;
using FurniturePro.Core.Models.Dto.Catalog.Read;
using FurniturePro.Core.Models.Dto.Catalog.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurniturePro.Controllers.Catalog;

[Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.Constructor))]
public class FurnitureCategoryController : BaseController<int, FurnitureCategoryDto, CreateFurnitureCategoryDto, UpdateFurnitureCategoryDto>
{
    public FurnitureCategoryController(IFurnitureCategoryService service) : base(service) { }

    [HttpGet]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.Constructor) + "," + nameof(SystemRoleEnum.SalesManager))]
    public override Task<ActionResult<List<FurnitureCategoryDto>>> GetAll(CancellationToken ct = default) => base.GetAll(ct);

    [HttpGet("{id}")]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.Constructor) + "," + nameof(SystemRoleEnum.SalesManager))]
    public override Task<ActionResult<FurnitureCategoryDto>> GetById(int id, CancellationToken ct = default) => base.GetById(id, ct);

    [HttpGet("sync")]
    [Authorize(Roles = nameof(SystemRoleEnum.Administrator) + "," + nameof(SystemRoleEnum.Constructor) + "," + nameof(SystemRoleEnum.SalesManager))]
    public override Task<ActionResult<List<FurnitureCategoryDto>>> Sync([FromQuery] DateTime since, CancellationToken ct = default) => base.Sync(since, ct);
}