namespace FurniturePro.Core.Models.DTO.Statuses;

public class StatusDTO
{
    public required int Id { get; set; }

    public required string Name { get; set; }
    public string? Description { get; set; }
}
