using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitsSystem.Migrations
{
    /// <inheritdoc />
    public partial class SetupDatabase11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VISITS_OFFICES_OfficerId",
                table: "VISITS");

            migrationBuilder.DropIndex(
                name: "IX_VISITS_OfficerId",
                table: "VISITS");

            migrationBuilder.DropColumn(
                name: "OfficerId",
                table: "VISITS");

            migrationBuilder.CreateIndex(
                name: "IX_VISITS_OfficeId",
                table: "VISITS",
                column: "OfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_VISITS_OFFICES_OfficeId",
                table: "VISITS",
                column: "OfficeId",
                principalTable: "OFFICES",
                principalColumn: "OfficeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VISITS_OFFICES_OfficeId",
                table: "VISITS");

            migrationBuilder.DropIndex(
                name: "IX_VISITS_OfficeId",
                table: "VISITS");

            migrationBuilder.AddColumn<int>(
                name: "OfficerId",
                table: "VISITS",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VISITS_OfficerId",
                table: "VISITS",
                column: "OfficerId");

            migrationBuilder.AddForeignKey(
                name: "FK_VISITS_OFFICES_OfficerId",
                table: "VISITS",
                column: "OfficerId",
                principalTable: "OFFICES",
                principalColumn: "OfficeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
