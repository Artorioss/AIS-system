using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WpfAppMVVM.Migrations
{
    /// <inheritdoc />
    public partial class AdditingPaymentMethodToTransportationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "Transportations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    PaymentMethodId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.PaymentMethodId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_PaymentMethodId",
                table: "Transportations",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transportations_PaymentMethods_PaymentMethodId",
                table: "Transportations",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "PaymentMethodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transportations_PaymentMethods_PaymentMethodId",
                table: "Transportations");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_PaymentMethodId",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "Transportations");
        }
    }
}
