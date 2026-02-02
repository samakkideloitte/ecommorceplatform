namespace Orders.Service.Commands
{
    public class CreateOrderCommand
    {
        public string CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string? CatalogItemId { get; set; }
        public string? CatalogItemName { get; set; }
    }
}
