using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class AddQuestRewardEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_Quests_QuestId",
                table: "Rewards");

            migrationBuilder.AlterColumn<int>(
                name: "QuestId",
                table: "Rewards",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "QuestRewardId",
                table: "Rewards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "QuestReward",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PercentDiscount = table.Column<int>(type: "int", nullable: false),
                    PercentPointRemain = table.Column<int>(type: "int", nullable: false),
                    QuestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestReward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestReward_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_QuestRewardId",
                table: "Rewards",
                column: "QuestRewardId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestReward_QuestId",
                table: "QuestReward",
                column: "QuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_QuestReward_QuestRewardId",
                table: "Rewards",
                column: "QuestRewardId",
                principalTable: "QuestReward",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_Quests_QuestId",
                table: "Rewards",
                column: "QuestId",
                principalTable: "Quests",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_QuestReward_QuestRewardId",
                table: "Rewards");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_Quests_QuestId",
                table: "Rewards");

            migrationBuilder.DropTable(
                name: "QuestReward");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_QuestRewardId",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "QuestRewardId",
                table: "Rewards");

            migrationBuilder.AlterColumn<int>(
                name: "QuestId",
                table: "Rewards",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_Quests_QuestId",
                table: "Rewards",
                column: "QuestId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
