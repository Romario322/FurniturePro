using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Orders;
using FurniturePro.Core.Models.DTO.Statuses;

namespace UchetCartridge.Core.Services.Interfaces;

public interface IOrderService
{
    Task<List<OrderDTO>> GetAllAsync(CancellationToken ct = default);

    Task<OrderDTO?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default);

    Task<int> CreateAsync(CreateOrderDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id, UpdateOrderDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
