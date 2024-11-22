namespace BookStore.API.Models;
using System.ComponentModel.DataAnnotations;

public class Login
{
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Debe ser una dirección de correo válida.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

}