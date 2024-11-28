using System.Data.Entity;
using System.Security.Claims;
using BookStore.API.Data;
using BookStore.API.Identity;
using BookStore.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace BookStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<MyUser> _userManager;

    public OrdersController(ApplicationDbContext context, UserManager<MyUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet()]
    public ActionResult<IEnumerable<Order>> GetOrders()
    {
        return Ok(_context.Orders.ToList());
    }

    [HttpGet("user")]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUser()
    {
       // Obtener el ID del usuario actual desde el token JWT
        var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userName == null)
            return Unauthorized(new { Message = "Usuario no autenticado." });

        // Buscar al usuario en la base de datos
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
            return NotFound(new { Message = "Usuario no encontrado." });
            
        var orders = _context.Orders.Where(o => o.User.Id == user.Id).Include(o => o.Details).ToList();
        var orderDetails = _context.OrderDetails.ToList();
        orders.ForEach(o => o.Details = orderDetails.Where(d => d.OrderId == o.Id).ToList());

        return orders;
    }

    [HttpGet("{id}")]
    public ActionResult<Order> GetOrder(int id)
    {
        try
        {
            var order = _context.Orders.Find(id);
            if (order is null) return NotFound("Pedido no encontrado"); // NotFound (404)

            return Ok(order); // Ok (200)
        }
        catch
        {
            return Problem(); // InternalServerError (500)
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest createOrderRequest)
    {
        try
        {
            // Obtener el ID del usuario actual desde el token JWT
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userName == null)
                return Unauthorized(new { Message = "Usuario no autenticado." });

            // Buscar al usuario en la base de datos
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return NotFound(new { Message = "Usuario no encontrado." });

            if (createOrderRequest.Items == null || !createOrderRequest.Items.Any())
            {
                Console.WriteLine("Order does not contain items.");
                return BadRequest("The order must contain at least one item.");
            }

            var order = new Order
            {
                User = user,
                Details = new List<OrderDetail>()
            };

            foreach (var item in createOrderRequest.Items)
            {
                Console.WriteLine($"Processing item: BookId={item.BookId}, Quantity={item.Quantity}");
                var book = await _context.Books.FindAsync(item.BookId);
                if (book == null)
                {
                    Console.WriteLine($"Book with ID {item.BookId} not found.");
                    return BadRequest($"Book with ID {item.BookId} does not exist.");
                }

                var orderDetail = new OrderDetail
                {
                    Book = book,
                    Quantity = item.Quantity
                };

                order.Details.Add(orderDetail);
            }

            Console.WriteLine($"Order created with {order.Details.Count} items.");
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            Console.WriteLine($"Order saved with ID: {order.Id}");
            return CreatedAtAction(nameof(CreateOrder), new { id = order.Id }, order);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception: {ex.Message}");
            return StatusCode(500, "An error occurred while creating the order.");
        }
    }


}