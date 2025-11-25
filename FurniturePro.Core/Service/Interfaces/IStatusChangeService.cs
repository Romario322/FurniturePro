using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.FurnitureCompositions;
using FurniturePro.Core.Models.DTO.StatusChanges;
using FurniturePro.Core.Models.DTO.Statuses;

namespace UchetCartridge.Core.Services.Interfaces;

public interface IStatusChangeService
{
    Task<List<StatusChangeDTO>> GetAllAsync(CancellationToken ct = default);

    Task<StatusChangeDTO?> GetByIdsAsync(int id1, int id2, CancellationToken ct = default);

    Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default);

    Task<List<int>> CreateAsync(StatusChangeDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id1, int id2, StatusChangeDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id1, int id2, CancellationToken ct = default);
}
