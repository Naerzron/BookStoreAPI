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
    public ActionResult<IEnumerable<Order>> GetOrders(){
        return Ok(_context.Orders.ToList());
    }

    [HttpGet("user")]
    public IEnumerable<Order> GetOrdersByUser()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return _context.Orders.Where(o => o.User.Id == userId);
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
    public async Task<IActionResult> CreateOrder(CreateOrderRequest createOrderRequest)
    {
        try 
        {   
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest();
            }

            Order createdOrder = new Order
            {
                Details = createOrderRequest.Details,
                User = user
            };
            
             _context.Orders.Add(createdOrder);
            _context.SaveChanges();

            return Created();
        }
        catch
        {
            return Problem();
        }
    }
}