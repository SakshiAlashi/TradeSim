using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeSim.Migrations
{
    /// <inheritdoc />
    public partial class TradableInstrument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisplayName",
                table: "TradableInstruments",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "InstrumentToken",
                table: "TradableInstruments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstrumentToken",
                table: "TradableInstruments");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TradableInstruments",
                newName: "DisplayName");
        }
    }
}
