using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfAppMVVM.Migrations
{
    /// <inheritdoc />
    public partial class AdditingSoftDeletedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "TransportCompanies",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "Transportations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "Traillers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "TraillerBrands",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "StateOrders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "Routes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "RoutePoints",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PaymentMethods",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "PaymentMethods",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "Drivers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "Customers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "Cars",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "CarBrands",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "TransportCompanies");

            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "Traillers");

            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "TraillerBrands");

            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "StateOrders");

            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "RoutePoints");

            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "CarBrands");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PaymentMethods",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(16)",
                oldMaxLength: 16);
        }
    }
}
