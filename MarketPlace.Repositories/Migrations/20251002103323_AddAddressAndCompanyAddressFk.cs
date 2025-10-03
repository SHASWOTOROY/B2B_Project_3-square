using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPlace.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressAndCompanyAddressFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "MARKETPLACE_Order");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "MARKETPLACE_Company");

            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "MARKETPLACE_Auction");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "MARKETPLACE_Company",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MARKETPLACE_Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstLine = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SecondLine = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARKETPLACE_Address", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Company_AddressId",
                table: "MARKETPLACE_Company",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_MARKETPLACE_Company_MARKETPLACE_Address_AddressId",
                table: "MARKETPLACE_Company",
                column: "AddressId",
                principalTable: "MARKETPLACE_Address",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MARKETPLACE_Company_MARKETPLACE_Address_AddressId",
                table: "MARKETPLACE_Company");

            migrationBuilder.DropTable(
                name: "MARKETPLACE_Address");

            migrationBuilder.DropIndex(
                name: "IX_MARKETPLACE_Company_AddressId",
                table: "MARKETPLACE_Company");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "MARKETPLACE_Company");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "MARKETPLACE_Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "MARKETPLACE_Company",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "MARKETPLACE_Auction",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
