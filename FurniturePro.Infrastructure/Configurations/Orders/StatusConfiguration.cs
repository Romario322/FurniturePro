using FurniturePro.Core.Entities.Orders;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Orders;

internal class StatusConfiguration : DictionaryEntityConfiguration<Status, int>
{
    public override void Configure(EntityTypeBuilder<Status> builder)
    {
        base.Configure(builder);

        builder.ToTable("Statuses");

        builder.HasMany(e => e.StatusChanges)
               .WithOne(sc => sc.Status)
               .HasForeignKey(sc => sc.StatusId)
               .OnDelete(DeleteBehavior.Restrict);

        var seedDate = DateTime.SpecifyKind(new DateTime(2024, 1, 1), DateTimeKind.Utc);

        builder.HasData(
            new Status { Id = 1, Name = "Новый", Description = "Заказ оформлен, ожидает подтверждения", UpdateDate = seedDate },
            new Status { Id = 2, Name = "В работе", Description = "Заказ передан в цех на производство", UpdateDate = seedDate },
            new Status { Id = 3, Name = "Готов", Description = "Мебель изготовлена и находится на складе", UpdateDate = seedDate },
            new Status { Id = 4, Name = "Отгружен", Description = "Заказ передан клиенту", UpdateDate = seedDate },
            new Status { Id = 5, Name = "Отменен", Description = "Заказ отменен", UpdateDate = seedDate }
        );
    }
}
