using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BdTracker.Back.Migrations
{
    public partial class AddOwnerRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6dbb9259-ba9f-41f0-bc67-af39787f7e0a", "6dbb9259-ba9f-41f0-bc67-af39787f7e0a", "Owner", "OWNER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: "6dbb9259-ba9f-41f0-bc67-af39787f7e0a");
        }
    }
}
