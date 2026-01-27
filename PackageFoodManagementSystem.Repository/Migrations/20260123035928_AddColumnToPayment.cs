using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageFoodManagementSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnToPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderID",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
        name: "OrderID",
        table: "Payment");
        }
    }
}
