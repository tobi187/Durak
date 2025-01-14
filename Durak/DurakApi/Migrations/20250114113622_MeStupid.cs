using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DurakApi.Migrations
{
    /// <inheritdoc />
    public partial class MeStupid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Rooms_RoomId1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoomId1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoomId1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "splate",
                table: "Rooms");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoomId",
                table: "Users",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Rooms_RoomId",
                table: "Users",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Rooms_RoomId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoomId",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId1",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "splate",
                table: "Rooms",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoomId1",
                table: "Users",
                column: "RoomId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Rooms_RoomId1",
                table: "Users",
                column: "RoomId1",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
