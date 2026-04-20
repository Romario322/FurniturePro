using FurniturePro.Core.Models.DTO.Snapshots;

namespace UchetCartridge.Core.Services.Interfaces;

public interface ISnapshotService
{
    Task<List<SnapshotDTO>> GetAllAsync(CancellationToken ct = default);

    Task<SnapshotDTO?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<List<SnapshotDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default);

    Task<int> CreateAsync(CreateSnapshotDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id, UpdateSnapshotDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
