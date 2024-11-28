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

    public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderDetail>()
            .HasOne(d => d.Order)  // Configura la relación inversa en OrderDetail
            .WithMany(o => o.Details)
            .HasForeignKey(d => d.OrderId);  // Asegura que la clave foránea esté definida correctamente

        modelBuilder.Entity<Order>()
            .HasMany(o => o.Details)
            .WithOne(d => d.Order)
            .HasForeignKey(d => d.OrderId);

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
    }

}