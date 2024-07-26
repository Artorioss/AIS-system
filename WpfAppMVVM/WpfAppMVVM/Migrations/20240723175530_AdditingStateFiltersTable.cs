using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WpfAppMVVM.Migrations
{
    /// <inheritdoc />
    public partial class AdditingStateFiltersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StateFilter",
                columns: table => new
                {
                    StateFilterId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateFilter", x => x.StateFilterId);
                });

            migrationBuilder.CreateTable(
                name: "StateFilterStateOrder",
                columns: table => new
                {
                    StateFiltersStateFilterId = table.Column<int>(type: "integer", nullable: false),
                    StateOrdersStateOrderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateFilterStateOrder", x => new { x.StateFiltersStateFilterId, x.StateOrdersStateOrderId });
                    table.ForeignKey(
                        name: "FK_StateFilterStateOrder_StateFilter_StateFiltersStateFilterId",
                        column: x => x.StateFiltersStateFilterId,
                        principalTable: "StateFilter",
                        principalColumn: "StateFilterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StateFilterStateOrder_StateOrders_StateOrdersStateOrderId",
                        column: x => x.StateOrdersStateOrderId,
                        principalTable: "StateOrders",
                        principalColumn: "StateOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StateFilterStateOrder_StateOrdersStateOrderId",
                table: "StateFilterStateOrder",
                column: "StateOrdersStateOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StateFilterStateOrder");

            migrationBuilder.DropTable(
                name: "StateFilter");
        }
    }
}
