using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPlace.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class pradd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MARKETPLACE_VariationValue_VariationTypeId",
                table: "MARKETPLACE_VariationValue");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_VariationValue_VariationTypeId_Name",
                table: "MARKETPLACE_VariationValue",
                columns: new[] { "VariationTypeId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_VariationType_Name",
                table: "MARKETPLACE_VariationType",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MARKETPLACE_VariationValue_VariationTypeId_Name",
                table: "MARKETPLACE_VariationValue");

            migrationBuilder.DropIndex(
                name: "IX_MARKETPLACE_VariationType_Name",
                table: "MARKETPLACE_VariationType");

            migrationBuilder.CreateIndex(
                name: "IX_MARKETPLACE_VariationValue_VariationTypeId",
                table: "MARKETPLACE_VariationValue",
                column: "VariationTypeId");
        }
    }
}
