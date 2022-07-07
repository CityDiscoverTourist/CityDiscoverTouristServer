using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class Remove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestRewards_Quests_QuestId",
                table: "QuestRewards");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_QuestRewards_QuestRewardId",
                table: "Rewards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestRewards",
                table: "QuestRewards");

            migrationBuilder.RenameTable(
                name: "QuestRewards",
                newName: "QuestReward");

            migrationBuilder.RenameIndex(
                name: "IX_QuestRewards_QuestId",
                table: "QuestReward",
                newName: "IX_QuestReward_QuestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestReward",
                table: "QuestReward",
                column: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestReward_Quests_QuestId",
                table: "QuestReward",
                column: "QuestId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_QuestReward_QuestRewardId",
                table: "Rewards",
                column: "QuestRewardId",
                principalTable: "QuestReward",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestReward_Quests_QuestId",
                table: "QuestReward");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_QuestReward_QuestRewardId",
                table: "Rewards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestReward",
                table: "QuestReward");

            migrationBuilder.RenameTable(
                name: "QuestReward",
                newName: "QuestRewards");

            migrationBuilder.RenameIndex(
                name: "IX_QuestReward_QuestId",
                table: "QuestRewards",
                newName: "IX_QuestRewards_QuestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestRewards",
                table: "QuestRewards",
                column: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestRewards_Quests_QuestId",
                table: "QuestRewards",
                column: "QuestId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_QuestRewards_QuestRewardId",
                table: "Rewards",
                column: "QuestRewardId",
                principalTable: "QuestRewards",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
