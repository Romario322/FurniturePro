namespace FurniturePro.Core.Models.DTO.Operations;

public class OperationDTO
{
    public required int Id { get; set; }

    public required int PartId { get; set; }
    public required int OperationTypeId { get; set; }
    public required int Value { get; set; }
    public required DateTime Date { get; set; }
    public required DateTime UpdateDate { get; set; }
}
