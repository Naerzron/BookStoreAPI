using Microsoft.AspNetCore.Identity;

namespace BookStore.API.Identity
{
    public class MyUser : IdentityUser
    {
        public required string Address {get; set;}
        public required DateOnly BirthDate { get; set; }
        public required string Country { get; set; }
        public required string Dni { get; set; }
        public required string LastName { get; set; }
        public required string Name { get; set; }

        

        

    }
}