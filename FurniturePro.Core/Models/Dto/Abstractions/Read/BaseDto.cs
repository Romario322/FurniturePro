namespace FurniturePro.Core.Models.Dto.Abstractions.Read;

public abstract class BaseDto<TId> where TId : notnull
{
    public TId Id { get; set; } = default!;
    public required DateTime UpdateDate { get; set; }
}
