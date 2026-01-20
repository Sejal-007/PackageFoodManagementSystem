using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageFoodManagementSystem.Repository.Migrations
{
    public partial class FixDatabaseSync : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Try to add ImageData ONLY if it doesn't exist. 
            // If you get an error saying 'ImageData' already exists, comment this out too!
            migrationBuilder.AddColumn<string>(
                name: "ImageData",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            // 2. EVERYTHING ELSE ALREADY EXISTS - LEAVE COMMENTED OUT
            /*
            migrationBuilder.AddColumn<string>(name: "Phone", table: "Customer"...);
            migrationBuilder.AddColumn<string>(name: "Status", table: "Customer"...);
            migrationBuilder.CreateTable(name: "CustomerAddresses"...);
            */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "CustomerAddresses");
            migrationBuilder.DropColumn(name: "ImageData", table: "Products");
            migrationBuilder.DropColumn(name: "Status", table: "Customer");
            // migrationBuilder.DropColumn(name: "Phone", table: "Customer");
        }
    }
}