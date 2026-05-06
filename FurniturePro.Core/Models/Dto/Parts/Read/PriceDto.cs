using FurniturePro.Core.Models.Dto.Abstractions.Read;

namespace FurniturePro.Core.Models.Dto.Parts.Read;

public class PriceDto : BaseDto<int>
{
    public required decimal Value { get; set; }
    public required DateTime Date { get; set; }

    public required int PartId { get; set; }
    public required int EmployeeId { get; set; }
}