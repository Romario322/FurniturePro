using FurniturePro.Core.Models.DTO.StatusChanges;

namespace UchetCartridge.Core.Services.Interfaces;

public interface IStatusChangeService
{
    Task<List<StatusChangeDTO>> GetAllAsync(CancellationToken ct = default);

    Task<StatusChangeDTO?> GetByIdsAsync(int id1, int id2, CancellationToken ct = default);

    Task<List<StatusChangeDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default);

    Task<List<int>> CreateAsync(StatusChangeDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id1, int id2, StatusChangeDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id1, int id2, CancellationToken ct = default);
}
