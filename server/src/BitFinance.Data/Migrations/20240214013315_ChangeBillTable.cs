using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BitFinance.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBillTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Bills");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Bills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Bills",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
