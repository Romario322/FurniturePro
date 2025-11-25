using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Connections;

namespace FurniturePro.Core.Entities.Dictionaries;

public class Status : DictionaryEntity<int>
{
    public List<StatusChange>? StatusChanges { get; set; }
}
