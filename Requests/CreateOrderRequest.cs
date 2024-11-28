using BookStore.API.Models;

public class CreateOrderRequest
{
    public required IEnumerable<OrderItem> Items { get; set; }
}
