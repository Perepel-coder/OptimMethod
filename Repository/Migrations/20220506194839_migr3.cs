using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class migr3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Values_DescriptionTask_TaskId",
                table: "Values");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DescriptionTask",
                table: "DescriptionTask");

            migrationBuilder.RenameTable(
                name: "DescriptionTask",
                newName: "Tasks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Values_Tasks_TaskId",
                table: "Values",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Values_Tasks_TaskId",
                table: "Values");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "DescriptionTask");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DescriptionTask",
                table: "DescriptionTask",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Values_DescriptionTask_TaskId",
                table: "Values",
                column: "TaskId",
                principalTable: "DescriptionTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
