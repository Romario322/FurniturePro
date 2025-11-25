using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Statuses;

namespace UchetCartridge.Core.Services.Interfaces;

public interface IStatusService
{
    Task<List<StatusDTO>> GetAllAsync(CancellationToken ct = default);

    Task<StatusDTO?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default);

    Task<int> CreateAsync(CreateStatusDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id, UpdateStatusDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
