using FurniturePro.Core.Entities.Orders;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Orders;

internal class OrderPartDetailConfiguration : BaseEntityConfiguration<OrderPartDetail, int>
{
    public override void Configure(EntityTypeBuilder<OrderPartDetail> builder)
    {
        base.Configure(builder);

        builder.ToTable("OrderPartDetails");

        builder.Property(e => e.Quantity).IsRequired();
        builder.Property(e => e.SawingLength).IsRequired();
        builder.Property(e => e.SawingWidth).IsRequired();

        builder.Property(e => e.CostPerUnit)
               .IsRequired()
               .HasColumnType("numeric(18, 2)");

        builder.Property(e => e.Weight)
               .IsRequired()
               .HasColumnType("numeric(10, 3)");

        builder.HasOne(e => e.OrderComposition)
               .WithMany(oc => oc.OrderPartDetails)
               .HasForeignKey(e => e.OrderCompositionId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Part)
               .WithMany(p => p.OrderPartDetails)
               .HasForeignKey(e => e.PartId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
