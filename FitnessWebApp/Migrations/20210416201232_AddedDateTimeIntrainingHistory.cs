using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessWebApp.Migrations
{
    public partial class AddedDateTimeIntrainingHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MuscleGroupId",
                table: "TrainingHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TrainingFinish",
                table: "TrainingHistories",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_TrainingHistories_MuscleGroupId",
                table: "TrainingHistories",
                column: "MuscleGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingHistories_MuscleGroups_MuscleGroupId",
                table: "TrainingHistories",
                column: "MuscleGroupId",
                principalTable: "MuscleGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingHistories_MuscleGroups_MuscleGroupId",
                table: "TrainingHistories");

            migrationBuilder.DropIndex(
                name: "IX_TrainingHistories_MuscleGroupId",
                table: "TrainingHistories");

            migrationBuilder.DropColumn(
                name: "MuscleGroupId",
                table: "TrainingHistories");

            migrationBuilder.DropColumn(
                name: "TrainingFinish",
                table: "TrainingHistories");
        }
    }
}
