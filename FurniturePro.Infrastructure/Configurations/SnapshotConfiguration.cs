using FurniturePro.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations;

internal class SnapshotConfiguration : IEntityTypeConfiguration<Snapshot>
{
    public void Configure(EntityTypeBuilder<Snapshot> builder)
    {
        builder.ToTable("snapshots");

        builder.HasKey(et => et.Id);

        builder.HasIndex(et => et.UpdateDate);
    }
}
