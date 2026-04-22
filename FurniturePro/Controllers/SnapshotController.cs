using Asp.Versioning;
using FurniturePro.Core.Models.DTO.Snapshots;
using Microsoft.AspNetCore.Mvc;
using FurniturePro.Core.Services.Interfaces;

namespace FurniturePro.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/snapshots")]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
public class SnapshotController : ControllerBase
{
    private readonly ISnapshotService _snapshotService;

    public SnapshotController(ISnapshotService snapshotService)
    {
        _snapshotService = snapshotService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("all")]
    public async Task<List<SnapshotDTO>> GetAll(CancellationToken ct = default) => await _snapshotService.GetAllAsync(ct);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("{id}")]
    public async Task<SnapshotDTO?> Get(int id, CancellationToken ct = default) => await _snapshotService.GetByIdAsync(id, ct);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("after {dateTime}")]
    public async Task<List<SnapshotDTO>> GetAfterDate(string dateTime, CancellationToken ct = default) =>
        await _snapshotService.GetAfterDateAsync(dateTime, ct);

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSnapshotDTO modelDto, CancellationToken ct = default)
    {
        await _snapshotService.UpdateAsync(id, modelDto, ct);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] CreateSnapshotDTO modelDto, CancellationToken ct = default)
    {
        var id = await _snapshotService.CreateAsync(modelDto, ct);
        return CreatedAtAction(nameof(Create), id);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
    {
        await _snapshotService.DeleteAsync(id, ct);
        return NoContent();
    }
}
