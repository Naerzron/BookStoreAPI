using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookStore.API.Identity;
using BookStore.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<MyUser> _userManager; 
    private readonly SignInManager<MyUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<MyUser> userManager, SignInManager<MyUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Register model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
            
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
            return Ok(new { Message = "Usuario registrado con éxito." });
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("error", error.Description);

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
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, lockoutOnFailure: false);

            if (result.Succeeded && !user.UserName.IsNullOrEmpty()) {
                var roles = await _userManager.GetRolesAsync(user);
                var token = GenerateJwtToken(user.Id, user.UserName ?? "", roles.First());
                return Ok(new { Token = token });
            }
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

    private string GenerateJwtToken(string userId, string username, string role)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("userId", userId),
            new Claim("role", role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"] ?? ""));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"], 
            audience: _configuration["Jwt:Audience"], 
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        return jwtToken;
    }
}
