using Payments.Service.Data;
using Payments.Service.Domain;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Payments.Service.Handlers;

public class CreatePaymentHandler
{
    private readonly PaymentsDbContext _db;

    public CreatePaymentHandler(PaymentsDbContext db)
    {
        _db = db;
    }

    public async Task<Guid?> Handle(Guid orderId, decimal amount)
    {
        // Prevent duplicate payments
        if (await _db.Payments.AnyAsync(p => p.OrderId == orderId))
            return null;

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            Amount = amount,
            Status = "PAID",
            CreatedAt = DateTime.UtcNow
        };

        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = "PaymentCompleted",
            Payload = JsonSerializer.Serialize(new
            {
                OrderId = orderId
            }),
            CreatedAt = DateTime.UtcNow,
            Processed = false
        };

        _db.Payments.Add(payment);
        _db.OutboxMessages.Add(outboxMessage);

        //ONE transaction, ONE commit
        await _db.SaveChangesAsync();

        return payment.Id;
    }
}