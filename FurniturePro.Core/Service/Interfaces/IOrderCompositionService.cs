using FurniturePro.Core.Models.DTO.OrderCompositions;

namespace UchetCartridge.Core.Services.Interfaces;

public interface IOrderCompositionService
{
    Task<List<OrderCompositionDTO>> GetAllAsync(CancellationToken ct = default);

    Task<OrderCompositionDTO?> GetByIdsAsync(int id1, int id2, CancellationToken ct = default);

    Task<List<OrderCompositionDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default);

    Task<List<int>> CreateAsync(OrderCompositionDTO modelDto, CancellationToken ct = default);

    Task<List<List<int>>> CreateRangeAsync(List<OrderCompositionDTO> modelsDto, CancellationToken ct = default);

    Task UpdateAsync(int id1, int id2, OrderCompositionDTO modelDto, CancellationToken ct = default);

    Task UpdateRangeAsync(List<OrderCompositionDTO> modelsDto, CancellationToken ct = default);

    Task DeleteAsync(int id1, int id2, CancellationToken ct = default);

    Task DeleteRangeAsync(List<(int Id1, int Id2)> ids, CancellationToken ct = default);
}
