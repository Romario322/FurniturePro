using Asp.Versioning;
using FurniturePro.Core.Models.DTO.FurnitureCompositions;
using Microsoft.AspNetCore.Mvc;
using FurniturePro.Core.Services.Interfaces;

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
    [HttpGet("after {dateTime}")]
    public async Task<List<FurnitureCompositionDTO>> GetAfterDate(string dateTime, CancellationToken ct = default) =>
        await _furnitureCompositionService.GetAfterDateAsync(dateTime, ct);

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
        var ids = await _furnitureCompositionService.CreateAsync(modelDto, ct);
        return CreatedAtAction(nameof(Get), new { furnitureId = ids[0], partId = ids[1] }, null);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPost("range")]
    public async Task<IActionResult> CreateRange([FromBody] List<FurnitureCompositionDTO> modelsDto, CancellationToken ct = default)
    {
        var createdIds = await _furnitureCompositionService.CreateRangeAsync(modelsDto, ct);

        // Возвращаем список созданных пар ID
        return CreatedAtAction(nameof(GetAll), createdIds);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [HttpPut("range")]
    public async Task<IActionResult> UpdateRange([FromBody] List<FurnitureCompositionDTO> modelsDto, CancellationToken ct = default)
    {
        await _furnitureCompositionService.UpdateRangeAsync(modelsDto, ct);
        return NoContent();
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

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpDelete("range")]
    public async Task<IActionResult> DeleteRange([FromBody] List<(int idFurniture, int idPart)> ids, CancellationToken ct = default)
    {
        await _furnitureCompositionService.DeleteRangeAsync(ids, ct);
        return NoContent();
    }
}