namespace BookStore.API.Models;

public class Book
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public required DateOnly PublishedDate { get; set; }

    public required int Stock { get; set; }

    public decimal Price { get; set; }

    public required string Synopsis { get; set; }

    public required string Isbn { get; set; }

    public required int? AuthorId { get; set; }

    public required int? GenreId { get; set; }

    public virtual Author? Author { get; set; }

    public virtual Genre? Genre { get; set; }
}