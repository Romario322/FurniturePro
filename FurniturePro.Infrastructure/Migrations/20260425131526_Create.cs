using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FurniturePro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Create : Migration
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
                    UpdateDate = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "varchar(200)", nullable: false),
                    Phone = table.Column<string>(type: "varchar(200)", nullable: false),
                    Email = table.Column<string>(type: "varchar(200)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "deletedIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TableName = table.Column<string>(type: "varchar(200)", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deletedIds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "furniture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Markup = table.Column<int>(type: "integer", nullable: true),
                    Activity = table.Column<bool>(type: "boolean", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_furniture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_furniture_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address = table.Column<string>(type: "varchar(200)", nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_orders_clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "clients",
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
                    Weight = table.Column<decimal>(type: "decimal", nullable: true),
                    Activity = table.Column<bool>(type: "boolean", nullable: false),
                    ColorId = table.Column<int>(type: "integer", nullable: true),
                    MaterialId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamptz", nullable: false)
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
                name: "orderCompositions",
                columns: table => new
                {
                    Entity1Id = table.Column<int>(type: "integer", nullable: false),
                    Entity2Id = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderCompositions", x => new { x.Entity1Id, x.Entity2Id });
                    table.ForeignKey(
                        name: "FK_orderCompositions_furniture_Entity2Id",
                        column: x => x.Entity2Id,
                        principalTable: "furniture",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orderCompositions_orders_Entity1Id",
                        column: x => x.Entity1Id,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "statusChanges",
                columns: table => new
                {
                    Entity1Id = table.Column<int>(type: "integer", nullable: false),
                    Entity2Id = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statusChanges", x => new { x.Entity1Id, x.Entity2Id });
                    table.ForeignKey(
                        name: "FK_statusChanges_orders_Entity1Id",
                        column: x => x.Entity1Id,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_statusChanges_statuses_Entity2Id",
                        column: x => x.Entity2Id,
                        principalTable: "statuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "furnitureCompositions",
                columns: table => new
                {
                    Entity1Id = table.Column<int>(type: "integer", nullable: false),
                    Entity2Id = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_furnitureCompositions", x => new { x.Entity1Id, x.Entity2Id });
                    table.ForeignKey(
                        name: "FK_furnitureCompositions_furniture_Entity1Id",
                        column: x => x.Entity1Id,
                        principalTable: "furniture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_furnitureCompositions_parts_Entity2Id",
                        column: x => x.Entity2Id,
                        principalTable: "parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "operations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    OperationTypeId = table.Column<int>(type: "integer", nullable: false),
                    PartId = table.Column<int>(type: "integer", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_operations_operationTypes_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "operationTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_operations_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_operations_parts_PartId",
                        column: x => x.PartId,
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
                    Value = table.Column<decimal>(type: "decimal", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    PartId = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_prices_parts_PartId",
                        column: x => x.PartId,
                        principalTable: "parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "snapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    PartId = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_snapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_snapshots_parts_PartId",
                        column: x => x.PartId,
                        principalTable: "parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_categories_Name",
                table: "categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_categories_UpdateDate",
                table: "categories",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_clients_Phone",
                table: "clients",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clients_UpdateDate",
                table: "clients",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_colors_Name",
                table: "colors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_colors_UpdateDate",
                table: "colors",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_deletedIds_TableName_EntityId",
                table: "deletedIds",
                columns: new[] { "TableName", "EntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_deletedIds_UpdateDate",
                table: "deletedIds",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_furniture_CategoryId",
                table: "furniture",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_furniture_Name",
                table: "furniture",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_furniture_UpdateDate",
                table: "furniture",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_furnitureCompositions_Entity2Id",
                table: "furnitureCompositions",
                column: "Entity2Id");

            migrationBuilder.CreateIndex(
                name: "IX_furnitureCompositions_UpdateDate",
                table: "furnitureCompositions",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_materials_Name",
                table: "materials",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_materials_UpdateDate",
                table: "materials",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_operations_OperationTypeId",
                table: "operations",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_operations_OrderId",
                table: "operations",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_operations_PartId",
                table: "operations",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_operations_UpdateDate",
                table: "operations",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_operationTypes_Name",
                table: "operationTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operationTypes_UpdateDate",
                table: "operationTypes",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_orderCompositions_Entity2Id",
                table: "orderCompositions",
                column: "Entity2Id");

            migrationBuilder.CreateIndex(
                name: "IX_orderCompositions_UpdateDate",
                table: "orderCompositions",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_orders_ClientId",
                table: "orders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_UpdateDate",
                table: "orders",
                column: "UpdateDate");

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
                name: "IX_parts_UpdateDate",
                table: "parts",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_prices_PartId",
                table: "prices",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_prices_UpdateDate",
                table: "prices",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_snapshots_PartId",
                table: "snapshots",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_snapshots_UpdateDate",
                table: "snapshots",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_statusChanges_Entity2Id",
                table: "statusChanges",
                column: "Entity2Id");

            migrationBuilder.CreateIndex(
                name: "IX_statusChanges_UpdateDate",
                table: "statusChanges",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_statuses_Name",
                table: "statuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_statuses_UpdateDate",
                table: "statuses",
                column: "UpdateDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "deletedIds");

            migrationBuilder.DropTable(
                name: "furnitureCompositions");

            migrationBuilder.DropTable(
                name: "operations");

            migrationBuilder.DropTable(
                name: "orderCompositions");

            migrationBuilder.DropTable(
                name: "prices");

            migrationBuilder.DropTable(
                name: "snapshots");

            migrationBuilder.DropTable(
                name: "statusChanges");

            migrationBuilder.DropTable(
                name: "operationTypes");

            migrationBuilder.DropTable(
                name: "furniture");

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

            migrationBuilder.DropTable(
                name: "clients");
        }
    }
}
