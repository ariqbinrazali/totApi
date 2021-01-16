using Microsoft.EntityFrameworkCore.Migrations;

namespace api2.Migrations
{
    public partial class addDepartmentAtStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentID",
                table: "Student",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Student_DepartmentID",
                table: "Student",
                column: "DepartmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Departments_DepartmentID",
                table: "Student",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Departments_DepartmentID",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_DepartmentID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                table: "Student");
        }
    }
}
