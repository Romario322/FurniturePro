namespace FurniturePro.Core.Models.Dto.Abstractions.Update;

public abstract class UpdateBaseDto<TId> where TId : notnull
{
    public TId Id { get; set; } = default!;
}
