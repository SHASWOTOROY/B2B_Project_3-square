using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPlace.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AuctionOrderAddressAndVariations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryAddressId",
                table: "MARKETPLACE_Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryAddressId",
                table: "MARKETPLACE_Auction",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MARKETPLACE_AuctionProductVariation",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ValueId = table.Column<int>(type: "int", nullable: false),
                    AuctionProductId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_AuctionProductVariation", x => new { x.AuctionProductId, x.TypeId, x.ValueId });
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_AuctionProductVariation_MARKETPLACE_AuctionProduct_AuctionProductId",
                        column: x => x.AuctionProductId,
                        principalTable: "MARKETPLACE_AuctionProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MARKETPLACE_OrderProductVariation",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ValueId = table.Column<int>(type: "int", nullable: false),
                    OrderProductId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_OrderProductVariation", x => new { x.OrderProductId, x.TypeId, x.ValueId });
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_OrderProductVariation_MARKETPLACE_OrderProduct_OrderProductId",
                        column: x => x.OrderProductId,
                        principalTable: "MARKETPLACE_OrderProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Order_DeliveryAddressId",
                table: "MARKETPLACE_Order",
                column: "DeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Auction_DeliveryAddressId",
                table: "MARKETPLACE_Auction",
                column: "DeliveryAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_MARKETPLACE_Auction_MARKETPLACE_Address_DeliveryAddressId",
                table: "MARKETPLACE_Auction",
                column: "DeliveryAddressId",
                principalTable: "MARKETPLACE_Address",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MARKETPLACE_Order_MARKETPLACE_Address_DeliveryAddressId",
                table: "MARKETPLACE_Order",
                column: "DeliveryAddressId",
                principalTable: "MARKETPLACE_Address",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MARKETPLACE_Auction_MARKETPLACE_Address_DeliveryAddressId",
                table: "MARKETPLACE_Auction");

            migrationBuilder.DropForeignKey(
                name: "FK_MARKETPLACE_Order_MARKETPLACE_Address_DeliveryAddressId",
                table: "MARKETPLACE_Order");

            migrationBuilder.DropTable(
                name: "MARKETPLACE_AuctionProductVariation");

            migrationBuilder.DropTable(
                name: "MARKETPLACE_OrderProductVariation");

            migrationBuilder.DropIndex(
                name: "IX_MARKETPLACE_Order_DeliveryAddressId",
                table: "MARKETPLACE_Order");

            migrationBuilder.DropIndex(
                name: "IX_MARKETPLACE_Auction_DeliveryAddressId",
                table: "MARKETPLACE_Auction");

            migrationBuilder.DropColumn(
                name: "DeliveryAddressId",
                table: "MARKETPLACE_Order");

            migrationBuilder.DropColumn(
                name: "DeliveryAddressId",
                table: "MARKETPLACE_Auction");
        }
    }
}
