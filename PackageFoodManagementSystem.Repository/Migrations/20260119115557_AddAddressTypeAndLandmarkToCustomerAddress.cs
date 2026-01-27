using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageFoodManagementSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressTypeAndLandmarkToCustomerAddress : Migration
    {
        /// <inheritdoc />
        //protected override void Up(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.DropForeignKey(
        //        name: "FK_CustomerAddresses_Customers_CustomerId",
        //        table: "CustomerAddresses");

        //    migrationBuilder.DropPrimaryKey(
        //        name: "PK_Customers",
        //        table: "Customers");

        //    migrationBuilder.RenameTable(
        //        name: "Customers",
        //        newName: "Customer");

        //    migrationBuilder.RenameColumn(
        //        name: "OrderID",
        //        table: "Orders",
        //        newName: "OrderId");

        //    migrationBuilder.AddColumn<string>(
        //        name: "AddressType",
        //        table: "CustomerAddresses",
        //        type: "nvarchar(max)",
        //        nullable: false,
        //        defaultValue: "");

        //    migrationBuilder.AddColumn<string>(
        //        name: "Landmark",
        //        table: "CustomerAddresses",
        //        type: "nvarchar(max)",
        //        nullable: false,
        //        defaultValue: "");

        //    migrationBuilder.AddColumn<int>(
        //        name: "UserId",
        //        table: "Customer",
        //        type: "int",
        //        nullable: true);

        //    migrationBuilder.AddPrimaryKey(
        //        name: "PK_Customer",
        //        table: "Customer",
        //        column: "CustomerId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Customer_UserId",
        //        table: "Customer",
        //        column: "UserId");

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_Customer_UserAuthentications_UserId",
        //        table: "Customer",
        //        column: "UserId",
        //        principalTable: "UserAuthentications",
        //        principalColumn: "Id");

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_CustomerAddresses_Customer_CustomerId",
        //        table: "CustomerAddresses",
        //        column: "CustomerId",
        //        principalTable: "Customer",
        //        principalColumn: "CustomerId",
        //        onDelete: ReferentialAction.Cascade);
        //}

        ///// <inheritdoc />
        //protected override void Down(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.DropForeignKey(
        //        name: "FK_Customer_UserAuthentications_UserId",
        //        table: "Customer");

        //    migrationBuilder.DropForeignKey(
        //        name: "FK_CustomerAddresses_Customer_CustomerId",
        //        table: "CustomerAddresses");

        //    migrationBuilder.DropPrimaryKey(
        //        name: "PK_Customer",
        //        table: "Customer");

        //    migrationBuilder.DropIndex(
        //        name: "IX_Customer_UserId",
        //        table: "Customer");

        //    migrationBuilder.DropColumn(
        //        name: "AddressType",
        //        table: "CustomerAddresses");

        //    migrationBuilder.DropColumn(
        //        name: "Landmark",
        //        table: "CustomerAddresses");

        //    migrationBuilder.DropColumn(
        //        name: "UserId",
        //        table: "Customer");

        //    migrationBuilder.RenameTable(
        //        name: "Customer",
        //        newName: "Customers");

        //    migrationBuilder.RenameColumn(
        //        name: "OrderId",
        //        table: "Orders",
        //        newName: "OrderID");

        //    migrationBuilder.AddPrimaryKey(
        //        name: "PK_Customers",
        //        table: "Customers",
        //        column: "CustomerId");

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_CustomerAddresses_Customers_CustomerId",
        //        table: "CustomerAddresses",
        //        column: "CustomerId",
        //        principalTable: "Customers",
        //        principalColumn: "CustomerId",
        //        onDelete: ReferentialAction.Cascade);
        //}

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adds the AddressType column to match your radio buttons
            migrationBuilder.AddColumn<string>(
                name: "AddressType",
                table: "CustomerAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // Adds the Landmark column to match your form input
            migrationBuilder.AddColumn<string>(
                name: "Landmark",
                table: "CustomerAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Removes the columns if you need to roll back
            migrationBuilder.DropColumn(
                name: "AddressType",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "Landmark",
                table: "CustomerAddresses");
        }
    }
}
