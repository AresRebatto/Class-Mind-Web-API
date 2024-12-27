using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassMindAPI.Migrations
{
    /// <inheritdoc />
    public partial class CorrezioneErrore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Materie",
                columns: table => new
                {
                    MateriaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materie", x => x.MateriaId);
                });

            migrationBuilder.CreateTable(
                name: "Studenti",
                columns: table => new
                {
                    StudenteId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Cognome = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studenti", x => x.StudenteId);
                });

            migrationBuilder.CreateTable(
                name: "Lezioni",
                columns: table => new
                {
                    LezioneId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GiornoSettimana = table.Column<string>(type: "TEXT", nullable: false),
                    MateriaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lezioni", x => x.LezioneId);
                    table.ForeignKey(
                        name: "FK_Lezioni_Materie_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materie",
                        principalColumn: "MateriaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Interrogazioni",
                columns: table => new
                {
                    InterrogazioneId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<string>(type: "TEXT", nullable: false),
                    StudenteId = table.Column<int>(type: "INTEGER", nullable: false),
                    MateriaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interrogazioni", x => x.InterrogazioneId);
                    table.ForeignKey(
                        name: "FK_Interrogazioni_Materie_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materie",
                        principalColumn: "MateriaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Interrogazioni_Studenti_StudenteId",
                        column: x => x.StudenteId,
                        principalTable: "Studenti",
                        principalColumn: "StudenteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interrogazioni_MateriaId",
                table: "Interrogazioni",
                column: "MateriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Interrogazioni_StudenteId",
                table: "Interrogazioni",
                column: "StudenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Lezioni_MateriaId",
                table: "Lezioni",
                column: "MateriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Interrogazioni");

            migrationBuilder.DropTable(
                name: "Lezioni");

            migrationBuilder.DropTable(
                name: "Studenti");

            migrationBuilder.DropTable(
                name: "Materie");
        }
    }
}
