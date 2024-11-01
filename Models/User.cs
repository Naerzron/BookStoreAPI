namespace BookStore.API.Models;

public class User
{
    public int Id { get; set; }

    public required string Username { get; set; }

    public required string Password { get; set; }

    public required string Dni { get; set; }

    public required DateOnly BirthDay { get; set; }

    public required string Name { get; set; }

    public required string LastName { get; set; }

    public required string Address { get; set; }

    public required string Country { get; set; }

    public required string Email { get; set; }

    public required string Phone { get; set; }
}