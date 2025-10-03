using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPlace.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MARKETPLACE_Brand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_Brand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MARKETPLACE_Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_Category_MARKETPLACE_Category_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "MARKETPLACE_Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MARKETPLACE_Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MARKETPLACE_VariationType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_VariationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MARKETPLACE_Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MRPPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Variations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_Product_MARKETPLACE_Brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "MARKETPLACE_Brand",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_Product_MARKETPLACE_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "MARKETPLACE_Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MARKETPLACE_Auction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuyerCompanyId = table.Column<int>(type: "int", nullable: false),
                    BuyerUserId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_Auction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_Auction_MARKETPLACE_Company_BuyerCompanyId",
                        column: x => x.BuyerCompanyId,
                        principalTable: "MARKETPLACE_Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MARKETPLACE_Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BuyerCompanyId = table.Column<int>(type: "int", nullable: false),
                    SellerCompanyId = table.Column<int>(type: "int", nullable: false),
                    BuyerUserId = table.Column<int>(type: "int", nullable: false),
                    SellerUserId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_Order_MARKETPLACE_Company_BuyerCompanyId",
                        column: x => x.BuyerCompanyId,
                        principalTable: "MARKETPLACE_Company",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_Order_MARKETPLACE_Company_SellerCompanyId",
                        column: x => x.SellerCompanyId,
                        principalTable: "MARKETPLACE_Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MARKETPLACE_User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_User_MARKETPLACE_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "MARKETPLACE_Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MARKETPLACE_VariationValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VariationTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VariationTypeId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_VariationValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_VariationValue_MARKETPLACE_VariationType_VariationTypeId",
                        column: x => x.VariationTypeId,
                        principalTable: "MARKETPLACE_VariationType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_VariationValue_MARKETPLACE_VariationType_VariationTypeId1",
                        column: x => x.VariationTypeId1,
                        principalTable: "MARKETPLACE_VariationType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MARKETPLACE_ProductCompany",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_ProductCompany", x => new { x.CompanyId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_ProductCompany_MARKETPLACE_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "MARKETPLACE_Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_ProductCompany_MARKETPLACE_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "MARKETPLACE_Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MARKETPLACE_OrderProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Variations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_OrderProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_OrderProduct_MARKETPLACE_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "MARKETPLACE_Order",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_OrderProduct_MARKETPLACE_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "MARKETPLACE_Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MARKETPLACE_AuctionProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuctionId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    AcceptedBidId = table.Column<int>(type: "int", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Variations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_AuctionProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_AuctionProduct_MARKETPLACE_Auction_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "MARKETPLACE_Auction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_AuctionProduct_MARKETPLACE_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "MARKETPLACE_Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MARKETPLACE_Bid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuctionId = table.Column<int>(type: "int", nullable: false),
                    AuctionProductId = table.Column<int>(type: "int", nullable: false),
                    SellerCompanyId = table.Column<int>(type: "int", nullable: false),
                    SellerUserId = table.Column<int>(type: "int", nullable: false),
                    OfferedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_Bid", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_Bid_MARKETPLACE_AuctionProduct_AuctionProductId",
                        column: x => x.AuctionProductId,
                        principalTable: "MARKETPLACE_AuctionProduct",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_Bid_MARKETPLACE_Auction_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "MARKETPLACE_Auction",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_Bid_MARKETPLACE_User_SellerCompanyId",
                        column: x => x.SellerCompanyId,
                        principalTable: "MARKETPLACE_User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Auction_BuyerCompanyId",
                table: "MARKETPLACE_Auction",
                column: "BuyerCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Auction_BuyerUserId",
                table: "MARKETPLACE_Auction",
                column: "BuyerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Auction_EndTime",
                table: "MARKETPLACE_Auction",
                column: "EndTime");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Auction_Status",
                table: "MARKETPLACE_Auction",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_AuctionProduct_AcceptedBidId",
                table: "MARKETPLACE_AuctionProduct",
                column: "AcceptedBidId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_AuctionProduct_AuctionId",
                table: "MARKETPLACE_AuctionProduct",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_AuctionProduct_ProductId",
                table: "MARKETPLACE_AuctionProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Bid_AuctionId",
                table: "MARKETPLACE_Bid",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Bid_AuctionProductId",
                table: "MARKETPLACE_Bid",
                column: "AuctionProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Bid_SellerCompanyId",
                table: "MARKETPLACE_Bid",
                column: "SellerCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Bid_SellerUserId",
                table: "MARKETPLACE_Bid",
                column: "SellerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Bid_Status",
                table: "MARKETPLACE_Bid",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Brand_IsActive",
                table: "MARKETPLACE_Brand",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Category_IsActive",
                table: "MARKETPLACE_Category",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Category_ParentCategoryId",
                table: "MARKETPLACE_Category",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Company_Email",
                table: "MARKETPLACE_Company",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Company_IsActive",
                table: "MARKETPLACE_Company",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Company_Phone",
                table: "MARKETPLACE_Company",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Order_BuyerCompanyId",
                table: "MARKETPLACE_Order",
                column: "BuyerCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Order_BuyerUserId",
                table: "MARKETPLACE_Order",
                column: "BuyerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Order_OrderDate",
                table: "MARKETPLACE_Order",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Order_OrderId",
                table: "MARKETPLACE_Order",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Order_SellerCompanyId",
                table: "MARKETPLACE_Order",
                column: "SellerCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Order_SellerUserId",
                table: "MARKETPLACE_Order",
                column: "SellerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Order_Status",
                table: "MARKETPLACE_Order",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_OrderProduct_OrderId",
                table: "MARKETPLACE_OrderProduct",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_OrderProduct_ProductId",
                table: "MARKETPLACE_OrderProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Product_BrandId",
                table: "MARKETPLACE_Product",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Product_CategoryId",
                table: "MARKETPLACE_Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Product_IsActive",
                table: "MARKETPLACE_Product",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Product_Name",
                table: "MARKETPLACE_Product",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_ProductCompany_ProductId",
                table: "MARKETPLACE_ProductCompany",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_User_CompanyId",
                table: "MARKETPLACE_User",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_User_Email",
                table: "MARKETPLACE_User",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_User_IsActive",
                table: "MARKETPLACE_User",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_User_Phone",
                table: "MARKETPLACE_User",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_User_Username",
                table: "MARKETPLACE_User",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_VariationType_IsActive",
                table: "MARKETPLACE_VariationType",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_VariationValue_IsActive",
                table: "MARKETPLACE_VariationValue",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_VariationValue_VariationTypeId",
                table: "MARKETPLACE_VariationValue",
                column: "VariationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_VariationValue_VariationTypeId1",
                table: "MARKETPLACE_VariationValue",
                column: "VariationTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MARKETPLACE_AuctionProduct_MARKETPLACE_Bid_AcceptedBidId",
                table: "MARKETPLACE_AuctionProduct",
                column: "AcceptedBidId",
                principalTable: "MARKETPLACE_Bid",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MARKETPLACE_Auction_MARKETPLACE_Company_BuyerCompanyId",
                table: "MARKETPLACE_Auction");

            migrationBuilder.DropForeignKey(
                name: "FK_MARKETPLACE_User_MARKETPLACE_Company_CompanyId",
                table: "MARKETPLACE_User");

            migrationBuilder.DropForeignKey(
                name: "FK_MARKETPLACE_AuctionProduct_MARKETPLACE_Auction_AuctionId",
                table: "MARKETPLACE_AuctionProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_MARKETPLACE_Bid_MARKETPLACE_Auction_AuctionId",
                table: "MARKETPLACE_Bid");

            migrationBuilder.DropForeignKey(
                name: "FK_MARKETPLACE_AuctionProduct_MARKETPLACE_Bid_AcceptedBidId",
                table: "MARKETPLACE_AuctionProduct");

            migrationBuilder.DropTable(
                name: "MARKETPLACE_OrderProduct");

            migrationBuilder.DropTable(
                name: "MARKETPLACE_ProductCompany");

            migrationBuilder.DropTable(
                name: "MARKETPLACE_VariationValue");

            migrationBuilder.DropTable(
                name: "MARKETPLACE_Order");

            migrationBuilder.DropTable(
                name: "MARKETPLACE_VariationType");

            migrationBuilder.DropTable(
                name: "MARKETPLACE_Company");

            migrationBuilder.DropTable(
                name: "MARKETPLACE_Auction");

            migrationBuilder.DropTable(
                name: "MARKETPLACE_Bid");

            migrationBuilder.DropTable(
                name: "MARKETPLACE_AuctionProduct");

            migrationBuilder.DropTable(
                name: "MARKETPLACE_User");

            migrationBuilder.DropTable(
                name: "MARKETPLACE_Product");

            migrationBuilder.DropTable(
                name: "MARKETPLACE_Brand");

            migrationBuilder.DropTable(
                name: "MARKETPLACE_Category");
        }
    }
}
