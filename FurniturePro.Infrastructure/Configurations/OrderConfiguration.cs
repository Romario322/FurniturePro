using FurniturePro.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(et => et.Id);

        builder.HasMany(et => et.OrderCompositions)
               .WithOne(e => e.Entity1)
               .HasForeignKey(e => e.Entity1Id)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(et => et.StatusChanges)
               .WithOne(e => e.Entity1)
               .HasForeignKey(e => e.Entity1Id)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(et => et.Operations)
               .WithOne(e => e.Order)
               .HasForeignKey(e => e.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(et => et.UpdateDate);
    }
}
