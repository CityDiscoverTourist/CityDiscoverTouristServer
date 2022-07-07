using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestReward_Quests_QuestId",
                table: "QuestReward");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_QuestReward_QuestRewardId",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_QuestRewardId",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_QuestReward_QuestId",
                table: "QuestReward");

            migrationBuilder.DropColumn(
                name: "QuestId",
                table: "QuestReward");

            migrationBuilder.RenameColumn(
                name: "QuestRewardId",
                table: "Rewards",
                newName: "Code");

            migrationBuilder.AddColumn<int>(
                name: "PercentDiscount",
                table: "Rewards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_Code",
                table: "Rewards",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rewards_Code",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "PercentDiscount",
                table: "Rewards");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Rewards",
                newName: "QuestRewardId");

            migrationBuilder.AddColumn<int>(
                name: "QuestId",
                table: "QuestReward",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_QuestRewardId",
                table: "Rewards",
                column: "QuestRewardId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestReward_QuestId",
                table: "QuestReward",
                column: "QuestId");

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
    }
}
