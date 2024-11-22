using BookStore.API.Identity;

namespace BookStore.API.Models;

    public class Order
    {
        public int Id { get; set; }
        public required IEnumerable<OrderDetail> Details { get; set; }
        public required MyUser User { get; set; }
    }
