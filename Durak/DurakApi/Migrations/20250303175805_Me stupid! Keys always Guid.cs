using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DurakApi.Migrations
{
    /// <inheritdoc />
    public partial class MestupidKeysalwaysGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Rooms_RoomId1",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_RoomId1",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "RoomId1",
                table: "Profiles");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Rooms",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_RoomId",
                table: "Profiles",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Rooms_RoomId",
                table: "Profiles",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Rooms_RoomId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_RoomId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Rooms");

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId1",
                table: "Profiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_RoomId1",
                table: "Profiles",
                column: "RoomId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Rooms_RoomId1",
                table: "Profiles",
                column: "RoomId1",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
