using FurniturePro.Core.Models.DTO.Materials;

namespace FurniturePro.Core.Services.Interfaces;

public interface IMaterialService
{
    Task<List<MaterialDTO>> GetAllAsync(CancellationToken ct = default);

    Task<MaterialDTO?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<List<MaterialDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default);

    Task<int> CreateAsync(CreateMaterialDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id, UpdateMaterialDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
