using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfAppMVVM.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingTransportationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "Transportations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "Transportations");
        }
    }
}
