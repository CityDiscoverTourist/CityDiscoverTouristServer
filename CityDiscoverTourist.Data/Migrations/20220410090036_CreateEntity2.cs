using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class CreateEntity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBack_Quests_QuestId",
                table: "FeedBack");

            migrationBuilder.DropForeignKey(
                name: "FK_Suggestion_Tasks_TaskId",
                table: "Suggestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Suggestion",
                table: "Suggestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedBack",
                table: "FeedBack");

            migrationBuilder.RenameTable(
                name: "Suggestion",
                newName: "Suggestions");

            migrationBuilder.RenameTable(
                name: "FeedBack",
                newName: "FeedBacks");

            migrationBuilder.RenameIndex(
                name: "IX_Suggestion_TaskId",
                table: "Suggestions",
                newName: "IX_Suggestions_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_FeedBack_QuestId",
                table: "FeedBacks",
                newName: "IX_FeedBacks_QuestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Suggestions",
                table: "Suggestions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedBacks",
                table: "FeedBacks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLogs_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RightAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_UserId1",
                table: "ActivityLogs",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_TaskId",
                table: "Answers",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBacks_Quests_QuestId",
                table: "FeedBacks",
                column: "QuestId",
                principalTable: "Quests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_Tasks_TaskId",
                table: "Suggestions",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBacks_Quests_QuestId",
                table: "FeedBacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_Tasks_TaskId",
                table: "Suggestions");

            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Suggestions",
                table: "Suggestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedBacks",
                table: "FeedBacks");

            migrationBuilder.RenameTable(
                name: "Suggestions",
                newName: "Suggestion");

            migrationBuilder.RenameTable(
                name: "FeedBacks",
                newName: "FeedBack");

            migrationBuilder.RenameIndex(
                name: "IX_Suggestions_TaskId",
                table: "Suggestion",
                newName: "IX_Suggestion_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_FeedBacks_QuestId",
                table: "FeedBack",
                newName: "IX_FeedBack_QuestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Suggestion",
                table: "Suggestion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedBack",
                table: "FeedBack",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBack_Quests_QuestId",
                table: "FeedBack",
                column: "QuestId",
                principalTable: "Quests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestion_Tasks_TaskId",
                table: "Suggestion",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
