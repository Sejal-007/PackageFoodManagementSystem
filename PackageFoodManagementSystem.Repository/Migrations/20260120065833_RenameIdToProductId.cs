using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageFoodManagementSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RenameIdToProductId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Batches",
                table: "Batches");

            migrationBuilder.RenameTable(
                name: "Batches",
                newName: "Batch");

            migrationBuilder.AddColumn<int>(
                name: "PaymentID",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "BatchNumber",
                table: "Batch",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Batch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InitialQuantity",
                table: "Batch",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Batch",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProductionDate",
                table: "Batch",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RemainingQuantity",
                table: "Batch",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Batch",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Batch",
                table: "Batch",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentID",
                table: "Orders",
                column: "PaymentID");

            migrationBuilder.CreateIndex(
                name: "IX_Batch_ProductId",
                table: "Batch",
                column: "ProductId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batch_Products_ProductId",
                table: "Batch");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Payments_PaymentID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentID",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Batch",
                table: "Batch");

            migrationBuilder.DropIndex(
                name: "IX_Batch_ProductId",
                table: "Batch");

            migrationBuilder.DropColumn(
                name: "PaymentID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Batch");

            migrationBuilder.DropColumn(
                name: "InitialQuantity",
                table: "Batch");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Batch");

            migrationBuilder.DropColumn(
                name: "ProductionDate",
                table: "Batch");

            migrationBuilder.DropColumn(
                name: "RemainingQuantity",
                table: "Batch");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Batch");

            migrationBuilder.RenameTable(
                name: "Batch",
                newName: "Batches");

            migrationBuilder.AlterColumn<string>(
                name: "BatchNumber",
                table: "Batches",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Batches",
                table: "Batches",
                column: "Id");
        }
    }
}
