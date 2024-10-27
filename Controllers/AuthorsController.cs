namespace BookStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
class AuthorsController : BaseController
{
    private readonly ApplicationDbContext _context;

    public AuthorsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet()]
    public IEnumerable<Author> GetAuthors()
    {
        return _context.Authors.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<Book> GetAuthor(int id)
    {
        try
        {
            var author = _context.Authors.Find(id);
            if(author is null) return NotFound("Autor no encontrado"); // NotFound (404)

            return Ok(autor); // Ok (200)
        } 
        catch 
        {
            return Problem(); // InternalServerError (500)
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
            _context.Books.Add(createdBook);
            _context.SaveChanges();

            return CreatedAtAction(nameof(CreateAuthor), new {id = createdAuthor.Id}, createdAuthor);
        } 
        catch
        {
            return Problem();
        }
    }
}