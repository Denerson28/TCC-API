using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class IncreaseImageColumnSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Publishes",
                type: "nvarchar(max)", // Ou nvarchar(2000) para um tamanho específico
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)"); // Tamanho anterior
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Publishes",
                type: "nvarchar(255)", // Tamanho anterior
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)"); // Ou nvarchar(2000) para um tamanho específico
        }
    }
}
