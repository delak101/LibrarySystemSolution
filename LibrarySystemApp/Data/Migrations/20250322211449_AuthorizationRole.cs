using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystemApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AuthorizationRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                type: "nvarchar(10)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                type: "INTEGER",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "nvarchar(10)",
                oldMaxLength: 50);
        }
    }
}
