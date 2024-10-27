using BookStore.API.Data;
using BookStore.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly Book[] _books = new Book[3];
    
    // Inyectar el DbContext a través del constructor
    public BooksController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet()]
    public IEnumerable<Book> GetBooks()
    {
        return _context.Books.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<Book> GetBook(int id)
    {
        try
        {
            var book = _context.Books.Find(id);
            if(book is null) return NotFound("Libro no encontrado"); // NotFound (404)

            return Ok(book); // Ok (200)
        } 
        catch 
        {
            return Problem(); // InternalServerError (500)
        }
    }

    [HttpPost]
    public IActionResult CreateBook(Book book)
    {
        try
        {
            Book createdBook = new Book
            {
                Autor = book.Autor,
                Editorial = book.Editorial,
                Titulo = book.Titulo,
                Precio = book.Precio,
                Sinopsis = book.Sinopsis,
                Isbn = book.Isbn
            };
            _context.Books.Add(createdBook);
            _context.SaveChanges();

            return CreatedAtAction(nameof(CreateBook), new {id = createdBook.Id}, createdBook);
        } 
        catch
        {
            return Problem();
        }
    }

    [HttpDelete("{id}")]
    public void DeleteBook(int id){
        Book deleteBook = _context.Books.Find(id) ?? throw new Exception();
        _context.Books.Remove(deleteBook);
        _context.SaveChanges();
    }


    [HttpPut("{id}")]
    public IActionResult UpdateBook(int id, [FromBody]Book book)
    {
        var bookToUpdate = _context.Books.Find(id);
        if(bookToUpdate is null) 
        {
            return NotFound("Libro no encontrado");
        }

        bookToUpdate.Titulo = book.Titulo;
        bookToUpdate.Autor = book.Autor;
        bookToUpdate.Editorial = book.Editorial;
        bookToUpdate.Precio = book.Precio;
        bookToUpdate.Sinopsis = book.Sinopsis;
        bookToUpdate.Isbn = book.Isbn;

        _context.Books.Update(bookToUpdate);
        _context.SaveChanges();

        return Ok();
    }

    [HttpGet("loadData")]
    public void LoadBooks()
    {
        _context.Books.AddRange(_books);
        _context.SaveChanges();
    }
}


