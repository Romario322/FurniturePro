using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FurniturePro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Discount = table.Column<int>(type: "integer", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "furnitures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Markup = table.Column<int>(type: "integer", nullable: true),
                    Activity = table.Column<bool>(type: "boolean", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_furnitures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_furnitures_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "parts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Length = table.Column<int>(type: "integer", nullable: true),
                    Width = table.Column<int>(type: "integer", nullable: true),
                    Height = table.Column<int>(type: "integer", nullable: true),
                    Diameter = table.Column<int>(type: "integer", nullable: true),
                    Weight = table.Column<int>(type: "integer", nullable: true),
                    Activity = table.Column<bool>(type: "boolean", nullable: false),
                    ColorId = table.Column<int>(type: "integer", nullable: true),
                    MaterialId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_parts_colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "colors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_parts_materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "materials",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "statusChanges",
                columns: table => new
                {
                    Entity1Id = table.Column<int>(type: "integer", nullable: false),
                    Entity2Id = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statusChanges", x => new { x.Entity1Id, x.Entity2Id });
                    table.ForeignKey(
                        name: "FK_statusChanges_orders_Entity1Id",
                        column: x => x.Entity1Id,
                        principalTable: "orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_statusChanges_statuses_Entity2Id",
                        column: x => x.Entity2Id,
                        principalTable: "statuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "orderCompositions",
                columns: table => new
                {
                    Entity1Id = table.Column<int>(type: "integer", nullable: false),
                    Entity2Id = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderCompositions", x => new { x.Entity1Id, x.Entity2Id });
                    table.ForeignKey(
                        name: "FK_orderCompositions_furnitures_Entity2Id",
                        column: x => x.Entity2Id,
                        principalTable: "furnitures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orderCompositions_orders_Entity1Id",
                        column: x => x.Entity1Id,
                        principalTable: "orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "counts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Remaining = table.Column<int>(type: "integer", nullable: false),
                    WrittenOff = table.Column<int>(type: "integer", nullable: false),
                    Updated = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp", nullable: false),
                    PartId = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_counts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_counts_parts_PartId",
                        column: x => x.PartId,
                        principalTable: "parts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "furnitureCompositions",
                columns: table => new
                {
                    Entity1Id = table.Column<int>(type: "integer", nullable: false),
                    Entity2Id = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_furnitureCompositions", x => new { x.Entity1Id, x.Entity2Id });
                    table.ForeignKey(
                        name: "FK_furnitureCompositions_furnitures_Entity1Id",
                        column: x => x.Entity1Id,
                        principalTable: "furnitures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_furnitureCompositions_parts_Entity2Id",
                        column: x => x.Entity2Id,
                        principalTable: "parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp", nullable: false),
                    PartId = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_prices_parts_PartId",
                        column: x => x.PartId,
                        principalTable: "parts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_categories_Name",
                table: "categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_colors_Name",
                table: "colors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_counts_PartId",
                table: "counts",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_furnitureCompositions_Entity2Id",
                table: "furnitureCompositions",
                column: "Entity2Id");

            migrationBuilder.CreateIndex(
                name: "IX_furnitures_CategoryId",
                table: "furnitures",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_furnitures_Name",
                table: "furnitures",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_materials_Name",
                table: "materials",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orderCompositions_Entity2Id",
                table: "orderCompositions",
                column: "Entity2Id");

            migrationBuilder.CreateIndex(
                name: "IX_parts_ColorId",
                table: "parts",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_parts_MaterialId",
                table: "parts",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_parts_Name",
                table: "parts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_prices_PartId",
                table: "prices",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_statusChanges_Entity2Id",
                table: "statusChanges",
                column: "Entity2Id");

            migrationBuilder.CreateIndex(
                name: "IX_statuses_Name",
                table: "statuses",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "counts");

            migrationBuilder.DropTable(
                name: "furnitureCompositions");

            migrationBuilder.DropTable(
                name: "orderCompositions");

            migrationBuilder.DropTable(
                name: "prices");

            migrationBuilder.DropTable(
                name: "statusChanges");

            migrationBuilder.DropTable(
                name: "furnitures");

            migrationBuilder.DropTable(
                name: "parts");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "statuses");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "colors");

            migrationBuilder.DropTable(
                name: "materials");
        }
    }
}
