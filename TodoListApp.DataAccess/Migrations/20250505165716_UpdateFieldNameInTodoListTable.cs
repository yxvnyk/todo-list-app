using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.WebApi.Migrations
{
    public partial class UpdateFieldNameInTodoListTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder, nameof(migrationBuilder));
            _ = migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TodoLists",
                newName: "OwnerId");

            _ = migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Tasks",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: string.Empty);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder, nameof(migrationBuilder));
            _ = migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Tasks");

            _ = migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "TodoLists",
                newName: "UserId");
        }
    }
}
