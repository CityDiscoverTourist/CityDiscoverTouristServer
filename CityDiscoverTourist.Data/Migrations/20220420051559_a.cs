using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAnswers_CustomerTasks_CustomerTaskId",
                table: "CustomerAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTasks_AspNetUsers_ApplicationUserId",
                table: "CustomerTasks");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "CustomerTasks",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerTasks_ApplicationUserId",
                table: "CustomerTasks",
                newName: "IX_CustomerTasks_CustomerId");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerTaskId",
                table: "CustomerAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAnswers_CustomerTasks_CustomerTaskId",
                table: "CustomerAnswers",
                column: "CustomerTaskId",
                principalTable: "CustomerTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTasks_AspNetUsers_CustomerId",
                table: "CustomerTasks",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAnswers_CustomerTasks_CustomerTaskId",
                table: "CustomerAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTasks_AspNetUsers_CustomerId",
                table: "CustomerTasks");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "CustomerTasks",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerTasks_CustomerId",
                table: "CustomerTasks",
                newName: "IX_CustomerTasks_ApplicationUserId");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerTaskId",
                table: "CustomerAnswers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAnswers_CustomerTasks_CustomerTaskId",
                table: "CustomerAnswers",
                column: "CustomerTaskId",
                principalTable: "CustomerTasks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTasks_AspNetUsers_ApplicationUserId",
                table: "CustomerTasks",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
