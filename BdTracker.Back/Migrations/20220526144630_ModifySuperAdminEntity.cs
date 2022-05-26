using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BdTracker.Back.Migrations
{
    public partial class ModifySuperAdminEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "CompanyOwnerId", "Name" },
                values: new object[] { "506c4655-0053-414e-bb8f-9612876ab2a1", "25d733fa-b5ce-41fe-a868-beea7723a3e5", "BdTracker" });

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: "25d733fa-b5ce-41fe-a868-beea7723a3e5",
                columns: new[] { "CompanyId", "Email", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PositionName", "UserName" },
                values: new object[] { "506c4655-0053-414e-bb8f-9612876ab2a1", "super.admin@bdtracker.com", "Super", "SUPER.ADMIN@ADMIN.COM", "SUPERADMIN", "AQAAAAEAACcQAAAAENfsGLvH3eW8fdbsA4JidLp7p1gnmP1HukUsZ0EHQzKZ20TmxCd7LJmOYmiazZDMDw==", "Super Admin", "SuperAdmin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: "506c4655-0053-414e-bb8f-9612876ab2a1");

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: "25d733fa-b5ce-41fe-a868-beea7723a3e5",
                columns: new[] { "CompanyId", "Email", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PositionName", "UserName" },
                values: new object[] { null, "admin@admin.com", "Admin", "ADMIN@ADMIN.COM", "ADMIN", "AQAAAAEAACcQAAAAENCkLthTPd0r7CXtX5/yOEaaKYTHaxSS19gjdFYysigNskCv/encZ9iMB6heC6TPvA==", "Admin", "admin" });
        }
    }
}
