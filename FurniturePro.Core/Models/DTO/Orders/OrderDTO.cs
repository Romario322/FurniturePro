using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Connections;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Models.DTO.Orders;

public class OrderDTO
{
    public required int Id { get; set; }

    public int? Discount { get; set; }
    public List<OrderComposition>? OrderCompositions { get; set; }
    public List<StatusChange>? StatusChanges { get; set; }
}
