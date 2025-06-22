using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseApi.Migrations
{
    /// <inheritdoc />
    public partial class new_movement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Change",
                table: "StockMovements",
                newName: "ValueBefore");

            migrationBuilder.AddColumn<int>(
                name: "ValueAfter",
                table: "StockMovements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValueAfter",
                table: "StockMovements");

            migrationBuilder.RenameColumn(
                name: "ValueBefore",
                table: "StockMovements",
                newName: "Change");
        }
    }
}
