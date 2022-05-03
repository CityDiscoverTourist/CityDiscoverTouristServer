using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class RelationAreaQuest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "Quests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Quests_AreaId",
                table: "Quests",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_Areas_AreaId",
                table: "Quests",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Areas_AreaId",
                table: "Quests");

            migrationBuilder.DropIndex(
                name: "IX_Quests_AreaId",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Quests");
        }
    }
}
