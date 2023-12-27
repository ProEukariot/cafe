using Microsoft.AspNetCore.Mvc.ModelBinding;

public class Product
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Category { get; set; }
    public string? Size { get; set; }
    public string? Description { get; set; }
}