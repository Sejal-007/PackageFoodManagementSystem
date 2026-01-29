using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageFoodManagementSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class CleanLinkAndDecimalFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Add the UserId column to Customers
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Customer",
                type: "int",
                nullable: true);

            // 2. Create the Index for performance
            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customer",
                column: "UserId");

            // 3. Add the Foreign Key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_Customers_UserAuthentications_UserId",
                table: "Customer",
                column: "UserId",
                principalTable: "UserAuthentications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Customers_UserAuthentications_UserId", table: "Customer");
            migrationBuilder.DropIndex(name: "IX_Customers_UserId", table: "Customer");
            migrationBuilder.DropColumn(name: "UserId", table: "Customer");
        }
    }
}
