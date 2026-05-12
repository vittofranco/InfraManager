using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraManager.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarEquipamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataCadastro",
                table: "Equipamentos",
                newName: "DataEntrada");

            migrationBuilder.AddColumn<string>(
                name: "Gravidade",
                table: "Equipamentos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Responsavel",
                table: "Equipamentos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gravidade",
                table: "Equipamentos");

            migrationBuilder.DropColumn(
                name: "Responsavel",
                table: "Equipamentos");

            migrationBuilder.RenameColumn(
                name: "DataEntrada",
                table: "Equipamentos",
                newName: "DataCadastro");
        }
    }
}
