using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class UpdateDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ActivityLogs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Tasks");

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
        }
    }
}
