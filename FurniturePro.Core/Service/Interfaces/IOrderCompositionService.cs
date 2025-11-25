using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.FurnitureCompositions;
using FurniturePro.Core.Models.DTO.OrderCompositions;
using FurniturePro.Core.Models.DTO.Statuses;

namespace UchetCartridge.Core.Services.Interfaces;

public interface IOrderCompositionService
{
    Task<List<OrderCompositionDTO>> GetAllAsync(CancellationToken ct = default);

    Task<OrderCompositionDTO?> GetByIdsAsync(int id1, int id2, CancellationToken ct = default);

    Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default);

    Task<List<int>> CreateAsync(OrderCompositionDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id1, int id2, OrderCompositionDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id1, int id2, CancellationToken ct = default);
}
