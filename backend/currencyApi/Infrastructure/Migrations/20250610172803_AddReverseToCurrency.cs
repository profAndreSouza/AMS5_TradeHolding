using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace currencyApi.Migrations
{
    /// <inheritdoc />
    public partial class AddReverseToCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Reverse",
                table: "Currencies",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reverse",
                table: "Currencies");
        }
    }
}
