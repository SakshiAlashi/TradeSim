using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeSim.Migrations
{
    /// <inheritdoc />
    public partial class Up2TradableInstrument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "TradableInstruments");

            migrationBuilder.CreateTable(
                name: "UserWatchlistItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TradableInstrumentId = table.Column<int>(type: "int", nullable: false),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWatchlistItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWatchlistItems_TradableInstruments_TradableInstrumentId",
                        column: x => x.TradableInstrumentId,
                        principalTable: "TradableInstruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWatchlistItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWatchlistItems_TradableInstrumentId",
                table: "UserWatchlistItems",
                column: "TradableInstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWatchlistItems_UserId_TradableInstrumentId",
                table: "UserWatchlistItems",
                columns: new[] { "UserId", "TradableInstrumentId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserWatchlistItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "TradableInstruments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
