using BookStore.API.Data;
using BookStore.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrdersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet()]
    public ActionResult<IEnumerable<Order>> GetOrders(){
        return Ok(_context.Orders.ToList());
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
    public IActionResult CreateOrder(CreateOrUpdateOrderRequest createOrderRequest)
    {
        try 
        {
            //var user = _context.Users.Find() Esto hay que ver si se puede cambiar por la gesiton de usuarios de .net.
            var books = new List<Book>();
            var bookIds = createOrderRequest.BookIds;
            foreach(var bookId in bookIds)
            {
                var book = _context.Books.Find(bookId);
                if(book == null)
                {
                    return BadRequest(); // Libro no existe
                }
                
                books.Add(book);                
            }

            var totalAmount = 0m;
            foreach(var book in books)
            {
                totalAmount += book.Price;
            }


            Order createdOrder = new Order
            {
                Amount = totalAmount,
                OrderDate = createOrderRequest.OrderDate,
                //User = null, // CAMBIAR ESTO POR LOS USUARIOS DE .NET
                Books = books
            };
            
             _context.Orders.Add(createdOrder);
            _context.SaveChanges();

            return CreatedAtAction(nameof(CreateOrder), new {id = createdOrder.Id}, createdOrder);
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
        return NotFound();
        // var orderToUpdate = _context.Orders.Find(id);
        // if(orderToUpdate is null) 
        // {
        //     return NotFound("Pedido no encontrado");
        // }

        // orderToUpdate.Name = order.Name;
        // orderToUpdate.Description = order.Description;
        // User = order.User,
        // BookList = order.Books
        
        // _context.Orders.Update(orderToUpdate);
        // _context.SaveChanges();

        // return Ok();
    }
}