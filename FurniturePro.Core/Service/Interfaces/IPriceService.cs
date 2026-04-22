using FurniturePro.Core.Models.DTO.Prices;

namespace FurniturePro.Core.Services.Interfaces;

public interface IPriceService
{
    Task<List<PriceDTO>> GetAllAsync(CancellationToken ct = default);

    Task<PriceDTO?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<List<PriceDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default);

    Task<int> CreateAsync(CreatePriceDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id, UpdatePriceDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
