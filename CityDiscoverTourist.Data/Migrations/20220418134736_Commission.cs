using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class Commission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestNote_Quests_QuestId",
                table: "QuestNote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestNote",
                table: "QuestNote");

            migrationBuilder.RenameTable(
                name: "QuestNote",
                newName: "QuestNotes");

            migrationBuilder.RenameIndex(
                name: "IX_QuestNote_QuestId",
                table: "QuestNotes",
                newName: "IX_QuestNotes_QuestId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Tasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Tasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReceivedDate",
                table: "Rewards",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Quests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Quests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ActivityLogs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestNotes",
                table: "QuestNotes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestNotes_Quests_QuestId",
                table: "QuestNotes",
                column: "QuestId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestNotes_Quests_QuestId",
                table: "QuestNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestNotes",
                table: "QuestNotes");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Quests");

            migrationBuilder.RenameTable(
                name: "QuestNotes",
                newName: "QuestNote");

            migrationBuilder.RenameIndex(
                name: "IX_QuestNotes_QuestId",
                table: "QuestNote",
                newName: "IX_QuestNote_QuestId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReceivedDate",
                table: "Rewards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ActivityLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestNote",
                table: "QuestNote",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestNote_Quests_QuestId",
                table: "QuestNote",
                column: "QuestId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
