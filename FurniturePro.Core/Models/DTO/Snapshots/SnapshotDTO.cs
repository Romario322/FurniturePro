namespace FurniturePro.Core.Models.DTO.Snapshots;

public class SnapshotDTO
{
    public required int Id { get; set; }

    public required int PartId { get; set; }
    public required int Value { get; set; }
    public required DateTime Date { get; set; }
    public required DateTime UpdateDate { get; set; }
}
