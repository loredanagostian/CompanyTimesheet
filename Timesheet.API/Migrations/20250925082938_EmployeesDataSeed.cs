using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Timesheet.API.Migrations
{
    /// <inheritdoc />
    public partial class EmployeesDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "CNP", "ContractType", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "1234567890123", 0, "Ana", "Blandiana" },
                    { 2, "9876543210987", 1, "Ion", "Gladiatorul" },
                    { 3, "4567891234567", 0, "Maria", "Ioana" },
                    { 4, "7891234567891", 2, "Catalin", "Botezatul" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 4);
        }
    }
}
