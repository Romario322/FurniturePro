namespace FurniturePro.Core.Models.DTO.OrderCompositions;

public class OrderCompositionDTO
{
    public required int IdOrder { get; set; }
    public required int IdFurniture { get; set; }

    public required int Count { get; set; }
}
