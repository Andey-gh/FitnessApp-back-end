using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessWebApp.Migrations
{
    public partial class FixMetricsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Goal",
                table: "AspNetUsers",
                type: "varchar(40) CHARACTER SET utf8mb4",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxPullUps",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxPushUps",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HealthProblems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Problem = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthProblems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthProblems_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthProblems_UserId",
                table: "HealthProblems",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthProblems");

            migrationBuilder.DropColumn(
                name: "Goal",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MaxPullUps",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MaxPushUps",
                table: "AspNetUsers");
        }
    }
}
