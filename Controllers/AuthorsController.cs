using BookStore.API.Data;
using BookStore.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthorsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet()]
    public ActionResult<IEnumerable<Author>> GetAuthors()
    {
        return Ok(_context.Authors.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Book> GetAuthor(int id)
    {
        try
        {
            var author = _context.Authors.Find(id);
            if(author is null) return NotFound("Autor no encontrado"); 

            return Ok(author); 
        } 
        catch 
        {
            return Problem(); 
        }
    }

    [HttpPost]
    public IActionResult CreateAuthor(Author author)
    {
        try
        {
            Author createdAuthor = new Author
            {
                Name = author.Name,
                BirthDay = author.BirthDay
            };
            _context.Authors.Add(createdAuthor);
            _context.SaveChanges();

            return CreatedAtAction(nameof(CreateAuthor), new {id = createdAuthor.Id}, createdAuthor);
        } 
        catch
        {
            return Problem();
        }
    }

    [HttpDelete("{id}")]
    public void DeleteAuthor(int id){
        Author deleteAuthor = _context.Authors.Find(id) ?? throw new Exception();
        _context.Authors.Remove(deleteAuthor);
        _context.SaveChanges();
    }
}