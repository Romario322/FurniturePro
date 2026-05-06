namespace FurniturePro.Core.Models.Dto.Orders.Create;

public class CreateStatusChangeDto
{
    public required DateTime Date { get; set; }

    public required int OrderId { get; set; }
    public required int StatusId { get; set; }
}