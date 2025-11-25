using Asp.Versioning;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Furnitures;
using Microsoft.AspNetCore.Mvc;
using UchetCartridge.Core.Services.Interfaces;

namespace FurniturePro.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/furnitures")]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
public class FurnitureController : ControllerBase
{
    private readonly IFurnitureService _furnitureService;

    public FurnitureController(IFurnitureService furnitureService)
    {
        _furnitureService = furnitureService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("all")]
    public async Task<List<FurnitureDTO>> GetAll(CancellationToken ct = default) => await _furnitureService.GetAllAsync(ct);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("{id}")]
    public async Task<FurnitureDTO?> Get(int id, CancellationToken ct = default) => await _furnitureService.GetByIdAsync(id, ct);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("lastUpdateDate")]
    public async Task<DateTime?> GetLastUpdateDate(CancellationToken ct = default) => await _furnitureService.GetLastUpdateDateAsync(ct);

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFurnitureDTO modelDto, CancellationToken ct = default)
    {
        await _furnitureService.UpdateAsync(id, modelDto, ct);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] CreateFurnitureDTO modelDto, CancellationToken ct = default)
    {
        var id = await _furnitureService.CreateAsync(modelDto, ct);
        return CreatedAtAction(nameof(Create), id);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
    {
        await _furnitureService.DeleteAsync(id, ct);
        return NoContent();
    }
}
