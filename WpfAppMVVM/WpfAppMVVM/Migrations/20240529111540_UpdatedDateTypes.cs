using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfAppMVVM.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDateTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountDate",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Transportations");

            migrationBuilder.RenameColumn(
                name: "AccountName",
                table: "Transportations",
                newName: "RouteName");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateLoading",
                table: "Transportations",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "date",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RouteName",
                table: "Transportations",
                newName: "AccountName");

            migrationBuilder.AlterColumn<string>(
                name: "DateLoading",
                table: "Transportations",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountDate",
                table: "Transportations",
                type: "text",
                nullable: true,
                collation: "default");

            migrationBuilder.AddColumn<int>(
                name: "AccountNumber",
                table: "Transportations",
                type: "integer",
                nullable: true);
        }
    }
}
