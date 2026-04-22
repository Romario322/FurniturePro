using FurniturePro.Core.Models.DTO.OperationTypes;

namespace FurniturePro.Core.Services.Interfaces;

public interface IOperationTypeService
{
    Task<List<OperationTypeDTO>> GetAllAsync(CancellationToken ct = default);

    Task<OperationTypeDTO?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<List<OperationTypeDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default);

    Task<int> CreateAsync(CreateOperationTypeDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id, UpdateOperationTypeDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
