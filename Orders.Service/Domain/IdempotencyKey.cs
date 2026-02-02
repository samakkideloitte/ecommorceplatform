namespace Orders.Service.Domain
{
    public class IdempotencyKey
    {
        public string Key { get; set; }
        public Guid ResponseOrderId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
