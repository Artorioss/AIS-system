using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfAppMVVM.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingRouteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transportations_RouteId",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Routes");

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_RouteId",
                table: "Transportations",
                column: "RouteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transportations_RouteId",
                table: "Transportations");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Routes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_RouteId",
                table: "Transportations",
                column: "RouteId",
                unique: true);
        }
    }
}
