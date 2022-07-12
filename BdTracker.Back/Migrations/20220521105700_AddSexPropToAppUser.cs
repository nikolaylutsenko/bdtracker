using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BdTracker.Back.Migrations
{
    public partial class AddSexPropToAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sex",
                table: "AppUser",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sex",
                table: "AppUser");
        }
    }
}
