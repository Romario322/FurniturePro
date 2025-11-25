using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Furnitures;
using FurniturePro.Core.Models.DTO.Statuses;

namespace UchetCartridge.Core.Services.Interfaces;

public interface IFurnitureService
{
    Task<List<FurnitureDTO>> GetAllAsync(CancellationToken ct = default);

    Task<FurnitureDTO?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default);

    Task<int> CreateAsync(CreateFurnitureDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id, UpdateFurnitureDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
