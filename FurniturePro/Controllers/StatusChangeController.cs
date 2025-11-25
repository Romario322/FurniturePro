using Asp.Versioning;
using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.FurnitureCompositions;
using FurniturePro.Core.Models.DTO.StatusChanges;
using Microsoft.AspNetCore.Mvc;
using UchetCartridge.Core.Services.Interfaces;

namespace FurniturePro.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/statusChanges")]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
public class StatusChangeController : ControllerBase
{
    private readonly IStatusChangeService _statusChangeService;

    public StatusChangeController(IStatusChangeService statusChangeService)
    {
        _statusChangeService = statusChangeService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("all")]
    public async Task<List<StatusChangeDTO>> GetAll(CancellationToken ct = default) => 
        await _statusChangeService.GetAllAsync(ct);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("{orderId},{statusId}")]
    public async Task<StatusChangeDTO?> Get(int orderId, int statusId, CancellationToken ct = default) => 
        await _statusChangeService.GetByIdsAsync(orderId, statusId, ct);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("lastUpdateDate")]
    public async Task<DateTime?> GetLastUpdateDate(CancellationToken ct = default) => 
        await _statusChangeService.GetLastUpdateDateAsync(ct);

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPut("{orderId},{statusId}")]
    public async Task<IActionResult> Update(int orderId, int statusId, [FromBody] StatusChangeDTO modelDto, CancellationToken ct = default)
    {
        await _statusChangeService.UpdateAsync(orderId, statusId, modelDto, ct);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] StatusChangeDTO modelDto, CancellationToken ct = default)
    {
        var id = await _statusChangeService.CreateAsync(modelDto, ct);
        return CreatedAtAction(nameof(Create), id);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpDelete("{orderId},{statusId}")]
    public async Task<IActionResult> Delete(int orderId, int statusId, CancellationToken ct = default)
    {
        await _statusChangeService.DeleteAsync(orderId, statusId, ct);
        return NoContent();
    }
}
