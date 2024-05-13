using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WpfAppMVVM.Migrations
{
    /// <inheritdoc />
    public partial class AdditingTraillerBrandsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TraillerBrandId",
                table: "Traillers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TraillerBrandId",
                table: "RussianBrandNames",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TraillerBrands",
                columns: table => new
                {
                    TraillerBrandId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraillerBrands", x => x.TraillerBrandId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Traillers_TraillerBrandId",
                table: "Traillers",
                column: "TraillerBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_RussianBrandNames_TraillerBrandId",
                table: "RussianBrandNames",
                column: "TraillerBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_RussianBrandNames_TraillerBrands_TraillerBrandId",
                table: "RussianBrandNames",
                column: "TraillerBrandId",
                principalTable: "TraillerBrands",
                principalColumn: "TraillerBrandId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Traillers_TraillerBrands_TraillerBrandId",
                table: "Traillers",
                column: "TraillerBrandId",
                principalTable: "TraillerBrands",
                principalColumn: "TraillerBrandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RussianBrandNames_TraillerBrands_TraillerBrandId",
                table: "RussianBrandNames");

            migrationBuilder.DropForeignKey(
                name: "FK_Traillers_TraillerBrands_TraillerBrandId",
                table: "Traillers");

            migrationBuilder.DropTable(
                name: "TraillerBrands");

            migrationBuilder.DropIndex(
                name: "IX_Traillers_TraillerBrandId",
                table: "Traillers");

            migrationBuilder.DropIndex(
                name: "IX_RussianBrandNames_TraillerBrandId",
                table: "RussianBrandNames");

            migrationBuilder.DropColumn(
                name: "TraillerBrandId",
                table: "Traillers");

            migrationBuilder.DropColumn(
                name: "TraillerBrandId",
                table: "RussianBrandNames");
        }
    }
}
