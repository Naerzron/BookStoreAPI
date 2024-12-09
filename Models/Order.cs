using BookStore.API.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models
{
    public class Order
    {
        public int Id { get; set; }

        public virtual ICollection<OrderDetail> Details { get; set; } = new List<OrderDetail>();

        public required MyUser User { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }
    }
}