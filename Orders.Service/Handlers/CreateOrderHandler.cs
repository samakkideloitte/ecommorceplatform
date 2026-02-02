using Orders.Service.Data;
using Orders.Service.Domain;
using Orders.Service.Commands;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Orders.Service.Handlers;

public class CreateOrderHandler
{
    private readonly OrdersDbContext _db;

    public CreateOrderHandler(OrdersDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateOrderCommand command,string idemKey)
    {
        var existing = await _db.IdempotencyKeys.FirstOrDefaultAsync(x=>x.Key==idemKey);
        if (existing != null)
        {
            return existing.ResponseOrderId;
        }
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = command.CustomerId,
            Amount = command.Amount,
            CatalogItemId=command.CatalogItemId,
            CatalogItemName= command.CatalogItemName,
            Status = "CREATED",
            CreatedAt = DateTime.UtcNow
        };

        _db.Orders.Add(order);
        _db.IdempotencyKeys.Add(new IdempotencyKey
        {
            Key = idemKey,
            ResponseOrderId=order.Id,
            CreatedAt = DateTime.UtcNow
        });
       

        // 🔹 OUTBOX ENTRY (RELIABLE EVENT)
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = "OrderCreated",
            Payload = JsonSerializer.Serialize(new
            {
                OrderId = order.Id,
                
                Amount =order.Amount
            }),
            CreatedAt = DateTime.UtcNow,
            Processed = false
        };

        _db.OutboxMessages.Add(outboxMessage);

        // 🔹 ONE TRANSACTION
        await _db.SaveChangesAsync();

        return order.Id;
    }
}