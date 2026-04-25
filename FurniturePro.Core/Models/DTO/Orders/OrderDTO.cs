using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Connections;

namespace FurniturePro.Core.Models.DTO.Orders;

public class OrderDTO
{
    public required int Id { get; set; }

    public required string Address { get; set; }
    public required int ClientId { get; set; }
    public List<OrderComposition>? OrderCompositions { get; set; }
    public List<StatusChange>? StatusChanges { get; set; }
    public List<Operation>? Operations { get; set; }
    public required DateTime UpdateDate { get; set; }
}
