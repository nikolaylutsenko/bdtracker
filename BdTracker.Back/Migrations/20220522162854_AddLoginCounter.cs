using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BdTracker.Back.Migrations
{
    public partial class AddLoginCounter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoginCounter",
                table: "AppUser",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginCounter",
                table: "AppUser");
        }
    }
}
