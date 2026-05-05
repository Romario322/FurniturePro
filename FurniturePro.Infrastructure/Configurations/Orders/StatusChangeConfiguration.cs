using FurniturePro.Core.Entities.Orders;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Orders;

internal class StatusChangeConfiguration : BaseEntityConfiguration<StatusChange, int>
{
    public override void Configure(EntityTypeBuilder<StatusChange> builder)
    {
        base.Configure(builder);

        builder.ToTable("StatusChanges");

        builder.Property(e => e.Date)
               .IsRequired()
               .HasColumnType("timestamp with time zone");

        builder.HasOne(e => e.Order)
               .WithMany(o => o.StatusChanges)
               .HasForeignKey(e => e.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Status)
               .WithMany(s => s.StatusChanges)
               .HasForeignKey(e => e.StatusId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}