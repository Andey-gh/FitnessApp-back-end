using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessWebApp.Migrations
{
    public partial class FixMetricsModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthProblems_AspNetUsers_UserId",
                table: "HealthProblems");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "HealthProblems",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthProblems_AspNetUsers_UserId",
                table: "HealthProblems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthProblems_AspNetUsers_UserId",
                table: "HealthProblems");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "HealthProblems",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthProblems_AspNetUsers_UserId",
                table: "HealthProblems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
