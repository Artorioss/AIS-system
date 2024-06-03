using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WpfAppMVVM.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingNumbersKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarDriver_Cars_CarsCarId",
                table: "CarDriver");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverTrailler_Traillers_TraillersTraillerId",
                table: "DriverTrailler");

            migrationBuilder.DropForeignKey(
                name: "FK_Transportations_Brands_CarBrandId",
                table: "Transportations");

            migrationBuilder.DropForeignKey(
                name: "FK_Transportations_Brands_TraillerBrandId",
                table: "Transportations");

            migrationBuilder.DropForeignKey(
                name: "FK_Transportations_Cars_CarId",
                table: "Transportations");

            migrationBuilder.DropForeignKey(
                name: "FK_Transportations_Traillers_TraillerId",
                table: "Transportations");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_CarBrandId",
                table: "Transportations");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_CarId",
                table: "Transportations");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_TraillerBrandId",
                table: "Transportations");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_TraillerId",
                table: "Transportations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Traillers",
                table: "Traillers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverTrailler",
                table: "DriverTrailler");

            migrationBuilder.DropIndex(
                name: "IX_DriverTrailler_TraillersTraillerId",
                table: "DriverTrailler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cars",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarDriver",
                table: "CarDriver");

            migrationBuilder.DropColumn(
                name: "CarBrandId",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "TraillerBrandId",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "TraillerId",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "TraillerId",
                table: "Traillers");

            migrationBuilder.DropColumn(
                name: "TraillersTraillerId",
                table: "DriverTrailler");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CarsCarId",
                table: "CarDriver");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TransportCompanies",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "RouteName",
                table: "Transportations",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldCollation: "default");

            migrationBuilder.AddColumn<string>(
                name: "CarNumber",
                table: "Transportations",
                type: "character varying(9)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TraillerNumber",
                table: "Transportations",
                type: "character varying(8)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Traillers",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StateOrders",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "RouteName",
                table: "Routes",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RoutePoints",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldCollation: "default");

            migrationBuilder.AddColumn<string>(
                name: "TraillersNumber",
                table: "DriverTrailler",
                type: "character varying(8)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Drivers",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Customers",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Cars",
                type: "character varying(9)",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(9)",
                oldMaxLength: 9,
                oldCollation: "default");

            migrationBuilder.AddColumn<string>(
                name: "CarsNumber",
                table: "CarDriver",
                type: "character varying(9)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "RussianBrandName",
                table: "Brands",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Brands",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldCollation: "default");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Traillers",
                table: "Traillers",
                column: "Number");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverTrailler",
                table: "DriverTrailler",
                columns: new[] { "DriversDriverId", "TraillersNumber" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cars",
                table: "Cars",
                column: "Number");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarDriver",
                table: "CarDriver",
                columns: new[] { "CarsNumber", "DriversDriverId" });

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_CarNumber",
                table: "Transportations",
                column: "CarNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_TraillerNumber",
                table: "Transportations",
                column: "TraillerNumber");

            migrationBuilder.CreateIndex(
                name: "IX_DriverTrailler_TraillersNumber",
                table: "DriverTrailler",
                column: "TraillersNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_CarDriver_Cars_CarsNumber",
                table: "CarDriver",
                column: "CarsNumber",
                principalTable: "Cars",
                principalColumn: "Number",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverTrailler_Traillers_TraillersNumber",
                table: "DriverTrailler",
                column: "TraillersNumber",
                principalTable: "Traillers",
                principalColumn: "Number",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transportations_Cars_CarNumber",
                table: "Transportations",
                column: "CarNumber",
                principalTable: "Cars",
                principalColumn: "Number");

            migrationBuilder.AddForeignKey(
                name: "FK_Transportations_Traillers_TraillerNumber",
                table: "Transportations",
                column: "TraillerNumber",
                principalTable: "Traillers",
                principalColumn: "Number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarDriver_Cars_CarsNumber",
                table: "CarDriver");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverTrailler_Traillers_TraillersNumber",
                table: "DriverTrailler");

            migrationBuilder.DropForeignKey(
                name: "FK_Transportations_Cars_CarNumber",
                table: "Transportations");

            migrationBuilder.DropForeignKey(
                name: "FK_Transportations_Traillers_TraillerNumber",
                table: "Transportations");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_CarNumber",
                table: "Transportations");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_TraillerNumber",
                table: "Transportations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Traillers",
                table: "Traillers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverTrailler",
                table: "DriverTrailler");

            migrationBuilder.DropIndex(
                name: "IX_DriverTrailler_TraillersNumber",
                table: "DriverTrailler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cars",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarDriver",
                table: "CarDriver");

            migrationBuilder.DropColumn(
                name: "CarNumber",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "TraillerNumber",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "TraillersNumber",
                table: "DriverTrailler");

            migrationBuilder.DropColumn(
                name: "CarsNumber",
                table: "CarDriver");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TransportCompanies",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "RouteName",
                table: "Transportations",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AddColumn<int>(
                name: "CarBrandId",
                table: "Transportations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Transportations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TraillerBrandId",
                table: "Transportations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TraillerId",
                table: "Transportations",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Traillers",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8);

            migrationBuilder.AddColumn<int>(
                name: "TraillerId",
                table: "Traillers",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StateOrders",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "RouteName",
                table: "Routes",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RoutePoints",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AddColumn<int>(
                name: "TraillersTraillerId",
                table: "DriverTrailler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Drivers",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Customers",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Cars",
                type: "character varying(9)",
                maxLength: 9,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "character varying(9)",
                oldMaxLength: 9);

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Cars",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "CarsCarId",
                table: "CarDriver",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "RussianBrandName",
                table: "Brands",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Brands",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Traillers",
                table: "Traillers",
                column: "TraillerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverTrailler",
                table: "DriverTrailler",
                columns: new[] { "DriversDriverId", "TraillersTraillerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cars",
                table: "Cars",
                column: "CarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarDriver",
                table: "CarDriver",
                columns: new[] { "CarsCarId", "DriversDriverId" });

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_CarBrandId",
                table: "Transportations",
                column: "CarBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_CarId",
                table: "Transportations",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_TraillerBrandId",
                table: "Transportations",
                column: "TraillerBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_TraillerId",
                table: "Transportations",
                column: "TraillerId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverTrailler_TraillersTraillerId",
                table: "DriverTrailler",
                column: "TraillersTraillerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarDriver_Cars_CarsCarId",
                table: "CarDriver",
                column: "CarsCarId",
                principalTable: "Cars",
                principalColumn: "CarId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverTrailler_Traillers_TraillersTraillerId",
                table: "DriverTrailler",
                column: "TraillersTraillerId",
                principalTable: "Traillers",
                principalColumn: "TraillerId",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Transportations_Cars_CarId",
                table: "Transportations",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transportations_Traillers_TraillerId",
                table: "Transportations",
                column: "TraillerId",
                principalTable: "Traillers",
                principalColumn: "TraillerId");
        }
    }
}
