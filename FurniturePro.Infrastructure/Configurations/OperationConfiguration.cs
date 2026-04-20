using FurniturePro.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations;

internal class OperationConfiguration : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.ToTable("operations");

        builder.HasKey(et => et.Id);

        builder.HasIndex(et => et.UpdateDate);
    }
}
