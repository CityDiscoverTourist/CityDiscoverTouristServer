using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class ModifyEntiy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Quests_QuestId",
                table: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Areas_QuestId",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "QuestId",
                table: "Areas");

            migrationBuilder.AddColumn<string>(
                name: "TeamCode",
                table: "CustomerQuests",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamCode",
                table: "CustomerQuests");

            migrationBuilder.AddColumn<int>(
                name: "QuestId",
                table: "Areas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_QuestId",
                table: "Areas",
                column: "QuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Quests_QuestId",
                table: "Areas",
                column: "QuestId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
