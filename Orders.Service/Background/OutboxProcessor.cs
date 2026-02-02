using Microsoft.EntityFrameworkCore;
using Orders.Service.Data;
using Orders.Service.Events;
using System.Net.Http.Json;
using System.Text.Json;

namespace Orders.Service.Background;

public class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHttpClientFactory _httpClientFactory;

    public OutboxProcessor(
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
            var db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
            var client = _httpClientFactory.CreateClient();

            var messages = await db.OutboxMessages
                .Where(x => !x.Processed)
                .OrderBy(x => x.CreatedAt)
                .Take(5)
                .ToListAsync(stoppingToken);

            foreach (var message in messages)
            {
                if (message.Type == "OrderCreated")
                {
                    var payload = JsonSerializer.Deserialize<OrderCreatedEvent>(message.Payload);
                    if (payload == null)
                        continue;
                    var response = await client.PostAsJsonAsync(
                        "http://localhost:5295/api/payments",
                        new
                        {
                            orderId = payload.OrderId,
                            amount = payload.Amount
                        });

                    if (response.IsSuccessStatusCode)
                    {
                        message.Processed = true;
                    }
                }
            }

            await db.SaveChangesAsync(stoppingToken);
            await Task.Delay(3000, stoppingToken);
        }
    }
}