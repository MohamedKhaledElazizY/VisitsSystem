using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitsSystem.Migrations
{
    /// <inheritdoc />
    public partial class SetupDatabase12AllowNullOfficeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VISITS_OFFICES_OfficeId",
                table: "VISITS");

            migrationBuilder.AlterColumn<int>(
                name: "OfficeId",
                table: "VISITS",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_VISITS_OFFICES_OfficeId",
                table: "VISITS",
                column: "OfficeId",
                principalTable: "OFFICES",
                principalColumn: "OfficeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VISITS_OFFICES_OfficeId",
                table: "VISITS");

            migrationBuilder.AlterColumn<int>(
                name: "OfficeId",
                table: "VISITS",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VISITS_OFFICES_OfficeId",
                table: "VISITS",
                column: "OfficeId",
                principalTable: "OFFICES",
                principalColumn: "OfficeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
