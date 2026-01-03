using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updateconfigg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Drop the foreign key first
            migrationBuilder.DropForeignKey(
                name: "FK_CommentLikes_Posts_PostId",
                table: "CommentLikes");

            // 2. Drop the index
            migrationBuilder.DropIndex(
                name: "IX_CommentLikes_PostId",
                table: "CommentLikes");

            // 3. Finally drop the column
            migrationBuilder.DropColumn(
                name: "PostId",
                table: "CommentLikes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse order for rollback
            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "CommentLikes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentLikes_PostId",
                table: "CommentLikes",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLikes_Posts_PostId",
                table: "CommentLikes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}