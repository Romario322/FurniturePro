using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Dictionaries;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities;

public class Order : BaseEntity<int>
{
    [Column(TypeName = "varchar(200)")]
    public required string Address { get; set; }

    public List<OrderComposition>? OrderCompositions { get; set; }

    public List<StatusChange>? StatusChanges { get; set; }

    public List<Operation>? Operations { get; set; }

    [Column(TypeName = "integer")]
    public required int ClientId { get; set; }
    public Client? Client { get; set; }
}
