using FurniturePro.Core.Entities.Constructors;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Constructors;

internal class FurniturePartConfiguration : BaseEntityConfiguration<FurniturePart, int>
{
    public override void Configure(EntityTypeBuilder<FurniturePart> builder)
    {
        base.Configure(builder);

        builder.ToTable("FurnitureParts");

        builder.Property(e => e.Quantity)
               .IsRequired();

        builder.HasOne(e => e.Furniture)
               .WithMany(f => f.FurnitureParts)
               .HasForeignKey(e => e.FurnitureId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.PartRole)
               .WithMany(pr => pr.FurnitureParts)
               .HasForeignKey(e => e.PartRoleId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ReplacementGroup)
               .WithMany(rg => rg.FurnitureParts)
               .HasForeignKey(e => e.ReplacementGroupId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
