namespace FurniturePro.Core.Models.Dto.Abstractions.Update;

public abstract class UpdateDictionaryDto<TId> : UpdateBaseDto<TId> where TId : notnull
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}