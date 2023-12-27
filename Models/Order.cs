public class Order
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime OrderTimestamp { get; set; }
    public string? Status { get; set; }

}