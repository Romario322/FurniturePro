using Asp.Versioning;
using FurniturePro.Core.Models.DTO.Categories;
using Microsoft.AspNetCore.Mvc;
using FurniturePro.Core.Services.Interfaces;

namespace FurniturePro.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/cache")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public class CacheController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IClientService _clientService;
        private readonly IColorService _colorService;
        private readonly IMaterialService _materialService;
        private readonly IOperationTypeService _operationTypeService;
        private readonly IStatusService _statusService;

        private readonly IFurnitureService _furnitureService;
        private readonly IOperationService _operationService;
        private readonly IOrderService _orderService;
        private readonly IPartService _partService;
        private readonly IPriceService _priceService;
        private readonly ISnapshotService _snapshotService;

        private readonly IFurnitureCompositionService _furnitureCompositionService;
        private readonly IOrderCompositionService _orderCompositionService;
        private readonly IStatusChangeService _statusChangeService;

        private readonly IDeletedIdService _deletedIdService;

        public CacheController(ICategoryService categoryService, IClientService clientService, IColorService colorService, 
            IMaterialService materialService, IOperationTypeService operationTypeService, IStatusService statusService, 
            IFurnitureService furnitureService, IOperationService operationService, IOrderService orderService, IPartService partService,
            IPriceService priceService, ISnapshotService snapshotService, IFurnitureCompositionService furnitureCompositionService,
            IOrderCompositionService orderCompositionService, IStatusChangeService statusChangeService, IDeletedIdService deletedIdService)
        {
            _categoryService = categoryService;
            _clientService = clientService;
            _colorService = colorService;
            _materialService = materialService;
            _operationTypeService = operationTypeService;
            _statusService = statusService;

            _furnitureService = furnitureService;
            _operationService = operationService;
            _orderService = orderService;
            _partService = partService;
            _priceService = priceService;
            _snapshotService = snapshotService;

            _furnitureCompositionService = furnitureCompositionService;
            _orderCompositionService = orderCompositionService;
            _statusChangeService = statusChangeService;

            _deletedIdService = deletedIdService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [HttpGet("after {dateTime}")]
        public async Task<List<CategoryDTO>> GetAfterDate(string dateTime, CancellationToken ct = default) 
        {
            return await _categoryService.GetAfterDateAsync(dateTime, ct);
        }
    }

}
