using FurniturePro.Core.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations;

internal class OperationTypeConfiguration : IEntityTypeConfiguration<OperationType>
{
    public void Configure(EntityTypeBuilder<OperationType> builder)
    {
        builder.ToTable("operationTypes");

        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasKey(et => et.Id);

        builder.HasMany(et => et.Operations)
               .WithOne(e => e.OperationType)
               .HasForeignKey(e => e.OperationTypeId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(et => et.UpdateDate);
    }
}
