using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BdTracker.Back.Migrations
{
    public partial class FixSuperAdminRoleSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppUserAppRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0a26e36f-1626-4298-9a97-34a8c4118e08", "25d733fa-b5ce-41fe-a868-beea7723a3e5" });

            migrationBuilder.InsertData(
                table: "AppUserAppRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "58f3dea3-67eb-4284-b4bd-e4504d8e523e", "25d733fa-b5ce-41fe-a868-beea7723a3e5" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppUserAppRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "58f3dea3-67eb-4284-b4bd-e4504d8e523e", "25d733fa-b5ce-41fe-a868-beea7723a3e5" });

            migrationBuilder.InsertData(
                table: "AppUserAppRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "0a26e36f-1626-4298-9a97-34a8c4118e08", "25d733fa-b5ce-41fe-a868-beea7723a3e5" });
        }
    }
}
