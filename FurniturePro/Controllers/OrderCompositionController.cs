using Asp.Versioning;
using FurniturePro.Core.Models.DTO.OrderCompositions;
using Microsoft.AspNetCore.Mvc;
using UchetCartridge.Core.Services.Interfaces;

namespace FurniturePro.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/orderCompositions")]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
public class OrderCompositionController : ControllerBase
{
    private readonly IOrderCompositionService _orderCompositionService;

    public OrderCompositionController(IOrderCompositionService orderCompositionService)
    {
        _orderCompositionService = orderCompositionService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("all")]
    public async Task<List<OrderCompositionDTO>> GetAll(CancellationToken ct = default) =>
        await _orderCompositionService.GetAllAsync(ct);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("{orderId},{furnitureId}")]
    public async Task<OrderCompositionDTO?> Get(int orderId, int furnitureId, CancellationToken ct = default) =>
        await _orderCompositionService.GetByIdsAsync(orderId, furnitureId, ct);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("after {dateTime}")]
    public async Task<List<OrderCompositionDTO>> GetAfterDate(string dateTime, CancellationToken ct = default) =>
        await _orderCompositionService.GetAfterDateAsync(dateTime, ct);

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPut("{orderId},{furnitureId}")]
    public async Task<IActionResult> Update(int orderId, int furnitureId, [FromBody] OrderCompositionDTO modelDto, CancellationToken ct = default)
    {
        await _orderCompositionService.UpdateAsync(orderId, furnitureId, modelDto, ct);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] OrderCompositionDTO modelDto, CancellationToken ct = default)
    {
        var id = await _orderCompositionService.CreateAsync(modelDto, ct);
        return CreatedAtAction(nameof(Create), id);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPost("range")]
    public async Task<IActionResult> CreateRange([FromBody] List<OrderCompositionDTO> modelsDto, CancellationToken ct = default)
    {
        var createdIds = await _orderCompositionService.CreateRangeAsync(modelsDto, ct);

        // Возвращаем список созданных пар ID
        return CreatedAtAction(nameof(GetAll), createdIds);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [HttpPut("range")]
    public async Task<IActionResult> UpdateRange([FromBody] List<OrderCompositionDTO> modelsDto, CancellationToken ct = default)
    {
        await _orderCompositionService.UpdateRangeAsync(modelsDto, ct);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpDelete("{orderId},{furnitureId}")]
    public async Task<IActionResult> Delete(int orderId, int furnitureId, CancellationToken ct = default)
    {
        await _orderCompositionService.DeleteAsync(orderId, furnitureId, ct);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpDelete("range")]
    public async Task<IActionResult> DeleteRange([FromBody] List<(int idOrder, int idFurniture)> ids, CancellationToken ct = default)
    {
        await _orderCompositionService.DeleteRangeAsync(ids, ct);
        return NoContent();
    }
}
