namespace BookStore.API.Models;

public class Genre 
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string Description { get; set; }

    public virtual Book Book { get; set; }
}