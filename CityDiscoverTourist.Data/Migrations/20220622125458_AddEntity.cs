using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class AddEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserSubscribeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscribeds", x => x.Id);
                });

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestRewards_Quests_QuestId",
                table: "QuestRewards");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_QuestRewards_QuestRewardId",
                table: "Rewards");

            migrationBuilder.DropTable(
                name: "UserSubscribeds");

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
                column: "Id");

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
