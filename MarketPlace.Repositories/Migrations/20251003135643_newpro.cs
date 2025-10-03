using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPlace.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class newpro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Variations",
                table: "MARKETPLACE_OrderProduct");

            migrationBuilder.DropColumn(
                name: "Variations",
                table: "MARKETPLACE_AuctionProduct");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Variations",
                table: "MARKETPLACE_OrderProduct",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Variations",
                table: "MARKETPLACE_AuctionProduct",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
