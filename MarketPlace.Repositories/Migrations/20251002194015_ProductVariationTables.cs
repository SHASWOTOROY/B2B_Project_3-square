using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPlace.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class ProductVariationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Variations",
                table: "MARKETPLACE_Product");

            migrationBuilder.CreateTable(
                name: "MARKETPLACE_ProductVariationType",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_ProductVariationType", x => new { x.ProductId, x.TypeId });
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_ProductVariationType_MARKETPLACE_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "MARKETPLACE_Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MARKETPLACE_ProductVariationValue",
                columns: table => new
                {
                    ValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_ProductVariationValue", x => new { x.ProductId, x.TypeId, x.ValueId });
                    table.ForeignKey(
                        name: "FK_MARKETPLACE_ProductVariationValue_MARKETPLACE_ProductVariationType_ProductId_TypeId",
                        columns: x => new { x.ProductId, x.TypeId },
                        principalTable: "MARKETPLACE_ProductVariationType",
                        principalColumns: new[] { "ProductId", "TypeId" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MARKETPLACE_ProductVariationValue");

            migrationBuilder.DropTable(
                name: "MARKETPLACE_ProductVariationType");

            migrationBuilder.AddColumn<string>(
                name: "Variations",
                table: "MARKETPLACE_Product",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
