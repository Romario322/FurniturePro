using Asp.Versioning;
using FurniturePro.Core.Models.DTO.DeletedIds;
using Microsoft.AspNetCore.Mvc;
using FurniturePro.Core.Services.Interfaces;

namespace FurniturePro.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/deletedIds")]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
public class DeletedIdController : ControllerBase
{
    private readonly IDeletedIdService _deletedIdService;

    public DeletedIdController(IDeletedIdService deletedIdService)
    {
        _deletedIdService = deletedIdService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("all {tableName}")]
    public async Task<List<DeletedIdDTO>> GetAll(string tableName, CancellationToken ct = default) => await _deletedIdService.GetAllAsync(tableName, ct);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("{id}")]
    public async Task<DeletedIdDTO?> Get(int id, CancellationToken ct = default) => await _deletedIdService.GetByIdAsync(id, ct);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("after {dateTime} {tableName}")]
    public async Task<List<DeletedIdDTO>> GetAfterDate(string dateTime, string tableName, CancellationToken ct = default) =>
        await _deletedIdService.GetAfterDateAsync(dateTime, tableName, ct);

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDeletedIdDTO modelDto, CancellationToken ct = default)
    {
        await _deletedIdService.UpdateAsync(id, modelDto, ct);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] CreateDeletedIdDTO modelDto, CancellationToken ct = default)
    {
        var id = await _deletedIdService.CreateAsync(modelDto, ct);
        return CreatedAtAction(nameof(Create), id);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPost("range")]
    public async Task<IActionResult> CreateRange([FromBody] List<CreateDeletedIdDTO> modelsDto, CancellationToken ct = default)
    {
        var createdIds = await _deletedIdService.CreateRangeAsync(modelsDto, ct);

        return CreatedAtAction(nameof(GetAll), createdIds);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
    {
        await _deletedIdService.DeleteAsync(id, ct);
        return NoContent();
    }
}
