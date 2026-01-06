using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brackets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brackets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    BracketId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tournaments_Brackets_BracketId",
                        column: x => x.BracketId,
                        principalTable: "Brackets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    TournamentId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Round = table.Column<int>(type: "INTEGER", nullable: false),
                    Player1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Player2Id = table.Column<int>(type: "INTEGER", nullable: true),
                    WinnerId = table.Column<int>(type: "INTEGER", nullable: true),
                    BracketId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Brackets_BracketId",
                        column: x => x.BracketId,
                        principalTable: "Brackets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Matches_Users_Player1Id",
                        column: x => x.Player1Id,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Matches_Users_Player2Id",
                        column: x => x.Player2Id,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Matches_Users_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_BracketId",
                table: "Matches",
                column: "BracketId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player1Id",
                table: "Matches",
                column: "Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player2Id",
                table: "Matches",
                column: "Player2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_WinnerId",
                table: "Matches",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_BracketId",
                table: "Tournaments",
                column: "BracketId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TournamentId",
                table: "Users",
                column: "TournamentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "Brackets");
        }
    }
}
