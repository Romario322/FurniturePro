namespace FurniturePro.Core.Models.DTO.Prices;

public class UpdatePriceDTO
{
    public required int PartId { get; set; }
    public required int Value { get; set; }
    public required DateTime Date { get; set; }
}
