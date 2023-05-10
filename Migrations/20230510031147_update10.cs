using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace timviec.Migrations
{
    /// <inheritdoc />
    public partial class update10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "time",
                table: "applies",
                newName: "Time");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "applies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "applies");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "applies",
                newName: "time");
        }
    }
}
