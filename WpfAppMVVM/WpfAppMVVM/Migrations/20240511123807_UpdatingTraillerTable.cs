using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfAppMVVM.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingTraillerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Traillers_CarBrands_CarBrandId",
                table: "Traillers");

            migrationBuilder.DropIndex(
                name: "IX_Traillers_CarBrandId",
                table: "Traillers");

            migrationBuilder.DropColumn(
                name: "CarBrandId",
                table: "Traillers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarBrandId",
                table: "Traillers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Traillers_CarBrandId",
                table: "Traillers",
                column: "CarBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Traillers_CarBrands_CarBrandId",
                table: "Traillers",
                column: "CarBrandId",
                principalTable: "CarBrands",
                principalColumn: "CarBrandId");
        }
    }
}
