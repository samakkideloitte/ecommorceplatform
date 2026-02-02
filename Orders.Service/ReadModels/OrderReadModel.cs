namespace Orders.Service.ReadModels
{
    public class OrderReadModel
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
