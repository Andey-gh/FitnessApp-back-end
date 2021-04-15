using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessWebApp.Migrations
{
    public partial class AddedPreSesstionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MuscleGroupId",
                table: "TrainingPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExcerciseId",
                table: "TrainingHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Excercises",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExcercisesInPlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    ExcerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcercisesInPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExcercisesInPlan_Excercises_ExcerciseId",
                        column: x => x.ExcerciseId,
                        principalTable: "Excercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExcercisesInPlan_TrainingPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "TrainingPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MuscleGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false),
                    Photo = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuscleGroups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlans_MuscleGroupId",
                table: "TrainingPlans",
                column: "MuscleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingHistories_ExcerciseId",
                table: "TrainingHistories",
                column: "ExcerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcercisesInPlan_ExcerciseId",
                table: "ExcercisesInPlan",
                column: "ExcerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcercisesInPlan_PlanId",
                table: "ExcercisesInPlan",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingHistories_Excercises_ExcerciseId",
                table: "TrainingHistories",
                column: "ExcerciseId",
                principalTable: "Excercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingPlans_MuscleGroups_MuscleGroupId",
                table: "TrainingPlans",
                column: "MuscleGroupId",
                principalTable: "MuscleGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingHistories_Excercises_ExcerciseId",
                table: "TrainingHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingPlans_MuscleGroups_MuscleGroupId",
                table: "TrainingPlans");

            migrationBuilder.DropTable(
                name: "ExcercisesInPlan");

            migrationBuilder.DropTable(
                name: "MuscleGroups");

            migrationBuilder.DropIndex(
                name: "IX_TrainingPlans_MuscleGroupId",
                table: "TrainingPlans");

            migrationBuilder.DropIndex(
                name: "IX_TrainingHistories_ExcerciseId",
                table: "TrainingHistories");

            migrationBuilder.DropColumn(
                name: "MuscleGroupId",
                table: "TrainingPlans");

            migrationBuilder.DropColumn(
                name: "ExcerciseId",
                table: "TrainingHistories");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Excercises",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);
        }
    }
}
