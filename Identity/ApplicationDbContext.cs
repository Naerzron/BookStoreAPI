using BookStore.API.Identity;
using BookStore.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Data;

public class ApplicationDbContext : IdentityDbContext<MyUser>
{
    // Constructor de la clase ApplicationDbContext, que hereda de DbContext.
    // Este constructor recibe una instancia de DbContextOptions<ApplicationDbContext>
    // como parámetro, la cual contiene la configuración necesaria para la conexión
    // a la base de datos (cadena de conexión, proveedor de base de datos, etc.).
    // Luego, llama al constructor de la clase base DbContext, pasando estas opciones
    // para configurar el contexto de la base de datos correctamente.
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
            
    }

    public DbSet<Book> Books { get; set; }

    public DbSet<Genre> Genres { get; set; }

    public DbSet<Author> Authors { get; set; }

    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, Name = "Ciencia Ficción", Description = "Explora futuros hipotéticos, avances científicos y tecnológicos, y mundos alternativos, abordando temas de impacto en la humanidad y su relación con el universo." },
            new Genre { Id = 2, Name = "Fantasía Épica", Description = "Historias en mundos mágicos donde héroes enfrentan misiones de gran envergadura contra fuerzas oscuras." },
            new Genre { Id = 3, Name = "Distopía", Description = "Representa futuros sombríos y autoritarios en los que la sociedad ha colapsado o es gobernada de manera opresiva." },
            new Genre { Id = 4, Name = "Aventura Espacial", Description = "Narrativas ambientadas en el espacio, con viajes intergalácticos, planetas desconocidos y combates entre especies alienígenas." },
            new Genre { Id = 5, Name = "Ciberpunk", Description = "Retrata un futuro urbano decadente donde la alta tecnología se mezcla con el bajo nivel de vida, explorando temas de identidad y control corporativo." },
            new Genre { Id = 6, Name = "Steampunk", Description = "Fusiona la estética victoriana y tecnología a vapor en un mundo alternativo con invenciones retrofuturistas y aventuras rebeldes." },
            new Genre { Id = 7, Name = "Horror Cósmico", Description = "Explora terrores de origen extraterrestre y fuerzas cósmicas que superan la comprensión humana." },
            new Genre { Id = 8, Name = "Mitología Moderna", Description = "Relatos que reinterpretan mitos antiguos y leyendas en un contexto contemporáneo o fantástico." },
            new Genre { Id = 9, Name = "Zombis y Apocalipsis", Description = "Historias de supervivencia en mundos desmoronados, llenos de zombis u otras amenazas apocalípticas." },
            new Genre { Id = 10, Name = "LitRPG", Description = "Historias donde los personajes viven en mundos de rol o videojuegos, experimentando niveles, habilidades y mecánicas típicas de juegos de rol." },
            // Géneros de cómics
            new Genre { Id = 11, Name = "Superhéroes", Description = "Historias de personajes con habilidades extraordinarias que protegen al mundo de amenazas y villanos." },
            new Genre { Id = 12, Name = "Cómic Noir", Description = "Narrativas oscuras y llenas de misterio, a menudo centradas en crímenes y antihéroes." },
            new Genre { Id = 13, Name = "Fantasía Urbana", Description = "Mundos donde la magia y lo sobrenatural conviven con la vida cotidiana en ciudades modernas." },
            new Genre { Id = 14, Name = "Ciencia Ficción Distópica", Description = "Explora futuros sombríos y tecnológicos con sociedades autoritarias y temas de rebelión." },
            // Géneros de manga
            new Genre { Id = 15, Name = "Shonen", Description = "Historias de acción y aventuras orientadas a un público juvenil, a menudo con personajes en constante superación." },
            new Genre { Id = 16, Name = "Shojo", Description = "Narrativas enfocadas en romance y relaciones emocionales, orientadas a un público femenino joven." },
            new Genre { Id = 17, Name = "Seinen", Description = "Historias maduras y complejas, a menudo de acción, horror o temas psicológicos, dirigidas a un público adulto." },
            new Genre { Id = 18, Name = "Isekai", Description = "Relatos donde los protagonistas son transportados a mundos paralelos o fantásticos, enfrentando desafíos y nuevas vidas." }
        );

        var author = new Author
        {
            Id = 1,
            Name = "Brandon Sanderson",
            BirthDay = new DateOnly(1975, 12, 19)
        };

        modelBuilder.Entity<Author>().HasData(author);

//         modelBuilder.Entity<Book>().HasData(
//             new Book
//             {
//                 Id = 1,
//                 Title = "El camino de los reyes",
//                 PublishedDate = new DateOnly(2010, 08, 31),
//                 Isbn = "0765326353",
//                 Stock = 10,
//                 Price = 33.33m,
//                 Synopsis = @"El camino de los reyes es el primer volumen de «El Archivo de las Tormentas», el resultado de más de una década de construcción y escritura de universos, convertido en una obra maestra de la fantasía contemporánea en diez volúmenes. Con ella, Brandon Sanderson se postula como el autor del género que más lectores está ganando en todo el mundo.

// Anhelo los días previos a la Última Desolación.

// Los días en que los Heraldos nos abandonaron y los Caballeros Radiantes se giraron en nuestra contra. Un tiempo en que aún había magia en el mundo y honor en el corazón de los hombres.

// El mundo fue nuestro, pero lo perdimos. Probablemente no hay nada más estimulante para las almas de los hombres que la victoria.

// ¿O tal vez fue la victoria una ilusión durante todo ese tiempo? ¿Comprendieron nuestros enemigos que cuanto más duramente luchaban, más resistíamos nosotros? Quizá vieron que el fuego y el martillo tan solo producían mejores espadas. Pero ignoraron el acero durante el tiempo suficiente para oxidarse.

// Hay cuatro personas a las que observamos. La primera es el médico, quien dejó de curar para convertirse en soldado durante la guerra más brutal de nuestro tiempo. La segunda es el asesino, un homicida que llora siempre que mata. La tercera es la mentirosa, una joven que viste un manto de erudita sobre un corazón de ladrona. Por último está el alto príncipe, un guerrero que mira al pasado mientras languidece su sed de guerra.

// El mundo puede cambiar. La potenciación y el uso de las esquirlas pueden aparecer de nuevo, la magia de los días pasados puede volver a ser nuestra. Esas cuatro personas son la clave.

// Una de ellas nos redimirá. Y una de ellas nos destruirá.",
//                 Image = "",
//                 Author = author,
//                 Genre = genres.First(),
            // }
        // );
    }

}