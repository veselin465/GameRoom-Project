using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameRoom.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false, maxLength: 30),
                    RegisteredOn = table.Column<DateTime>(nullable: false),
                    RemovedOn = table.Column<DateTime>(nullable: true),
                    RequiredAge = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false, maxLength: 240)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: false, maxLength: 30),
                    Password = table.Column<string>(nullable: false, maxLength: 40),
                    FirstName = table.Column<string>(nullable: false, maxLength: 30),
                    LastName = table.Column<string>(nullable: false, maxLength: 30),
                    Age = table.Column<int>(nullable: false),
                    RegisteredOn = table.Column<DateTime>(nullable: false),
                    BeingBannedOn = table.Column<DateTime>(nullable: true),
                    ReasonForBeingBanned = table.Column<string>(nullable: true),
                    PlayerOptedOutOn = table.Column<DateTime>(nullable: true),
                    AdditionalInformation = table.Column<string>(nullable: true, maxLength: 240)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scoreboards",
                columns: table => new
                {
                    GameId = table.Column<int>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false),
                    Score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scoreboards", x => new { x.PlayerId, x.GameId });
                    table.ForeignKey(
                        name: "FK_Scoreboards_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Scoreboards_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scoreboards_GameId",
                table: "Scoreboards",
                column: "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Scoreboards");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Players");
        }
    
    }
}
