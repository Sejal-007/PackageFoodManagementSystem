using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageFoodManagementSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class SyncMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Batch",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Batch_ProductId",
                table: "Batch",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batch_Products_ProductId",
                table: "Batch",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batch_Products_ProductId",
                table: "Batch");

            migrationBuilder.DropIndex(
                name: "IX_Batch_ProductId",
                table: "Batch");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Batch",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
