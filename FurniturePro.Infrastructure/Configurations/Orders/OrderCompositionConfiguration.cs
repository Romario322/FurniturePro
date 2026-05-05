using FurniturePro.Core.Entities.Orders;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Orders;

internal class OrderCompositionConfiguration : BaseEntityConfiguration<OrderComposition, int>
{
    public override void Configure(EntityTypeBuilder<OrderComposition> builder)
    {
        base.Configure(builder);

        builder.ToTable("OrderCompositions");

        builder.Property(e => e.Quantity).IsRequired();

        builder.Property(e => e.Cost)
               .IsRequired()
               .HasColumnType("numeric(18, 2)");

        builder.Property(e => e.Weight)
               .IsRequired()
               .HasColumnType("numeric(10, 3)");

        builder.Property(e => e.Length);
        builder.Property(e => e.Width);
        builder.Property(e => e.Depth);

        builder.HasOne(e => e.Order)
               .WithMany(o => o.OrderCompositions)
               .HasForeignKey(e => e.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Furniture)
               .WithMany(f => f.OrderCompositions)
               .HasForeignKey(e => e.FurnitureId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
