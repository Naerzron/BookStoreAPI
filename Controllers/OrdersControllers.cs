using BookStore.API.Data;
using BookStore.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly Order[] _order;

    public OrdersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet()]
    public IEnumerable<Order> GetOrders(){
        return _context.Orders.ToList();
    }
    
    [HttpGet("{id}")]
    public ActionResult<Order> GetOrder(int id){
        try
        {
            var order = _context.Orders.Find(id);
            if(order is null) return NotFound("Pedido no encontrado"); // NotFound (404)

            return Ok(order); // Ok (200)
        } 
        catch 
        {
            return Problem(); // InternalServerError (500)
        }
    }

    [HttpPost]
    public IActionResult CreateOrder(Order order)
    {
        try 
        {
            Order CreatedOrder = new Order
            {
                Amount = order.Amount,
                OrderDate = order.OrderDate,
                User = order.User,
                BookList = order.Books
            }
        }
        catch
        {
            return Problem();
        }
    }

    [HttpDelete("{id}")]
    public void DeleteOrder(int id){
        Order deleteOrder = _context.Orders.Find(id) ?? throw new Exception();
        _context.Orders.Remove(deleteOrder);
        _context.SaveChanges();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateOrder(int id, [FromBody]Order order){
        var orderToUpdate = _context.Orders.Find(id);
        if(orderToUpdate is null) 
        {
            return NotFound("Pedido no encontrado");
        }

        orderToUpdate.Name = order.Name;
        orderToUpdate.Description = order.Description;
        User = order.User,
        BookList = order.Books
        
        _context.Orders.Update(orderToUpdate);
        _context.SaveChanges();

        return Ok();
    }
}