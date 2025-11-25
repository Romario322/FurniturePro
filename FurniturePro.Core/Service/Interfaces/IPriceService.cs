using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Prices;
using FurniturePro.Core.Models.DTO.Statuses;

namespace UchetCartridge.Core.Services.Interfaces;

public interface IPriceService
{
    Task<List<PriceDTO>> GetAllAsync(CancellationToken ct = default);

    Task<PriceDTO?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default);

    Task<int> CreateAsync(CreatePriceDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id, UpdatePriceDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
