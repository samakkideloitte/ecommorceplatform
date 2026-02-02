using Microsoft.EntityFrameworkCore;
using Fulfillment.Service.Data;
using Fulfillment.Service.Domain;

namespace Fulfillment.Service.Handlers;

public class CreateFulfillmentHandler
{
    private readonly FulfillmentDbContext _db;

    public CreateFulfillmentHandler(FulfillmentDbContext db)
    {
        _db = db;
    }

    public async Task<Guid?> Handle(Guid orderId)
    {
        if (await _db.Fulfillments.AnyAsync(f => f.OrderId == orderId))
            return null;

        var fulfillment = new Fulfillment.Service.Domain.Fulfillment
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            Status = "SHIPPED",
            CreatedAt = DateTime.UtcNow
        };

        _db.Fulfillments.Add(fulfillment);
        await _db.SaveChangesAsync();

        return fulfillment.Id;
    }
}