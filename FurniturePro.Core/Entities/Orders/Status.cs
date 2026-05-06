using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Enums;

namespace FurniturePro.Core.Entities.Orders;

public class Status : DictionaryEntity<StatusEnum>
{
    public List<StatusChange>? StatusChanges { get; set; }
}
