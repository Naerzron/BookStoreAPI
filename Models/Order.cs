namespace BookStore.API.Models;

public class Order
{
    public int Id { get; set; }

    public required decimal Amount { get; set; }

    public required DateTime OrderDate { get; set; }

    // public required virtual User User { get; set; }

    public required virtual ICollection<Book> Books { get; set;}
}