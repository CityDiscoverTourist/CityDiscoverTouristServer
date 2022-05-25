using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class AddKeyForQuestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestId",
                table: "CustomerQuests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerQuests_QuestId",
                table: "CustomerQuests",
                column: "QuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerQuests_Quests_QuestId",
                table: "CustomerQuests",
                column: "QuestId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerQuests_Quests_QuestId",
                table: "CustomerQuests");

            migrationBuilder.DropIndex(
                name: "IX_CustomerQuests_QuestId",
                table: "CustomerQuests");

            migrationBuilder.DropColumn(
                name: "QuestId",
                table: "CustomerQuests");
        }
    }
}
