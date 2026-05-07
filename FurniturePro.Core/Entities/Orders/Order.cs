using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.System;

namespace FurniturePro.Core.Entities.Orders;

public class Order : BaseEntity<int>
{
    public required string OrderNumber { get; set; }
    public required decimal TotalAmount { get; set; }
    public required decimal TotalWeight { get; set; }

    public required int ClientId { get; set; }
    public Client? Client { get; set; }
    public required int ResponsibleEmployeeId { get; set; }
    public Employee? ResponsibleEmployee { get; set; }

    public List<OrderComposition>? OrderCompositions { get; set; }
    public List<StatusChange>? StatusChanges { get; set; }

    public override string ToString()
    {
        // Если у вас есть навигационное свойство Client, можно сделать так:
        // var clientName = Client?.Name ?? "Неизвестный клиент";
        // return $"Заказ №{Id} от {CreateDate:dd.MM.yyyy} ({clientName})";

        return $"Заказ №{OrderNumber}";
    }
}