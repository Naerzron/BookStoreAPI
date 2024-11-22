using System.Security.Claims;
using BookStore.API.Identity;
using BookStore.API.Models;
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

    // Obtener información del usuario autenticado
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        // Obtener el ID del usuario actual desde el token JWT
        var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userName == null)
            return Unauthorized(new { Message = "Usuario no autenticado." });

        // Buscar al usuario en la base de datos
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
            return NotFound(new { Message = "Usuario no encontrado." });

        // Retornar la información del usuario
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

    // Actualizar información del usuario autenticado
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

        // Actualizar las propiedades del usuario
        user.Name = model.Name;
        user.LastName = model.LastName;
        user.PhoneNumber = model.PhoneNumber;
        user.Address = model.Address;
        user.BirthDate = model.BirthDate;
        user.Country = model.Country;
        user.Dni = model.Dni;

        // Guardar los cambios en la base de datos
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
            return Ok(new { Message = "Perfil actualizado con éxito." });

        foreach (var error in result.Errors)
            ModelState.AddModelError("error", error.Description);

        return BadRequest(ModelState);
    }

    // Cambiar la contraseña del usuario autenticado
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

        // Intentar cambiar la contraseña
        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (result.Succeeded)
            return Ok(new { Message = "Contraseña actualizada con éxito." });

        // Manejar errores
        foreach (var error in result.Errors)
            ModelState.AddModelError("error", error.Description);

        return BadRequest(ModelState);
    }

}
