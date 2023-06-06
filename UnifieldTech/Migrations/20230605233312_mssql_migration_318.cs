using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnifieldTech.Migrations
{
    /// <inheritdoc />
    public partial class mssql_migration_318 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    ClienteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    E_Mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNacs = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.ClienteID);
                });

            migrationBuilder.CreateTable(
                name: "Celular",
                columns: table => new
                {
                    CelularID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CelularN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClienteID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Celular", x => x.CelularID);
                    table.ForeignKey(
                        name: "FK_Celular_Cliente_ClienteID",
                        column: x => x.ClienteID,
                        principalTable: "Cliente",
                        principalColumn: "ClienteID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fazenda",
                columns: table => new
                {
                    FazendaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeFazenda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hectar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cultivar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rua = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Num = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoPlantio = table.Column<bool>(type: "bit", nullable: false),
                    AreaMecanizada = table.Column<bool>(type: "bit", nullable: false),
                    ClienteID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fazenda", x => x.FazendaID);
                    table.ForeignKey(
                        name: "FK_Fazenda_Cliente_ClienteID",
                        column: x => x.ClienteID,
                        principalTable: "Cliente",
                        principalColumn: "ClienteID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cliente",
                columns: new[] { "ClienteID", "CPF", "Codigo", "DataNacs", "E_Mail", "NomeCliente", "Password" },
                values: new object[] { 1, "132.318.266.93", "34as5", new DateTime(1997, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "robert@gmail.com", "Robert", "123" });

            migrationBuilder.InsertData(
                table: "Celular",
                columns: new[] { "CelularID", "CelularN", "ClienteID" },
                values: new object[] { 1, "35991529241", 1 });

            migrationBuilder.InsertData(
                table: "Fazenda",
                columns: new[] { "FazendaID", "AreaMecanizada", "Cidade", "ClienteID", "Cultivar", "Estado", "Hectar", "Latitude", "Longitude", "NomeFazenda", "Num", "Rua", "TipoPlantio" },
                values: new object[] { 1, true, "Alfenas", 1, "Café", "Minas Gerais", "18 He", "48", "45", "Caipira", "1458", "Primeira segunda", true });

            migrationBuilder.CreateIndex(
                name: "IX_Celular_ClienteID",
                table: "Celular",
                column: "ClienteID");

            migrationBuilder.CreateIndex(
                name: "IX_Fazenda_ClienteID",
                table: "Fazenda",
                column: "ClienteID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Celular");

            migrationBuilder.DropTable(
                name: "Fazenda");

            migrationBuilder.DropTable(
                name: "Cliente");
        }
    }
}
