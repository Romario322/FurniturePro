namespace FurniturePro.Core.Models.Dto.Orders.Create;

public class CreateClientDto
{
    public required string FullName { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
}