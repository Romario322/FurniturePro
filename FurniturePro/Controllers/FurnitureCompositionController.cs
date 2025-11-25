using Asp.Versioning;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.FurnitureCompositions;
using Microsoft.AspNetCore.Mvc;
using UchetCartridge.Core.Services.Interfaces;

namespace FurniturePro.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/furnitureCompositions")]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
public class FurnitureCompositionController : ControllerBase
{
    private readonly IFurnitureCompositionService _furnitureCompositionService;

    public FurnitureCompositionController(IFurnitureCompositionService furnitureCompositionService)
    {
        _furnitureCompositionService = furnitureCompositionService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("all")]
    public async Task<List<FurnitureCompositionDTO>> GetAll(CancellationToken ct = default) => 
        await _furnitureCompositionService.GetAllAsync(ct);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("{furnitureId},{partId}")]
    public async Task<FurnitureCompositionDTO?> Get(int furnitureId, int partId, CancellationToken ct = default) => 
        await _furnitureCompositionService.GetByIdsAsync(furnitureId, partId, ct);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("lastUpdateDate")]
    public async Task<DateTime?> GetLastUpdateDate(CancellationToken ct = default) => 
        await _furnitureCompositionService.GetLastUpdateDateAsync(ct);

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPut("{furnitureId},{partId}")]
    public async Task<IActionResult> Update(int furnitureId, int partId, [FromBody] FurnitureCompositionDTO modelDto, CancellationToken ct = default)
    {
        await _furnitureCompositionService.UpdateAsync(furnitureId, partId, modelDto, ct);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] FurnitureCompositionDTO modelDto, CancellationToken ct = default)
    {
        var id = await _furnitureCompositionService.CreateAsync(modelDto, ct);
        return CreatedAtAction(nameof(Create), id);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpDelete("{furnitureId},{partId}")]
    public async Task<IActionResult> Delete(int furnitureId, int partId, CancellationToken ct = default)
    {
        await _furnitureCompositionService.DeleteAsync(furnitureId, partId, ct);
        return NoContent();
    }
}
