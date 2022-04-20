using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class Comm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Quests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "QuestId",
                table: "Commissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Quests_CreatedBy",
                table: "Quests",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_QuestId",
                table: "Commissions",
                column: "QuestId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_Quests_QuestId",
                table: "Commissions",
                column: "QuestId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_AspNetUsers_CreatedBy",
                table: "Quests",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commissions_Quests_QuestId",
                table: "Commissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Quests_AspNetUsers_CreatedBy",
                table: "Quests");

            migrationBuilder.DropIndex(
                name: "IX_Quests_CustomerId",
                table: "Quests");

            migrationBuilder.DropIndex(
                name: "IX_Commissions_QuestId",
                table: "Commissions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "QuestId",
                table: "Commissions");
        }
    }
}