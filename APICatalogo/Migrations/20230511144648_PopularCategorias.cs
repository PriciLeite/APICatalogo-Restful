using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopularCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into Categorias(Nome, ImagemUrl) values('Bebidas', 'bebidas.png')");
            migrationBuilder.Sql("insert into Categorias(Nome, ImagemUrl) values('Lanches', 'lanches.png')");
            migrationBuilder.Sql("insert into Categorias(Nome, ImagemUrl) values('Sobremesas', 'sobremesas.png')");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Categorias");
        }
    }
}
