using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructura.Data.Migrations
{
    public partial class MigracionInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbCompania",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompania = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Dirreccion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Telefono2 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCompania", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbEmpleado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Apellidos = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nombres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompaniaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbEmpleado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbEmpleado_TbCompania_CompaniaId",
                        column: x => x.CompaniaId,
                        principalTable: "TbCompania",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbEmpleado_CompaniaId",
                table: "TbEmpleado",
                column: "CompaniaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbEmpleado");

            migrationBuilder.DropTable(
                name: "TbCompania");
        }
    }
}
