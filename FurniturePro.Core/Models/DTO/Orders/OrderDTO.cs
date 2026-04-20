using FurniturePro.Core.Entities.Connections;

namespace FurniturePro.Core.Models.DTO.Orders;

public class OrderDTO
{
    public required int Id { get; set; }

    public int? Discount { get; set; }
    public required int ClientId { get; set; }
    public List<OrderComposition>? OrderCompositions { get; set; }
    public List<StatusChange>? StatusChanges { get; set; }
    public required DateTime UpdateDate { get; set; }
}
