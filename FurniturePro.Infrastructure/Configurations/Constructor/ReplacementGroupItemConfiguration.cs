using FurniturePro.Core.Entities.Constructor;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Constructor;

internal class ReplacementGroupItemConfiguration : BaseEntityConfiguration<ReplacementGroupItem, int>
{
    public override void Configure(EntityTypeBuilder<ReplacementGroupItem> builder)
    {
        base.Configure(builder);

        builder.ToTable("ReplacementGroupItems");

        builder.HasIndex(e => new { e.ReplacementGroupId, e.PartId })
               .IsUnique();

        builder.HasOne(e => e.ReplacementGroup)
               .WithMany(rg => rg.ReplacementGroupItems)
               .HasForeignKey(e => e.ReplacementGroupId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Part)
               .WithMany(p => p.ReplacementGroupItems)
               .HasForeignKey(e => e.PartId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
