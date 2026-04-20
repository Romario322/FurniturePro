namespace FurniturePro.Core.Models.DTO.Snapshots;

public class CreateSnapshotDTO
{
    public required int PartId { get; set; }
    public required int Value { get; set; }
    public required DateTime Date { get; set; }
}
