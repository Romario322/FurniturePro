using FurniturePro.Core.Entities.Parts;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Parts;

internal class PriceConfiguration : BaseEntityConfiguration<Price, int>
{
    public override void Configure(EntityTypeBuilder<Price> builder)
    {
        base.Configure(builder);

        builder.ToTable("Prices");

        builder.Property(e => e.Value)
               .IsRequired()
               .HasColumnType("numeric(18, 2)");

        builder.Property(e => e.Date)
               .IsRequired()
               .HasColumnType("timestamp with time zone");

        builder.Property(e => e.PartId).IsRequired();
        builder.Property(e => e.EmployeeId).IsRequired();

        builder.HasOne(e => e.Part)
               .WithMany(p => p.Prices)
               .HasForeignKey(e => e.PartId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Employee)
               .WithMany(emp => emp.SetPrices)
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}