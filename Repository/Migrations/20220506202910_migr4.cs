using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class migr4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Values_Tasks_TaskId",
                table: "Values");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "Values",
                newName: "DescriptionTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Values_Tasks_DescriptionTaskId",
                table: "Values",
                column: "DescriptionTaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Values_Tasks_DescriptionTaskId",
                table: "Values");

            migrationBuilder.RenameColumn(
                name: "DescriptionTaskId",
                table: "Values",
                newName: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Values_Tasks_TaskId",
                table: "Values",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
