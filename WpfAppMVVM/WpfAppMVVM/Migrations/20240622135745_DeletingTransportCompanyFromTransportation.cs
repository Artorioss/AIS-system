using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfAppMVVM.Migrations
{
    /// <inheritdoc />
    public partial class DeletingTransportCompanyFromTransportation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transportations_TransportCompanies_TransportCompanyId",
                table: "Transportations");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_TransportCompanyId",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "TransportCompanyId",
                table: "Transportations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransportCompanyId",
                table: "Transportations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_TransportCompanyId",
                table: "Transportations",
                column: "TransportCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transportations_TransportCompanies_TransportCompanyId",
                table: "Transportations",
                column: "TransportCompanyId",
                principalTable: "TransportCompanies",
                principalColumn: "TransportCompanyId");
        }
    }
}
