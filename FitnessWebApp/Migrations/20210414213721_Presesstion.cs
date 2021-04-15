using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessWebApp.Migrations
{
    public partial class Presesstion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingPlans_MuscleGroups_MuscleGroupId",
                table: "TrainingPlans");

            migrationBuilder.DropIndex(
                name: "IX_TrainingPlans_MuscleGroupId",
                table: "TrainingPlans");

            migrationBuilder.DropColumn(
                name: "MuscleGroupId",
                table: "TrainingPlans");

            migrationBuilder.AddColumn<int>(
                name: "MuscleGroupId",
                table: "ExcercisesInPlan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExcercisesInPlan_MuscleGroupId",
                table: "ExcercisesInPlan",
                column: "MuscleGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExcercisesInPlan_MuscleGroups_MuscleGroupId",
                table: "ExcercisesInPlan",
                column: "MuscleGroupId",
                principalTable: "MuscleGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExcercisesInPlan_MuscleGroups_MuscleGroupId",
                table: "ExcercisesInPlan");

            migrationBuilder.DropIndex(
                name: "IX_ExcercisesInPlan_MuscleGroupId",
                table: "ExcercisesInPlan");

            migrationBuilder.DropColumn(
                name: "MuscleGroupId",
                table: "ExcercisesInPlan");

            migrationBuilder.AddColumn<int>(
                name: "MuscleGroupId",
                table: "TrainingPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlans_MuscleGroupId",
                table: "TrainingPlans",
                column: "MuscleGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingPlans_MuscleGroups_MuscleGroupId",
                table: "TrainingPlans",
                column: "MuscleGroupId",
                principalTable: "MuscleGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
