using FurniturePro.Core.Entities.Abstractions;

namespace FurniturePro.Core.Entities.Orders;

public class Status : DictionaryEntity<int>
{
    public List<StatusChange>? StatusChanges { get; set; }
}
