using BookStore.API.Data;
using BookStore.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GenresController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet()]
    public ActionResult<IEnumerable<Genre>> GetGenres(){
        return Ok(_context.Genres.ToList());
    }
    
    [HttpGet("{id}")]
    public ActionResult<Genre> GetGenre(int id){
        try
        {
            var genre = _context.Genres.Find(id);
            if(genre is null) return NotFound("Género no encontrado"); // NotFound (404)

            return Ok(genre); // Ok (200)
        } 
        catch 
        {
            return Problem(); // InternalServerError (500)
        }
    }

    [HttpPost]
    public IActionResult CreateGenre(Genre genre)
    {
        try 
        {
            Genre createdGenre = new Genre
            {
                Name = genre.Name,
                Description = genre.Description
            };

            _context.Genres.Add(createdGenre);
            _context.SaveChanges();

            return CreatedAtAction(nameof(CreateGenre), new {id = createdGenre}, createdGenre);
        }
        catch
        {
            return Problem();
        }
    }

    [HttpDelete("{id}")]
    public void DeleteGenre(int id){
        Genre deleteGenre = _context.Genres.Find(id) ?? throw new Exception();
        _context.Genres.Remove(deleteGenre);
        _context.SaveChanges();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateGenre(int id, [FromBody]Genre genre){
        var genreToUpdate = _context.Genres.Find(id);
        if(genreToUpdate is null) 
        {
            return NotFound("Género no encontrado");
        }

        genreToUpdate.Name = genre.Name;
        genreToUpdate.Description = genre.Description;

        _context.Genres.Update(genreToUpdate);
        _context.SaveChanges();

        return Ok();
    }
}