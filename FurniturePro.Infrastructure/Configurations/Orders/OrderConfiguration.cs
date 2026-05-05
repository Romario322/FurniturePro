using FurniturePro.Core.Entities.Orders;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Orders;

internal class OrderConfiguration : BaseEntityConfiguration<Order, int>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.ToTable("Orders");

        builder.Property(e => e.OrderNumber)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasIndex(e => e.OrderNumber)
               .IsUnique();

        builder.Property(e => e.TotalAmount)
               .IsRequired()
               .HasColumnType("numeric(18, 2)");

        builder.Property(e => e.TotalWeight)
               .IsRequired()
               .HasColumnType("numeric(10, 3)");

        builder.HasOne(e => e.Client)
               .WithMany(c => c.Orders)
               .HasForeignKey(e => e.ClientId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ResponsibleEmployee)
               .WithMany(emp => emp.ResponsibleForOrders)
               .HasForeignKey(e => e.ResponsibleEmployeeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
