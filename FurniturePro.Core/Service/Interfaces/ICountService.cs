using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Counts;
using FurniturePro.Core.Models.DTO.Statuses;

namespace UchetCartridge.Core.Services.Interfaces;

public interface ICountService
{
    Task<List<CountDTO>> GetAllAsync(CancellationToken ct = default);

    Task<CountDTO?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default);

    Task<int> CreateAsync(CreateCountDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id, UpdateCountDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
