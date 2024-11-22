namespace BookStore.API.Models;

using System.ComponentModel.DataAnnotations;

public class Register
{
    [Required(ErrorMessage = "La dirección es obligatoria.")]
    public required string Address { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [DataType(DataType.Date, ErrorMessage = "Formato de fecha no válido.")]
    public required DateOnly BirthDate { get; set; }

    [Required(ErrorMessage = "La confirmación de contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
    public  required string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "El país es obligatorio.")]
    public required string Country { get; set; }

    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [StringLength(10, ErrorMessage = "El DNI no debe exceder los 10 caracteres.")]
     public required string Dni { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Debe ser una dirección de correo válida.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public required string Name { get; set; }

    [Phone(ErrorMessage = "Número de teléfono no válido.")]
    public required string PhoneNumber { get; set; }

    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    public required string UserName { get; set; }    
} 