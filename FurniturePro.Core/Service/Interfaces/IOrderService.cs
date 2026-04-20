using FurniturePro.Core.Models.DTO.Orders;

namespace UchetCartridge.Core.Services.Interfaces;

public interface IOrderService
{
    Task<List<OrderDTO>> GetAllAsync(CancellationToken ct = default);

    Task<OrderDTO?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<List<OrderDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default);

    Task<int> CreateAsync(CreateOrderDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id, UpdateOrderDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
