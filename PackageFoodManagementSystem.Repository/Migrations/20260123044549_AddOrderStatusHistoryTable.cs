using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageFoodManagementSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderStatusHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderStatusHistories",
                columns: table => new
                {
                    StatusHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedBy = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatusHistories", x => x.StatusHistoryID);
                    table.ForeignKey(
                        name: "FK_OrderStatusHistories_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusHistories_OrderID",
                table: "OrderStatusHistories",
                column: "OrderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderStatusHistories");

           
        }
    }
}
