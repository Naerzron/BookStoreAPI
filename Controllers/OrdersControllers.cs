using Microsoft.EntityFrameworkCore;
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
        var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userName == null)
            return Unauthorized(new { Message = "Usuario no autenticado." });

        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
            return NotFound(new { Message = "Usuario no encontrado." });

        var orders = await _context.Orders
            .Where(o => o.User.Id == user.Id)
            .Include(o => o.Details)
            .ThenInclude(d => d.Book)
            .ToListAsync();

        return Ok(orders);
    }

    [HttpGet("{id}")]
    public ActionResult<Order> GetOrder(int id)
    {
        try
        {
            var order = _context.Orders.Find(id);
            if (order is null) return NotFound("Pedido no encontrado");

            return Ok(order);
        }
        catch
        {
            return Problem();
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

            // Calcular el total del pedido
            decimal totalAmount = 0;

            var order = new Order
            {
                User = user,
                Details = new List<OrderDetail>(),
                CreatedDate = DateTime.UtcNow, // Fecha actual
                UpdatedDate = DateTime.UtcNow, // Inicializar igual que CreatedDate
                Status = "Pending", // Estado inicial del pedido
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

                // Calcular el total del detalle
                decimal itemTotal = book.Price * item.Quantity;

                totalAmount += itemTotal;

                var orderDetail = new OrderDetail
                {
                    Book = book,
                    Quantity = item.Quantity
                };

                order.Details.Add(orderDetail);
            }

            // Asignar el total calculado
            order.TotalAmount = totalAmount;

            Console.WriteLine($"Order created with {order.Details.Count} items and total amount {totalAmount}.");

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