using BookStore.API.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<MyUser> _userManager; 
    private readonly SignInManager<MyUser> _signInManager; 

    public AuthController(UserManager<MyUser> userManager, SignInManager<MyUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Register model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = new MyUser 
        { 
            Address = model.Address,
            BirthDate = model.BirthDate,
            Country = model.Country,
            Dni = model.Dni,
            Email = model.Email,
            LastName = model.LastName,
            Name = model.Name,
            PhoneNumber = model.PhoneNumber,
            UserName = model.UserName
        };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Usuario");
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok(new { Message = "Usuario registrado y logueado con éxito." });
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Login model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(model.Email);

        if(user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
                return Ok(new { Message = "Inicio de sesión exitoso." });
        }

        ModelState.AddModelError("", "Inicio de sesión fallido. Por favor, verifica tus credenciales.");
        return Unauthorized(ModelState);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { Message = "Sesión cerrada con éxito." });
    }
}
