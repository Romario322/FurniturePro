using FurniturePro.Core.Models.DTO.Colors;

namespace UchetCartridge.Core.Services.Interfaces;

public interface IColorService
{
    Task<List<ColorDTO>> GetAllAsync(CancellationToken ct = default);

    Task<ColorDTO?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<List<ColorDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default);

    Task<int> CreateAsync(CreateColorDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id, UpdateColorDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
