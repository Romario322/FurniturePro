namespace FurniturePro.Core.Models.DTO.Counts;

public class UpdateCountDTO
{
    public required int PartId { get; set; }
    public required int Remaining { get; set; }
    public required int WrittenOff { get; set; }
    public required int Updated { get; set; }
    public required DateTime Date { get; set; }
}
