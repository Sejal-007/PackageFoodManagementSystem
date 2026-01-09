using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageFoodManagementSystem.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Products");

        }
    }
}
