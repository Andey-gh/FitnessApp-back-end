using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessWebApp.Migrations
{
    public partial class AddedPlansCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "TrainingPlans",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "TrainingPlans");
        }
    }
}
