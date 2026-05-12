using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraManager.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCondenacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Condenado",
                table: "Equipamentos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCondenacao",
                table: "Equipamentos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarcaEquipamento",
                table: "Equipamentos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MotivoCondenacao",
                table: "Equipamentos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ReposicaoPatrimonio",
                table: "Equipamentos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Condenado",
                table: "Equipamentos");

            migrationBuilder.DropColumn(
                name: "DataCondenacao",
                table: "Equipamentos");

            migrationBuilder.DropColumn(
                name: "MarcaEquipamento",
                table: "Equipamentos");

            migrationBuilder.DropColumn(
                name: "MotivoCondenacao",
                table: "Equipamentos");

            migrationBuilder.DropColumn(
                name: "ReposicaoPatrimonio",
                table: "Equipamentos");
        }
    }
}
