using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeSim.Migrations
{
    /// <inheritdoc />
    public partial class AddPortfolioHoldings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PortfolioHoldings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TradableInstrumentId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AverageBuyPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioHoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortfolioHoldings_TradableInstruments_TradableInstrumentId",
                        column: x => x.TradableInstrumentId,
                        principalTable: "TradableInstruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PortfolioHoldings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioHoldings_TradableInstrumentId",
                table: "PortfolioHoldings",
                column: "TradableInstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioHoldings_UserId_TradableInstrumentId",
                table: "PortfolioHoldings",
                columns: new[] { "UserId", "TradableInstrumentId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PortfolioHoldings");
        }
    }
}
