using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessWebApp.Migrations
{
    public partial class AddedMuscleModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Excercises_AssistantMuscleId",
                table: "Excercises",
                column: "AssistantMuscleId");

            migrationBuilder.CreateIndex(
                name: "IX_Excercises_TargetMuscleId",
                table: "Excercises",
                column: "TargetMuscleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Excercises_Muscles_AssistantMuscleId",
                table: "Excercises",
                column: "AssistantMuscleId",
                principalTable: "Muscles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Excercises_Muscles_TargetMuscleId",
                table: "Excercises",
                column: "TargetMuscleId",
                principalTable: "Muscles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Excercises_Muscles_AssistantMuscleId",
                table: "Excercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Excercises_Muscles_TargetMuscleId",
                table: "Excercises");

            migrationBuilder.DropIndex(
                name: "IX_Excercises_AssistantMuscleId",
                table: "Excercises");

            migrationBuilder.DropIndex(
                name: "IX_Excercises_TargetMuscleId",
                table: "Excercises");
        }
    }
}
