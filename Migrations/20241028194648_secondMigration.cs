using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStore.API.Migrations
{
    /// <inheritdoc />
    public partial class secondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "BirthDay", "Name" },
                values: new object[] { 1, new DateOnly(1975, 12, 19), "Brandon Sanderson" });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Explora futuros hipotéticos, avances científicos y tecnológicos, y mundos alternativos, abordando temas de impacto en la humanidad y su relación con el universo.", "Ciencia Ficción" },
                    { 2, "Historias en mundos mágicos donde héroes enfrentan misiones de gran envergadura contra fuerzas oscuras.", "Fantasía Épica" },
                    { 3, "Representa futuros sombríos y autoritarios en los que la sociedad ha colapsado o es gobernada de manera opresiva.", "Distopía" },
                    { 4, "Narrativas ambientadas en el espacio, con viajes intergalácticos, planetas desconocidos y combates entre especies alienígenas.", "Aventura Espacial" },
                    { 5, "Retrata un futuro urbano decadente donde la alta tecnología se mezcla con el bajo nivel de vida, explorando temas de identidad y control corporativo.", "Ciberpunk" },
                    { 6, "Fusiona la estética victoriana y tecnología a vapor en un mundo alternativo con invenciones retrofuturistas y aventuras rebeldes.", "Steampunk" },
                    { 7, "Explora terrores de origen extraterrestre y fuerzas cósmicas que superan la comprensión humana.", "Horror Cósmico" },
                    { 8, "Relatos que reinterpretan mitos antiguos y leyendas en un contexto contemporáneo o fantástico.", "Mitología Moderna" },
                    { 9, "Historias de supervivencia en mundos desmoronados, llenos de zombis u otras amenazas apocalípticas.", "Zombis y Apocalipsis" },
                    { 10, "Historias donde los personajes viven en mundos de rol o videojuegos, experimentando niveles, habilidades y mecánicas típicas de juegos de rol.", "LitRPG" },
                    { 11, "Historias de personajes con habilidades extraordinarias que protegen al mundo de amenazas y villanos.", "Superhéroes" },
                    { 12, "Narrativas oscuras y llenas de misterio, a menudo centradas en crímenes y antihéroes.", "Cómic Noir" },
                    { 13, "Mundos donde la magia y lo sobrenatural conviven con la vida cotidiana en ciudades modernas.", "Fantasía Urbana" },
                    { 14, "Explora futuros sombríos y tecnológicos con sociedades autoritarias y temas de rebelión.", "Ciencia Ficción Distópica" },
                    { 15, "Historias de acción y aventuras orientadas a un público juvenil, a menudo con personajes en constante superación.", "Shonen" },
                    { 16, "Narrativas enfocadas en romance y relaciones emocionales, orientadas a un público femenino joven.", "Shojo" },
                    { 17, "Historias maduras y complejas, a menudo de acción, horror o temas psicológicos, dirigidas a un público adulto.", "Seinen" },
                    { 18, "Relatos donde los protagonistas son transportados a mundos paralelos o fantásticos, enfrentando desafíos y nuevas vidas.", "Isekai" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "GenreId", "Isbn", "OrderId", "Price", "PublishedDate", "Stock", "Synopsis", "Title" },
                values: new object[] { 1, 1, 2, "0765326353", null, 33.33m, new DateOnly(2010, 8, 31), 10, "Las tremendisimas desventuras y depresion de Kaladin", "El camino de los reyes" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
