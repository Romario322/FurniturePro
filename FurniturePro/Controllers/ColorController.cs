using Asp.Versioning;
using FurniturePro.Core.Models.DTO.Colors;
using Microsoft.AspNetCore.Mvc;
using FurniturePro.Core.Services.Interfaces;

namespace FurniturePro.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/colors")]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
public class ColorController : ControllerBase
{
    private readonly IColorService _colorService;

    public ColorController(IColorService colorService)
    {
        _colorService = colorService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("all")]
    public async Task<List<ColorDTO>> GetAll(CancellationToken ct = default) => await _colorService.GetAllAsync(ct);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("{id}")]
    public async Task<ColorDTO?> Get(int id, CancellationToken ct = default) => await _colorService.GetByIdAsync(id, ct);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("after {dateTime}")]
    public async Task<List<ColorDTO>> GetAfterDate(string dateTime, CancellationToken ct = default) =>
        await _colorService.GetAfterDateAsync(dateTime, ct);

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateColorDTO modelDto, CancellationToken ct = default)
    {
        await _colorService.UpdateAsync(id, modelDto, ct);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] CreateColorDTO modelDto, CancellationToken ct = default)
    {
        var id = await _colorService.CreateAsync(modelDto, ct);
        return CreatedAtAction(nameof(Create), id);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
    {
        await _colorService.DeleteAsync(id, ct);
        return NoContent();
    }
}
