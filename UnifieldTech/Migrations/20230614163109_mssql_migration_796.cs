using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnifieldTech.Migrations
{
    /// <inheritdoc />
    public partial class mssql_migration_796 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "Cliente",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "ClienteID",
                keyValue: 1,
                column: "Codigo",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "Cliente",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "ClienteID",
                keyValue: 1,
                column: "Codigo",
                value: "34as5");
        }
    }
}
