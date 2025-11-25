using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Dictionaries;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Connections;

public class StatusChange : ConnectionEntity<int, Order, Status>
{
    [Column(TypeName = "timestamp")]
    public required DateTime Date { get; set; }
}
