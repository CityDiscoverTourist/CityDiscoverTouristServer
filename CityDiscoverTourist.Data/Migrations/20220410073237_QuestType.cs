using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class QuestType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableTime",
                table: "Quests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Quests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Quests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EstimateTime",
                table: "Quests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Quests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "Quests",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "QuestTypeId",
                table: "Quests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Quests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "QuestTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quests_QuestTypeId",
                table: "Quests",
                column: "QuestTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_QuestTypes_QuestTypeId",
                table: "Quests",
                column: "QuestTypeId",
                principalTable: "QuestTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_QuestTypes_QuestTypeId",
                table: "Quests");

            migrationBuilder.DropTable(
                name: "QuestTypes");

            migrationBuilder.DropIndex(
                name: "IX_Quests_QuestTypeId",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "AvailableTime",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "EstimateTime",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "QuestTypeId",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Quests");
        }
    }
}
