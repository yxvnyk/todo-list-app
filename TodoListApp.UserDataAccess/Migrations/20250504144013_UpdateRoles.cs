using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.UserDataAccess.Migrations
{
    public partial class UpdateRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "31c056e4-b724-45cb-bee4-83d24b04983f", "1b72376f-3e61-4feb-a2c9-126ae9df5cee", "Viewer", "VIEWER" },
                    { "558d88cf-146f-41a7-a1fb-5d576f88ca4d", "4acc443b-2d4a-4cd2-8da8-a291723a4d4a", "Owner", "OWNER" },
                    { "8222ce84-5aad-4914-90ef-588a53de9192", "e84781df-f604-4d3b-8a5a-4c72fc438c01", "User", "USER" },
                    { "8f10f1b0-7ab4-467e-bb00-df3c359c2d66", "5da74503-4205-4897-8389-658478cb05d9", "Editor", "EDITOR" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31c056e4-b724-45cb-bee4-83d24b04983f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "558d88cf-146f-41a7-a1fb-5d576f88ca4d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8222ce84-5aad-4914-90ef-588a53de9192");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f10f1b0-7ab4-467e-bb00-df3c359c2d66");

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
    }
}
