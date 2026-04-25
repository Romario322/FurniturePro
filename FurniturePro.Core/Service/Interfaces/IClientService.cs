using FurniturePro.Core.Models.DTO.Clients;
using FurniturePro.Core.Models.DTO.Colors;

namespace FurniturePro.Core.Services.Interfaces;

public interface IClientService
{
    Task<List<ClientDTO>> GetAllAsync(CancellationToken ct = default);

    Task<ClientDTO?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<List<ClientDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default);

    Task<int> CreateAsync(CreateClientDTO modelDto, CancellationToken ct = default);

    Task<List<int>> CreateRangeAsync(List<CreateClientDTO> modelDtos, CancellationToken ct = default);

    Task UpdateAsync(int id, UpdateClientDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
