using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class DefaultValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "QuestItems",
                type: "int",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateIndex(
                name: "IX_QuestItems_ItemId",
                table: "QuestItems",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestItems_QuestItems_ItemId",
                table: "QuestItems",
                column: "ItemId",
                principalTable: "QuestItems",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestItems_QuestItems_ItemId",
                table: "QuestItems");

            migrationBuilder.DropIndex(
                name: "IX_QuestItems_ItemId",
                table: "QuestItems");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "QuestItems");
        }
    }
}