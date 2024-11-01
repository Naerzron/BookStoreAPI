public class CreateOrUpdateOrderRequest
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; }
    
    public int UserId { get; set; }

    public required IEnumerable<int> BookIds { get; set; }
}