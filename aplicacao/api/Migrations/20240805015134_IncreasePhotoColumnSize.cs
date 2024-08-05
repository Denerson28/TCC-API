using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class IncreasePhotoColumnSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "Users",
                type: "nvarchar(max)", // Ou nvarchar(2000) para um tamanho específico
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)"); // Tamanho anterior
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "Users",
                type: "nvarchar(255)", // Tamanho anterior
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)"); // Ou nvarchar(2000) para um tamanho específico
        }
    }
}
