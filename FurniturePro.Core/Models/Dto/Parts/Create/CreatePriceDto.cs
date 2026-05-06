namespace FurniturePro.Core.Models.Dto.Parts.Create;

public class CreatePriceDto
{
    public required decimal Value { get; set; }
    public required DateTime Date { get; set; }

    public required int PartId { get; set; }
    public required int EmployeeId { get; set; }
}