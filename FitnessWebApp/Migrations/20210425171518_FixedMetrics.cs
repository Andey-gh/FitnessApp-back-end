using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessWebApp.Migrations
{
    public partial class FixedMetrics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasMetrics",
                table: "AspNetUsers",
                newName: "IsMetrics");

            migrationBuilder.AlterColumn<string>(
                name: "Problem",
                table: "HealthProblems",
                type: "varchar(50) CHARACTER SET utf8mb4",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50) CHARACTER SET utf8mb4",
                oldMaxLength: 50);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsMetrics",
                table: "AspNetUsers",
                newName: "HasMetrics");

            migrationBuilder.AlterColumn<string>(
                name: "Problem",
                table: "HealthProblems",
                type: "varchar(50) CHARACTER SET utf8mb4",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50) CHARACTER SET utf8mb4",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
