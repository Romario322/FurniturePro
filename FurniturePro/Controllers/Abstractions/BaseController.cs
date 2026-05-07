using FurniturePro.Core.Interfaces.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FurniturePro.Controllers.Abstractions;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<TId, TReadDto, TCreateDto, TUpdateDto> : ControllerBase
    where TId : notnull
{
    protected readonly IBaseService<TId, TReadDto, TCreateDto, TUpdateDto> _service;

    protected BaseController(IBaseService<TId, TReadDto, TCreateDto, TUpdateDto> service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual async Task<ActionResult<List<TReadDto>>> GetAll(CancellationToken ct = default)
    {
        var result = await _service.GetAllAsync(ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TReadDto>> GetById(TId id, CancellationToken ct = default)
    {
        var result = await _service.GetByIdAsync(id, ct);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("sync")]
    public virtual async Task<ActionResult<List<TReadDto>>> Sync([FromQuery] DateTime since, CancellationToken ct = default)
    {
        var result = await _service.GetAfterDateAsync(since, ct);
        return Ok(result);
    }

    [HttpPost]
    public virtual async Task<ActionResult<TReadDto>> Create([FromBody] TCreateDto dto, CancellationToken ct = default)
    {
        var result = await _service.CreateAsync(dto, ct);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("bulk")]
    public virtual async Task<ActionResult<List<TReadDto>>> CreateRange([FromBody] List<TCreateDto> dtos, CancellationToken ct = default)
    {
        var result = await _service.CreateRangeAsync(dtos, ct);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(TId id, [FromBody] TUpdateDto dto, CancellationToken ct = default)
    {
        await _service.UpdateAsync(id, dto, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(TId id, CancellationToken ct = default)
    {
        await _service.DeleteAsync(id, ct);
        return NoContent();
    }
}