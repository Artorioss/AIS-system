using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfAppMVVM.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingTransportationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Transportations_TransportationId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_CustomerId",
                table: "Transportations");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_DriverId",
                table: "Transportations");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_StateOrderId",
                table: "Transportations");

            migrationBuilder.DropIndex(
                name: "IX_Routes_TransportationId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "TransportationId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "IsTrailler",
                table: "Brands");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Transportations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                table: "Transportations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TraillerId",
                table: "Transportations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RouteRoutePoint",
                columns: table => new
                {
                    RoutePointsRoutePointId = table.Column<int>(type: "integer", nullable: false),
                    RoutesRouteId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteRoutePoint", x => new { x.RoutePointsRoutePointId, x.RoutesRouteId });
                    table.ForeignKey(
                        name: "FK_RouteRoutePoint_RoutePoints_RoutePointsRoutePointId",
                        column: x => x.RoutePointsRoutePointId,
                        principalTable: "RoutePoints",
                        principalColumn: "RoutePointId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteRoutePoint_Routes_RoutesRouteId",
                        column: x => x.RoutesRouteId,
                        principalTable: "Routes",
                        principalColumn: "RouteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_CarId",
                table: "Transportations",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_CustomerId",
                table: "Transportations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_DriverId",
                table: "Transportations",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_RouteId",
                table: "Transportations",
                column: "RouteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_StateOrderId",
                table: "Transportations",
                column: "StateOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_TraillerId",
                table: "Transportations",
                column: "TraillerId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteRoutePoint_RoutesRouteId",
                table: "RouteRoutePoint",
                column: "RoutesRouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transportations_Cars_CarId",
                table: "Transportations",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transportations_Routes_RouteId",
                table: "Transportations",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transportations_Traillers_TraillerId",
                table: "Transportations",
                column: "TraillerId",
                principalTable: "Traillers",
                principalColumn: "TraillerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transportations_Cars_CarId",
                table: "Transportations");

            migrationBuilder.DropForeignKey(
                name: "FK_Transportations_Routes_RouteId",
                table: "Transportations");

            migrationBuilder.DropForeignKey(
                name: "FK_Transportations_Traillers_TraillerId",
                table: "Transportations");

            migrationBuilder.DropTable(
                name: "RouteRoutePoint");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_CarId",
                table: "Transportations");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_CustomerId",
                table: "Transportations");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_DriverId",
                table: "Transportations");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_RouteId",
                table: "Transportations");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_StateOrderId",
                table: "Transportations");

            migrationBuilder.DropIndex(
                name: "IX_Transportations_TraillerId",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "TraillerId",
                table: "Transportations");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Transportations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransportationId",
                table: "Routes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrailler",
                table: "Brands",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_CustomerId",
                table: "Transportations",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_DriverId",
                table: "Transportations",
                column: "DriverId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_StateOrderId",
                table: "Transportations",
                column: "StateOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_TransportationId",
                table: "Routes",
                column: "TransportationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Transportations_TransportationId",
                table: "Routes",
                column: "TransportationId",
                principalTable: "Transportations",
                principalColumn: "TransportationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
