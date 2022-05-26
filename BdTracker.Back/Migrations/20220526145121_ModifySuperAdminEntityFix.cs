using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BdTracker.Back.Migrations
{
    public partial class ModifySuperAdminEntityFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: "25d733fa-b5ce-41fe-a868-beea7723a3e5",
                column: "NormalizedEmail",
                value: "SUPER.ADMIN@BDTRACKER.COM");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: "25d733fa-b5ce-41fe-a868-beea7723a3e5",
                column: "NormalizedEmail",
                value: "SUPER.ADMIN@ADMIN.COM");
        }
    }
}
