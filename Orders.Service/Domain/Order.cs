namespace Orders.Service.Domain;

public class Order
{
    public Guid Id { get; set; }
    public string CustomerId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? CatalogItemId { get; set; }
    public string? CatalogItemName { get; set; }
}