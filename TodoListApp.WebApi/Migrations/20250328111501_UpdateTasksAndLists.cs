using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.WebApi.Migrations
{
    public partial class UpdateTasksAndLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Tasks");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TodoLists",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AssigneeId",
                table: "Tasks",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tasks",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssigneeId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "TodoLists",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Tasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
