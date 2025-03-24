using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionService.Migrations
{
    /// <inheritdoc />
    public partial class RenameFieldAuction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReversePrice",
                table: "Auctions",
                newName: "ReservePrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReservePrice",
                table: "Auctions",
                newName: "ReversePrice");
        }
    }
}
