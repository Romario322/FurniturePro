using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Connections;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities;

public class Order : BaseEntity<int>
{
    [Column(TypeName = "integer")]
    public int? Discount { get; set; }

    public List<OrderComposition>? OrderCompositions { get; set; }

    public List<StatusChange>? StatusChanges { get; set; }
}
