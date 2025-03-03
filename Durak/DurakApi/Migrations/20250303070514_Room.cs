using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DurakApi.Migrations
{
    /// <inheritdoc />
    public partial class Room : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Rooms_RoomId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Profiles_CreatorId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_CreatorId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_RoomId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "CreatorId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Rooms",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_CreatorId",
                table: "Rooms",
                column: "CreatorId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Profiles_CreatorId",
                table: "Rooms",
                column: "CreatorId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
