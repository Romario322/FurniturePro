using FurniturePro.Controllers.Abstractions;
using FurniturePro.Core.Interfaces.Services.Catalog;
using FurniturePro.Core.Models.Dto.Catalog.Create;
using FurniturePro.Core.Models.Dto.Catalog.Read;
using FurniturePro.Core.Models.Dto.Catalog.Update;
using Microsoft.AspNetCore.Authorization;

namespace FurniturePro.Controllers.Catalog;

[Authorize]
public class FurnitureController : BaseController<int, FurnitureDto, CreateFurnitureDto, UpdateFurnitureDto>
{
    public FurnitureController(IFurnitureService service) : base(service)
    {
    }
}
