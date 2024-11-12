using BookStore.API.Data;
using BookStore.API.Models;
using BookStore.API.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    // Inyectar el DbContext a través del constructor
    public BooksController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet()]
    public ActionResult<IEnumerable<GetBookResponse>> GetBooks()
    {
        var booksResponse = _context.Books.Select(b =>
             new GetBookResponse
            {
                Id = b.Id,
                Title = b.Title,
                Synopsis = b.Synopsis,
                GenreName = b.Genre.Name,
                Image = b.Image,
                Isbn = b.Isbn,
                PublishedDate = b.PublishedDate,
                Stock = b.Stock,
                AuthorName = b.Author.Name,
                Price = b.Price
            }
        );

        return Ok(booksResponse); // HTTP 200 OK
    }

    [HttpGet("{id}")]
    public ActionResult<GetBookResponse> GetBook(int id)
    {
        try
        {
            var book = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .FirstOrDefault(b => b.Id == id);
            if(book is null) return NotFound("Libro no encontrado"); // NotFound (404)

            var bookResponse = new GetBookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Synopsis = book.Synopsis,
                GenreName = book.Genre.Name,
                Image = book.Image,
                Isbn = book.Isbn,
                PublishedDate = book.PublishedDate,
                Stock = book.Stock,
                AuthorName = book.Author.Name,
                Price = book.Price
            };

            return Ok(bookResponse); // Ok (200)
        } 
        catch 
        {
            return Problem(); // InternalServerError (500)
        }
    }

    [HttpPost]
    public IActionResult CreateBook(CreateBookRequest createBookRequest)
    {
        try
        {
            Author? author = null;
            Genre? genre = null; 

            if(createBookRequest.AuthorId.HasValue)
            {
                author = _context.Authors.Find(createBookRequest.AuthorId);
                if(author == null)
                {
                    return BadRequest();
                }
            }

            if(createBookRequest.GenreId.HasValue)
            {
                genre = _context.Genres.Find(createBookRequest.GenreId);
                if(genre == null)
                {
                    return BadRequest();
                }
            }

            Book createdBook = new Book
            {
                Title = createBookRequest.Title,
                Price = createBookRequest.Price,
                Synopsis = createBookRequest.Synopsis,
                Isbn = createBookRequest.Isbn,
                PublishedDate = createBookRequest.PublishedDate,
                Stock = createBookRequest.Stock,
                Author = author,
                Genre = genre,
                Image = createBookRequest.Image
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
    public IActionResult DeleteBook(int id){
        Book? deleteBook = _context.Books.Find(id);

        if(deleteBook == null){
            return NotFound();
        }
        _context.Books.Remove(deleteBook);
        _context.SaveChanges();
        return Ok();
        
    }


    [HttpPut("{id}")]
    public IActionResult UpdateBook(int id, [FromBody]Book book)
    {
        var bookToUpdate = _context.Books.Find(id);
        if(bookToUpdate is null) 
        {
            return NotFound("Libro no encontrado");
        }

        bookToUpdate.Author = book.Author;
        bookToUpdate.Title = book.Title;
        bookToUpdate.Price = book.Price;
        bookToUpdate.Synopsis = book.Synopsis;
        bookToUpdate.Isbn = book.Isbn;
        bookToUpdate.PublishedDate = book.PublishedDate;
        bookToUpdate.Genre = book.Genre;
        bookToUpdate.Stock = book.Stock;
        

        _context.Books.Update(bookToUpdate);
        _context.SaveChanges();

        return Ok();
    }
}


