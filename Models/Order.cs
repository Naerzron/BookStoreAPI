namespace BookStore.API.Models;

public class Order
{
    public int Id { get; set; }

    public required decimal Amount { get; set; }

    public required DateOnly OrderDate { get; set; }

    public virtual User User { get; set; }

    public virtual ICollection<Book> Books { get; set;}
}