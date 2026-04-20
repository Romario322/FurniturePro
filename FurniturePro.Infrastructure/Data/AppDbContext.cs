using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FurniturePro.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    // Dictionaries
    public DbSet<Category> Categories { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<OperationType> OperationTypes { get; set; }

    // Base entities
    public DbSet<Part> Parts { get; set; }
    public DbSet<Furniture> Furniture { get; set; }
    public DbSet<DeletedId> DeletedIds { get; set; }
    public DbSet<Order> Orders { get; set; }

    // Dependent entities
    public DbSet<Operation> Operations { get; set; }
    public DbSet<Snapshot> Snapshots { get; set; }
    public DbSet<Price> Prices { get; set; }

    // Connections
    public DbSet<FurnitureComposition> FurnitureCompositions { get; set; }
    public DbSet<OrderComposition> OrderCompositions { get; set; }
    public DbSet<StatusChange> StatusChanges { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Dictionaries
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ColorConfiguration());
        modelBuilder.ApplyConfiguration(new MaterialConfiguration());
        modelBuilder.ApplyConfiguration(new StatusConfiguration());
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new OperationTypeConfiguration());

        // Base entities
        modelBuilder.ApplyConfiguration(new PartConfiguration());
        modelBuilder.ApplyConfiguration(new FurnitureConfiguration());
        modelBuilder.ApplyConfiguration(new DeletedIdConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());

        // Dependent entities
        modelBuilder.ApplyConfiguration(new OperationConfiguration());
        modelBuilder.ApplyConfiguration(new PriceConfiguration());
        modelBuilder.ApplyConfiguration(new SnapshotConfiguration());

        // Connections
        modelBuilder.ApplyConfiguration(new FurnitureCompositionConfiguration());
        modelBuilder.ApplyConfiguration(new OrderCompositionConfiguration());
        modelBuilder.ApplyConfiguration(new StatusChangeConfiguration());
    }
}
