using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Timesheet.API.Migrations
{
    /// <inheritdoc />
    public partial class newcolumnsforuseraccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasDefaultPassword",
                table: "UserAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAlias",
                table: "UserAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasDefaultPassword",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "IsAlias",
                table: "UserAccounts");
        }
    }
}
