//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace PackageFoodManagementSystem.Repository.Migrations
//{
//    /// <inheritdoc />
//    public partial class RenameIdToProductId : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "Wallets",
//                columns: table => new
//                {
//                    WalletId = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    UserId = table.Column<int>(type: "int", nullable: false),
//                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Wallets", x => x.WalletId);
//                    table.ForeignKey(
//                        name: "FK_Wallets_UserAuthentications_UserId",
//                        column: x => x.UserId,
//                        principalTable: "UserAuthentications",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateIndex(
//                name: "IX_Wallets_UserId",
//                table: "Wallets",
//                column: "UserId");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "Wallets");
//        }
//    }
//}



using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageFoodManagementSystem.Repository.Migrations
{
    public partial class RenameIdToProductId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Rename Id to ProductId in the Products table
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "ProductId");

            // 2. Create the Wallets table (since EF detected this new model)
            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    WalletId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse the changes if you roll back
            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Products",
                newName: "Id");
        }
    }
}