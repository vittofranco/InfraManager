using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraManager.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarConclusaoEquipamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Concluido",
                table: "Equipamentos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataConclusao",
                table: "Equipamentos",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Concluido",
                table: "Equipamentos");

            migrationBuilder.DropColumn(
                name: "DataConclusao",
                table: "Equipamentos");
        }
    }
}
