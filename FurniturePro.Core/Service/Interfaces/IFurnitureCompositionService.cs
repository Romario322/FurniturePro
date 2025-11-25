using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.FurnitureCompositions;
using FurniturePro.Core.Models.DTO.Statuses;

namespace UchetCartridge.Core.Services.Interfaces;

public interface IFurnitureCompositionService
{
    Task<List<FurnitureCompositionDTO>> GetAllAsync(CancellationToken ct = default);

    Task<FurnitureCompositionDTO?> GetByIdsAsync(int id1, int id2, CancellationToken ct = default);

    Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default);

    Task<List<int>> CreateAsync(FurnitureCompositionDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id1, int id2, FurnitureCompositionDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id1, int id2, CancellationToken ct = default);
}
