using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageFoodManagementSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerAndCustomerAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Addresses_Customers_CustomerId",
            //    table: "Addresses");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_UserAuthentication",
            //    table: "UserAuthentication");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Addresses",
            //    table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CustomerAddresses",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "UserAuthentication",
                newName: "UserAuthentications");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "CustomerAddresses");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_CustomerId",
                table: "CustomerAddresses",
                newName: "IX_CustomerAddresses_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAuthentications",
                table: "UserAuthentications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerAddresses",
                table: "CustomerAddresses",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAddresses_Customers_CustomerId",
                table: "CustomerAddresses",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_CustomerAddresses_Customers_CustomerId",
            //    table: "CustomerAddresses");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_UserAuthentications",
            //    table: "UserAuthentications");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_CustomerAddresses",
            //    table: "CustomerAddresses");

            migrationBuilder.RenameTable(
                name: "UserAuthentications",
                newName: "UserAuthentication");

            migrationBuilder.RenameTable(
                name: "CustomerAddresses",
                newName: "Addresses");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerAddresses_CustomerId",
                table: "Addresses",
                newName: "IX_Addresses_CustomerId");

            migrationBuilder.AddColumn<string>(
                name: "CustomerAddresses",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAuthentication",
                table: "UserAuthentication",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Customers_CustomerId",
                table: "Addresses",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
