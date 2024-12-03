using BookStore.API.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models
{
    public class Order
    {
        public int Id { get; set; }

        // Relación con los detalles del pedido
        public virtual ICollection<OrderDetail> Details { get; set; } = new List<OrderDetail>();

        // Usuario que realiza el pedido
        public required MyUser User { get; set; }

        // Fecha de creación del pedido
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Fecha de última actualización del pedido
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        // Estado del pedido (ejemplo: Pendiente, Procesado, Cancelado)
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        // Total del pedido
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }
    }
}