using System.Text.Json.Serialization;

namespace BookStore.API.Models;

public class OrderDetail
{
    public int Id { get; set; }
    public required Book Book { get; set; }
    public int Quantity { get; set; }
    public int OrderId { get; set; } // Clave foránea explícita
     
    [JsonIgnore] 
    public virtual Order Order { get; set; } // Relación inversa con Order
}