using FurniturePro.Core.Models.DTO.Categories;

namespace UchetCartridge.Core.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDTO>> GetAllAsync(CancellationToken ct = default);

    Task<CategoryDTO?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default);

    Task<int> CreateAsync(CreateCategoryDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id, UpdateCategoryDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
