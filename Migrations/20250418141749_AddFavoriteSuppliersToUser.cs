using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventMangerServerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddFavoriteSuppliersToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FavoriteSupplierIds",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavoriteSupplierIds",
                table: "Users");
        }
    }
}
