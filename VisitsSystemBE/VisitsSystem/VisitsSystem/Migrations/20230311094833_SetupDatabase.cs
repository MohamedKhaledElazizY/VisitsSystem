using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitsSystem.Migrations
{
    /// <inheritdoc />
    public partial class SetupDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OFFICES",
                columns: table => new
                {
                    OfficeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OFFICES", x => x.OfficeId);
                });

            migrationBuilder.CreateTable(
                name: "VISITORS",
                columns: table => new
                {
                    VisitorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    VisitorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VISITORS", x => x.VisitorId);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Degree = table.Column<bool>(type: "bit", nullable: false),
                    OfficeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_USERS_OFFICES_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "OFFICES",
                        principalColumn: "OfficeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VISITS",
                columns: table => new
                {
                    VisitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeavingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    VisitorId = table.Column<int>(type: "int", nullable: false),
                    OfficeId = table.Column<int>(type: "int", nullable: false),
                    OfficerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VISITS", x => x.VisitId);
                    table.ForeignKey(
                        name: "FK_VISITS_OFFICES_OfficerId",
                        column: x => x.OfficerId,
                        principalTable: "OFFICES",
                        principalColumn: "OfficeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VISITS_VISITORS_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "VISITORS",
                        principalColumn: "VisitorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "POSTPONED_VISITS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostponedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VisitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POSTPONED_VISITS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POSTPONED_VISITS_VISITS_VisitId",
                        column: x => x.VisitId,
                        principalTable: "VISITS",
                        principalColumn: "VisitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_POSTPONED_VISITS_VisitId",
                table: "POSTPONED_VISITS",
                column: "VisitId");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_OfficeId",
                table: "USERS",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_VISITS_OfficerId",
                table: "VISITS",
                column: "OfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_VISITS_VisitorId",
                table: "VISITS",
                column: "VisitorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "POSTPONED_VISITS");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.DropTable(
                name: "VISITS");

            migrationBuilder.DropTable(
                name: "OFFICES");

            migrationBuilder.DropTable(
                name: "VISITORS");
        }
    }
}
