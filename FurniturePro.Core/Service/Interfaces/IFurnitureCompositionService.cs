using FurniturePro.Core.Models.DTO.FurnitureCompositions;

namespace UchetCartridge.Core.Services.Interfaces;

public interface IFurnitureCompositionService
{
    Task<List<FurnitureCompositionDTO>> GetAllAsync(CancellationToken ct = default);

    Task<FurnitureCompositionDTO?> GetByIdsAsync(int id1, int id2, CancellationToken ct = default);

    Task<List<FurnitureCompositionDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default);

    Task<List<int>> CreateAsync(FurnitureCompositionDTO modelDto, CancellationToken ct = default);

    Task<List<List<int>>> CreateRangeAsync(List<FurnitureCompositionDTO> modelsDto, CancellationToken ct = default);

    Task UpdateAsync(int id1, int id2, FurnitureCompositionDTO modelDto, CancellationToken ct = default);

    Task UpdateRangeAsync(List<FurnitureCompositionDTO> modelsDto, CancellationToken ct = default);

    Task DeleteAsync(int id1, int id2, CancellationToken ct = default);

    Task DeleteRangeAsync(List<(int Id1, int Id2)> ids, CancellationToken ct = default);
}
