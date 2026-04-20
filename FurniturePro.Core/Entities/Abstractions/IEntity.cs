namespace FurniturePro.Core.Entities.Abstractions;

public interface IEntity<TId>
{
    TId Id { get; set; }

    DateTime UpdateDate { get; set; }
}
