using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class migr1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DescriptionTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DescriptionTask", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitsOfMeas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitsOfMeas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    UnitOfMeasId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parameters_UnitsOfMeas_UnitOfMeasId",
                        column: x => x.UnitOfMeasId,
                        principalTable: "UnitsOfMeas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Values",
                columns: table => new
                {
                    ParameterId = table.Column<int>(type: "INTEGER", nullable: false),
                    TaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Values", x => new { x.TaskId, x.ParameterId });
                    table.ForeignKey(
                        name: "FK_Values_DescriptionTask_TaskId",
                        column: x => x.TaskId,
                        principalTable: "DescriptionTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Values_Parameters_ParameterId",
                        column: x => x.ParameterId,
                        principalTable: "Parameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UnitsOfMeas",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "м" });

            migrationBuilder.InsertData(
                table: "UnitsOfMeas",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "кг/м^3" });

            migrationBuilder.InsertData(
                table: "UnitsOfMeas",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Дж/(кг·°С)" });

            migrationBuilder.InsertData(
                table: "UnitsOfMeas",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "°С" });

            migrationBuilder.InsertData(
                table: "UnitsOfMeas",
                columns: new[] { "Id", "Name" },
                values: new object[] { 5, "м/с" });

            migrationBuilder.InsertData(
                table: "UnitsOfMeas",
                columns: new[] { "Id", "Name" },
                values: new object[] { 6, "Па·с^n" });

            migrationBuilder.InsertData(
                table: "UnitsOfMeas",
                columns: new[] { "Id", "Name" },
                values: new object[] { 7, "1/°С" });

            migrationBuilder.InsertData(
                table: "UnitsOfMeas",
                columns: new[] { "Id", "Name" },
                values: new object[] { 8, "Вт/(м2·°С)" });

            migrationBuilder.InsertData(
                table: "UnitsOfMeas",
                columns: new[] { "Id", "Name" },
                values: new object[] { 9, "-" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Password", "Role" },
                values: new object[] { 1, "admin", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Password", "Role" },
                values: new object[] { 2, "user", "user", "user" });

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_UnitOfMeasId",
                table: "Parameters",
                column: "UnitOfMeasId");

            migrationBuilder.CreateIndex(
                name: "IX_Values_ParameterId",
                table: "Values",
                column: "ParameterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Values");

            migrationBuilder.DropTable(
                name: "DescriptionTask");

            migrationBuilder.DropTable(
                name: "Parameters");

            migrationBuilder.DropTable(
                name: "UnitsOfMeas");
        }
    }
}
