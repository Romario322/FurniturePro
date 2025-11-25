namespace FurniturePro.Core.Models.DTO.StatusChanges;

public class StatusChangeDTO
{
    public required int IdOrder { get; set; }
    public required int IdStatus { get; set; }

    public required DateTime Date { get; set; }
}
