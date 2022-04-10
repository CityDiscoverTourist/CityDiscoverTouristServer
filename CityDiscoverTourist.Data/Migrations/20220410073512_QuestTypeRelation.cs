using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class QuestTypeRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_QuestTypes_QuestTypeId",
                table: "Quests");

            migrationBuilder.AlterColumn<int>(
                name: "QuestTypeId",
                table: "Quests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_QuestTypes_QuestTypeId",
                table: "Quests",
                column: "QuestTypeId",
                principalTable: "QuestTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_QuestTypes_QuestTypeId",
                table: "Quests");

            migrationBuilder.AlterColumn<int>(
                name: "QuestTypeId",
                table: "Quests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_QuestTypes_QuestTypeId",
                table: "Quests",
                column: "QuestTypeId",
                principalTable: "QuestTypes",
                principalColumn: "Id");
        }
    }
}
