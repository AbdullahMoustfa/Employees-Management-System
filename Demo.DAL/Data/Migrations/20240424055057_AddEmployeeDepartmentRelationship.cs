using Microsoft.EntityFrameworkCore.Migrations;

namespace Demo.DAL.Data.Migrations
{
    public partial class AddEmployeeDepartmentRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Emplpoyees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Emplpoyees_DepartmentId",
                table: "Emplpoyees",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emplpoyees_Departments_DepartmentId",
                table: "Emplpoyees",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emplpoyees_Departments_DepartmentId",
                table: "Emplpoyees");

            migrationBuilder.DropIndex(
                name: "IX_Emplpoyees_DepartmentId",
                table: "Emplpoyees");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Emplpoyees");
        }
    }
}
