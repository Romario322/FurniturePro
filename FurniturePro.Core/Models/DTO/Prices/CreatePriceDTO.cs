namespace FurniturePro.Core.Models.DTO.Prices;

public class CreatePriceDTO
{
    public required int PartId { get; set; }
    public required decimal Value { get; set; }
    public required DateTime Date { get; set; }
}
