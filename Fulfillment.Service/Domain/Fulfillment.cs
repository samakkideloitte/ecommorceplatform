namespace Fulfillment.Service.Domain
{
    public class Fulfillment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
