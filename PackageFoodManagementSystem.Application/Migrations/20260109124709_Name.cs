using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageFoodManagementSystem.Application.Migrations
{
    /// <inheritdoc />
    public partial class Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAuthentication",
                table: "UserAuthentication");

            migrationBuilder.RenameTable(
                name: "UserAuthentication",
                newName: "UserAuthentications");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "UserAuthentications",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "UserAuthentications",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "UserAuthentications",
                newName: "MobileNumber");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserAuthentications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAuthentications",
                table: "UserAuthentications",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAuthentications",
                table: "UserAuthentications");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserAuthentications");

            migrationBuilder.RenameTable(
                name: "UserAuthentications",
                newName: "UserAuthentication");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "UserAuthentication",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "UserAuthentication",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "MobileNumber",
                table: "UserAuthentication",
                newName: "PasswordHash");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAuthentication",
                table: "UserAuthentication",
                column: "Id");
        }
    }
}
