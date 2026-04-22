using FurniturePro.Core.Models.DTO.Operations;

namespace FurniturePro.Core.Services.Interfaces;

public interface IOperationService
{
    Task<List<OperationDTO>> GetAllAsync(CancellationToken ct = default);

    Task<OperationDTO?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<List<OperationDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default);

    Task<int> CreateAsync(CreateOperationDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id, UpdateOperationDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
