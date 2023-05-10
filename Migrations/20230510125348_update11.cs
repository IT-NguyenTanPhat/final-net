using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace timviec.Migrations
{
    /// <inheritdoc />
    public partial class update11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_jobs_users_UserId",
                table: "jobs");

            migrationBuilder.DropIndex(
                name: "IX_jobs_UserId",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "jobs");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "users");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "jobs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_jobs_UserId",
                table: "jobs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_jobs_users_UserId",
                table: "jobs",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
