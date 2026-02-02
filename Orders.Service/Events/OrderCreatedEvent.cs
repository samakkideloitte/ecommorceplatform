namespace Orders.Service.Events
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public string? CatalogItemId { get; set; }
        public string? CatalogItemName { get; set; }
    }
}
