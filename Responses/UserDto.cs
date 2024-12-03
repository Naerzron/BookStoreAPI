namespace BookStore.API.Responses
{
    public class UserDto
    {
        public required string Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
        public DateOnly BirthDate { get; set; }
        public required string Country { get; set; }
        public required string Dni { get; set; }
    }
}
