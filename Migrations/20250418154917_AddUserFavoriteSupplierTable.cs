using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventMangerServerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserFavoriteSupplierTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavoriteSupplierIds",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserFavoriteSuppliers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteSuppliers", x => new { x.UserId, x.SupplierId });
                    table.ForeignKey(
                        name: "FK_UserFavoriteSuppliers_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavoriteSuppliers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteSuppliers_SupplierId",
                table: "UserFavoriteSuppliers",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavoriteSuppliers");

            migrationBuilder.AddColumn<string>(
                name: "FavoriteSupplierIds",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
