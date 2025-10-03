using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPlace.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexes_Brand_Category : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Category_Name",
                table: "MARKETPLACE_Category",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_Brand_Name",
                table: "MARKETPLACE_Brand",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MARKETPLACE_Category_Name",
                table: "MARKETPLACE_Category");

            migrationBuilder.DropIndex(
                name: "IX_MARKETPLACE_Brand_Name",
                table: "MARKETPLACE_Brand");
        }
    }
}
