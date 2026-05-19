using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClyvoVet.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    IdPet = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Especie = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Raca = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Peso = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    StatusSaude = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.IdPet);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pets");
        }
    }
}
