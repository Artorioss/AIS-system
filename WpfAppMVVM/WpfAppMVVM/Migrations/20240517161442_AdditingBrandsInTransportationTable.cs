using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfAppMVVM.Migrations
{
    /// <inheritdoc />
    public partial class AdditingBrandsInTransportationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarBrandId",
                table: "Transportations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TraillerBrandId",
                table: "Transportations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_CarBrandId",
                table: "Transportations",
                column: "CarBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_TraillerBrandId",
                table: "Transportations",
                column: "TraillerBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transportations_Brands_CarBrandId",
                table: "Transportations",
                column: "CarBrandId",
                principalTable: "Brands",
                principalColumn: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transportations_Brands_TraillerBrandId",
                table: "Transportations",
                column: "TraillerBrandId",
                principalTable: "Brands",
                principalColumn: "BrandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transportations_Brands_CarBrandId",
                table: "Transportations");

            migrationBuilder.DropForeignKey(
                name: "FK_Transportations_Brands_TraillerBrandId",
                table: "Transportations");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_CarBrandId",
                table: "Transportations");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_TraillerBrandId",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "CarBrandId",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "TraillerBrandId",
                table: "Transportations");
        }
    }
}
