using FurniturePro.Core.Models.DTO.Parts;

namespace UchetCartridge.Core.Services.Interfaces;

public interface IPartService
{
    Task<List<PartDTO>> GetAllAsync(CancellationToken ct = default);

    Task<PartDTO?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<List<PartDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default);

    Task<int> CreateAsync(CreatePartDTO modelDto, CancellationToken ct = default);

    Task UpdateAsync(int id, UpdatePartDTO modelDto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
