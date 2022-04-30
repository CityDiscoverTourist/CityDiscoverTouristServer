using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class AllowNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_QuestOwners_QuestOwnerId",
                table: "Quests");

            migrationBuilder.AlterColumn<int>(
                name: "QuestOwnerId",
                table: "Quests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_QuestOwners_QuestOwnerId",
                table: "Quests",
                column: "QuestOwnerId",
                principalTable: "QuestOwners",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_QuestOwners_QuestOwnerId",
                table: "Quests");

            migrationBuilder.AlterColumn<int>(
                name: "QuestOwnerId",
                table: "Quests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_QuestOwners_QuestOwnerId",
                table: "Quests",
                column: "QuestOwnerId",
                principalTable: "QuestOwners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
