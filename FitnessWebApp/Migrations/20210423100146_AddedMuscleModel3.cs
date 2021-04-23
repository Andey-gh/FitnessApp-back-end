using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessWebApp.Migrations
{
    public partial class AddedMuscleModel3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Excercises_Muscles_AssistantMuscleId",
                table: "Excercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Excercises_Muscles_TargetMuscleId",
                table: "Excercises");

            migrationBuilder.AlterColumn<int>(
                name: "TargetMuscleId",
                table: "Excercises",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AssistantMuscleId",
                table: "Excercises",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Excercises_Muscles_AssistantMuscleId",
                table: "Excercises",
                column: "AssistantMuscleId",
                principalTable: "Muscles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Excercises_Muscles_TargetMuscleId",
                table: "Excercises",
                column: "TargetMuscleId",
                principalTable: "Muscles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Excercises_Muscles_AssistantMuscleId",
                table: "Excercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Excercises_Muscles_TargetMuscleId",
                table: "Excercises");

            migrationBuilder.AlterColumn<int>(
                name: "TargetMuscleId",
                table: "Excercises",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AssistantMuscleId",
                table: "Excercises",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
