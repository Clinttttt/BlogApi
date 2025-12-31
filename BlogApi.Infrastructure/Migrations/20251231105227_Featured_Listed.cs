using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Featured_Listed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Featureds_PostId",
                table: "Featureds");

            migrationBuilder.CreateIndex(
                name: "IX_Featureds_PostId",
                table: "Featureds",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Featureds_PostId",
                table: "Featureds");

            migrationBuilder.CreateIndex(
                name: "IX_Featureds_PostId",
                table: "Featureds",
                column: "PostId",
                unique: true);
        }
    }
}
