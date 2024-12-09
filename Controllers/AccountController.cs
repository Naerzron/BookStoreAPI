using System.Security.Claims;
using BookStore.API.Identity;
using BookStore.API.Models;
using BookStore.API.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requiere que el usuario esté autenticado
public class AccountController : ControllerBase
{
    private readonly UserManager<MyUser> _userManager;

    public AccountController(UserManager<MyUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userName == null)
            return Unauthorized(new { Message = "Usuario no autenticado." });

        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
            return NotFound(new { Message = "Usuario no encontrado." });

        return Ok(new
        {
            user.Name,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.Address,
            user.BirthDate,
            user.Country,
            user.Dni
        });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfile model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userName == null)
            return Unauthorized(new { Message = "Usuario no autenticado." });

        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
            return NotFound(new { Message = "Usuario no encontrado." });

        user.Name = model.Name;
        user.LastName = model.LastName;
        user.PhoneNumber = model.PhoneNumber;
        user.Address = model.Address;
        user.BirthDate = model.BirthDate;
        user.Country = model.Country;
        user.Dni = model.Dni;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
            return Ok(new { Message = "Perfil actualizado con éxito." });

        foreach (var error in result.Errors)
            ModelState.AddModelError("error", error.Description);

        return BadRequest(ModelState);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePassword model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userName == null)
            return Unauthorized(new { Message = "Usuario no autenticado." });

        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
            return NotFound(new { Message = "Usuario no encontrado." });

        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (result.Succeeded)
            return Ok(new { Message = "Contraseña actualizada con éxito." });

        foreach (var error in result.Errors)
            ModelState.AddModelError("error", error.Description);

        return BadRequest(ModelState);
    }

    [HttpGet("all")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var userRole = "Usuario";
            var usersInRole = await _userManager.GetUsersInRoleAsync(userRole);

            var users = usersInRole.Select(user => new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                BirthDate = user.BirthDate,
                Country = user.Country,
                Dni = user.Dni
            }).ToList();

            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener los usuarios.", details = ex.Message });
        }
    }

    [HttpGet("detail/{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GetUserById(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                BirthDate = user.BirthDate,
                Country = user.Country,
                Dni = user.Dni
            };

            return Ok(userDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener el usuario.", details = ex.Message });
        }
    }

}
