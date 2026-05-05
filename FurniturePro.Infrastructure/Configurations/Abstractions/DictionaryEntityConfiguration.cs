using FurniturePro.Core.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Abstractions;

internal abstract class DictionaryEntityConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
    where TEntity : DictionaryEntity<TId>
    where TId : notnull
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(e => e.Name)
            .IsUnique();

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.UpdateDate)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.HasIndex(e => e.UpdateDate);
    }
}
