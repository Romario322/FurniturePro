using FurniturePro.Core.Models.DTO.DeletedIds;

namespace FurniturePro.Core.Services.Interfaces;

public interface IDeletedIdService
{
    Task<List<DeletedIdDTO>> GetAllAsync(string tableName, CancellationToken ct = default);

    Task<DeletedIdDTO?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<List<DeletedIdDTO>> GetAfterDateAsync(string dateTime, string tableName, CancellationToken ct = default);

    Task<int> CreateAsync(CreateDeletedIdDTO modelDto, CancellationToken ct = default);

    Task<List<int>> CreateRangeAsync(List<CreateDeletedIdDTO> modelsDto, CancellationToken ct = default);

    Task UpdateAsync(int id, UpdateDeletedIdDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
