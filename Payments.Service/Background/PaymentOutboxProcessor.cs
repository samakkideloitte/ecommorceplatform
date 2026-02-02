using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using Payments.Service.Data;
using System.Text.Json;
using Payments.Service.Events;

namespace Payments.Service.Background;

public class PaymentOutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHttpClientFactory _httpClientFactory;

    public PaymentOutboxProcessor(
        IServiceScopeFactory scopeFactory,
        IHttpClientFactory httpClientFactory)
    {
        _scopeFactory = scopeFactory;
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();
            var client = _httpClientFactory.CreateClient();

            var messages = await db.OutboxMessages
                .Where(x => !x.Processed)
                .OrderBy(x => x.CreatedAt)
                .Take(5)
                .ToListAsync(stoppingToken);

            foreach (var msg in messages)
            {
                try
                {
                    if (msg.Type == "PaymentCompleted")
                    {
                        var payload =
                            JsonSerializer.Deserialize<PaymentCompletedEvent>(msg.Payload);

                        var response = await client.PostAsJsonAsync(
                            "http://localhost:5132/api/fulfillment",
                            new { orderId = payload!.OrderId },
                            stoppingToken);

                        if (response.IsSuccessStatusCode)
                            msg.Processed = true;
                    }
                }
                catch
                {
                    // retry next loop
                }
            }

            await db.SaveChangesAsync(stoppingToken);
            await Task.Delay(3000, stoppingToken);
        }
    }
}