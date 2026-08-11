using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeSim.Migrations
{
    /// <inheritdoc />
    public partial class AddSectorToTradableInstrument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sector",
                table: "TradableInstruments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sector",
                table: "TradableInstruments");
        }
    }
}
