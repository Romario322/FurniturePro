using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurniturePro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_statuses_UpdateDate",
                table: "statuses",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_statusChanges_UpdateDate",
                table: "statusChanges",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_snapshots_UpdateDate",
                table: "snapshots",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_prices_UpdateDate",
                table: "prices",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_parts_UpdateDate",
                table: "parts",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_orders_UpdateDate",
                table: "orders",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_orderCompositions_UpdateDate",
                table: "orderCompositions",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_operationTypes_UpdateDate",
                table: "operationTypes",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_operations_UpdateDate",
                table: "operations",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_materials_UpdateDate",
                table: "materials",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_furnitureCompositions_UpdateDate",
                table: "furnitureCompositions",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_furniture_UpdateDate",
                table: "furniture",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_deletedIds_UpdateDate",
                table: "deletedIds",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_colors_UpdateDate",
                table: "colors",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_clients_UpdateDate",
                table: "clients",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_categories_UpdateDate",
                table: "categories",
                column: "UpdateDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_statuses_UpdateDate",
                table: "statuses");

            migrationBuilder.DropIndex(
                name: "IX_statusChanges_UpdateDate",
                table: "statusChanges");

            migrationBuilder.DropIndex(
                name: "IX_snapshots_UpdateDate",
                table: "snapshots");

            migrationBuilder.DropIndex(
                name: "IX_prices_UpdateDate",
                table: "prices");

            migrationBuilder.DropIndex(
                name: "IX_parts_UpdateDate",
                table: "parts");

            migrationBuilder.DropIndex(
                name: "IX_orders_UpdateDate",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orderCompositions_UpdateDate",
                table: "orderCompositions");

            migrationBuilder.DropIndex(
                name: "IX_operationTypes_UpdateDate",
                table: "operationTypes");

            migrationBuilder.DropIndex(
                name: "IX_operations_UpdateDate",
                table: "operations");

            migrationBuilder.DropIndex(
                name: "IX_materials_UpdateDate",
                table: "materials");

            migrationBuilder.DropIndex(
                name: "IX_furnitureCompositions_UpdateDate",
                table: "furnitureCompositions");

            migrationBuilder.DropIndex(
                name: "IX_furniture_UpdateDate",
                table: "furniture");

            migrationBuilder.DropIndex(
                name: "IX_deletedIds_UpdateDate",
                table: "deletedIds");

            migrationBuilder.DropIndex(
                name: "IX_colors_UpdateDate",
                table: "colors");

            migrationBuilder.DropIndex(
                name: "IX_clients_UpdateDate",
                table: "clients");

            migrationBuilder.DropIndex(
                name: "IX_categories_UpdateDate",
                table: "categories");
        }
    }
}
