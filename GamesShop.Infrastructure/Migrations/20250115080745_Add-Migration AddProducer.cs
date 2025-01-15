using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamesShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMigrationAddProducer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Producer",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Producer",
                table: "Products");
        }
    }
}
