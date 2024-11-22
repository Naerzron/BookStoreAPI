using BookStore.API.Models;

public class CreateOrderRequest
{
    public required IEnumerable<OrderDetail> Details { get; set; }
}