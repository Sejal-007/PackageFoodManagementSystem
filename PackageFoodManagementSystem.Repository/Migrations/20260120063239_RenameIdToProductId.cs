using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageFoodManagementSystem.Repository.Migrations
{
    public partial class RenameIdToProductId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. PRODUCTS TABLE: Rename Id to ProductId
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "ProductId");

            // 2. BATCH TABLE: Rename table and update constraints/columns
            migrationBuilder.DropPrimaryKey(
                name: "PK_Batches",
                table: "Batches");

            migrationBuilder.RenameTable(
                name: "Batches",
                newName: "Batch");

            migrationBuilder.AlterColumn<string>(
                name: "BatchNumber",
                table: "Batch",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(name: "Description", table: "Batch", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<int>(name: "InitialQuantity", table: "Batch", type: "int", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<int>(name: "ProductId", table: "Batch", type: "int", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<DateTime>(name: "ProductionDate", table: "Batch", type: "datetime2", nullable: false, defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AddColumn<int>(name: "RemainingQuantity", table: "Batch", type: "int", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<int>(name: "Status", table: "Batch", type: "int", nullable: false, defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Batch",
                table: "Batch",
                column: "Id");

            // 3. WALLETS TABLE: Create new table
            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    WalletId = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.WalletId);
                    table.ForeignKey(
                        name: "FK_Wallets_UserAuthentications_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAuthentications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // 4. ORDERS TABLE: Add PaymentID
            migrationBuilder.AddColumn<int>(
                name: "PaymentID",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // 5. INDEXES & FOREIGN KEYS
            migrationBuilder.CreateIndex(name: "IX_Wallets_UserId", table: "Wallets", column: "UserId");
            migrationBuilder.CreateIndex(name: "IX_Orders_PaymentID", table: "Orders", column: "PaymentID");
            migrationBuilder.CreateIndex(name: "IX_Batch_ProductId", table: "Batch", column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batch_Products_ProductId",
                table: "Batch",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Payments_PaymentID",
                table: "Orders",
                column: "PaymentID",
                principalTable: "Payments",
                principalColumn: "PaymentID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Wallets");
            migrationBuilder.DropForeignKey(name: "FK_Batch_Products_ProductId", table: "Batch");
            migrationBuilder.DropForeignKey(name: "FK_Orders_Payments_PaymentID", table: "Orders");

            migrationBuilder.DropPrimaryKey(name: "PK_Batch", table: "Batch");
            migrationBuilder.RenameTable(name: "Batch", newName: "Batches");
            migrationBuilder.RenameColumn(name: "ProductId", table: "Products", newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Batches",
                table: "Batches",
                column: "Id");
        }
    }
}