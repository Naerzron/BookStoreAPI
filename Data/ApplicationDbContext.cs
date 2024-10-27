using BookStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Data;

public class ApplicationDbContext : DbContext
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
}