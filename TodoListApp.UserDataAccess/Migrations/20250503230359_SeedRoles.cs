using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.UserDataAccess.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3759a894-f8cd-4c25-b931-32b7dce9b511", "96902a95-3adc-4ae7-b1e7-1067133a3523", "Viewer", "Viewer" },
                    { "43dd47e0-fa8a-4335-905c-ee4f0fb4eb41", "a68e7d9d-13a1-46ac-bb15-5f500d788c1b", "Owner", "OWNER" },
                    { "459e60f0-8c97-4a06-a290-89f582ada7ec", "d02c3a9f-c8bc-447f-ad4a-bd53c10aeb7a", "Editor", "EDITOR" },
                    { "d644a7f8-a9c2-4396-86ee-0fe403c0d848", "5b1102d4-2617-4117-9152-7e7947e7b945", "User", "USER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3759a894-f8cd-4c25-b931-32b7dce9b511");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43dd47e0-fa8a-4335-905c-ee4f0fb4eb41");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "459e60f0-8c97-4a06-a290-89f582ada7ec");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d644a7f8-a9c2-4396-86ee-0fe403c0d848");
        }
    }
}
