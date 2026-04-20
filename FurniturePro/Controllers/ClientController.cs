using Asp.Versioning;
using FurniturePro.Core.Models.DTO.Clients;
using Microsoft.AspNetCore.Mvc;
using UchetCartridge.Core.Services.Interfaces;

namespace FurniturePro.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/clients")]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("all")]
    public async Task<List<ClientDTO>> GetAll(CancellationToken ct = default) => await _clientService.GetAllAsync(ct);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("{id}")]
    public async Task<ClientDTO?> Get(int id, CancellationToken ct = default) => await _clientService.GetByIdAsync(id, ct);


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpGet("after {dateTime}")]
    public async Task<List<ClientDTO>> GetAfterDate(string dateTime, CancellationToken ct = default) =>
        await _clientService.GetAfterDateAsync(dateTime, ct);

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClientDTO modelDto, CancellationToken ct = default)
    {
        await _clientService.UpdateAsync(id, modelDto, ct);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] CreateClientDTO modelDto, CancellationToken ct = default)
    {
        var id = await _clientService.CreateAsync(modelDto, ct);
        return CreatedAtAction(nameof(Create), id);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
    {
        await _clientService.DeleteAsync(id, ct);
        return NoContent();
    }
}
