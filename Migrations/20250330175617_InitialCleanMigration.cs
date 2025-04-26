using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventMangerServerApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCleanMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_ManagerId",
                table: "Events");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_ManagerId",
                table: "Events",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_ManagerId",
                table: "Events");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_ManagerId",
                table: "Events",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
