namespace BookStore.API.Responses;

public class GetBookResponse
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public required DateOnly PublishedDate { get; set; }

    public required int Stock { get; set; }

    public decimal Price { get; set; }

    public required string Synopsis { get; set; }

    public required string Isbn { get; set; }

    public required string AuthorName { get; set; }

    public required string GenreName { get; set; }

    public required string Image { get; set; }
}