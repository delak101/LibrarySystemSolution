using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystemApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAuthors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Books");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Books",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }
    }
}
