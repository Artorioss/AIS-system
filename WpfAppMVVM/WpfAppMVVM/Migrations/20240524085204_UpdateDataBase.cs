using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfAppMVVM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TransportCompanies",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DateLoading",
                table: "Transportations",
                type: "text",
                nullable: true,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "Transportations",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AccountDate",
                table: "Transportations",
                type: "text",
                nullable: true,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Traillers",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StateOrders",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "RouteName",
                table: "Routes",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RoutePoints",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Drivers",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Customers",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Cars",
                type: "character varying(9)",
                maxLength: 9,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "RussianBrandName",
                table: "Brands",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Brands",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                collation: "default",
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TransportCompanies",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "DateLoading",
                table: "Transportations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "Transportations",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "AccountDate",
                table: "Transportations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Traillers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StateOrders",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "RouteName",
                table: "Routes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RoutePoints",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Drivers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Customers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Cars",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(9)",
                oldMaxLength: 9,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "RussianBrandName",
                table: "Brands",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldCollation: "default");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Brands",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldCollation: "default");
        }
    }
}
