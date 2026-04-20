namespace FurniturePro.Core.Models.DTO.Operations;

public class CreateOperationDTO
{
    public required int PartId { get; set; }
    public required int OperationTypeId { get; set; }
    public required int Value { get; set; }
    public required DateTime Date { get; set; }
}
