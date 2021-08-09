using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessWebApp.Migrations
{
    public partial class AddedActivePlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivePlanId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ActivePlanId",
                table: "AspNetUsers",
                column: "ActivePlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TrainingPlans_ActivePlanId",
                table: "AspNetUsers",
                column: "ActivePlanId",
                principalTable: "TrainingPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TrainingPlans_ActivePlanId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ActivePlanId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ActivePlanId",
                table: "AspNetUsers");
        }
    }
}
