using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.WebApi.Migrations
{
    public partial class UpdateTasksAndLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);
            _ = migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Tasks");

            _ = migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TodoLists",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            _ = migrationBuilder.AddColumn<string>(
                name: "AssigneeId",
                table: "Tasks",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: string.Empty);

            _ = migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tasks",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            _ = migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);
            _ = migrationBuilder.DropColumn(
                name: "AssigneeId",
                table: "Tasks");

            _ = migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Tasks");

            _ = migrationBuilder.DropColumn(
                name: "Description",
                table: "Tasks");

            _ = migrationBuilder.DropColumn(
                name: "Status",
                table: "Tasks");

            _ = migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "TodoLists",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            _ = migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Tasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
