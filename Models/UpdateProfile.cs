namespace BookStore.API.Models;
public class UpdateProfile
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public string Country { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
}
