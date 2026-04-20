using FurniturePro.Core.Entities.Abstractions;

namespace FurniturePro.Core.Entities.Dictionaries;

public class OperationType : DictionaryEntity<int>
{
    public List<Operation>? Operations { get; set; }
}
