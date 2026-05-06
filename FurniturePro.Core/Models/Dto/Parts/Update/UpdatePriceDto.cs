using FurniturePro.Core.Models.Dto.Abstractions.Update;

namespace FurniturePro.Core.Models.Dto.Parts.Update;

public class UpdatePriceDto : UpdateBaseDto<int>
{
    public required decimal Value { get; set; }
    public required DateTime Date { get; set; }

    public required int PartId { get; set; }
    public required int EmployeeId { get; set; }
}