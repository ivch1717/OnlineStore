using Infrastructure.Data.Db;
using Infrastructure.Messaging.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging;

public sealed class PaymentsOutboxPublisherHostedService(
    IServiceScopeFactory scopeFactory,
    RabbitMqOptions opts,
    IRabbitMqPublisher publisher,
    ILogger<PaymentsOutboxPublisherHostedService> log
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        log.LogInformation("Payments Outbox Publisher started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<PaymentServiceDbContext>();

                var batch = await db.OutboxPaymentResults
                    .Where(x => x.PublishedAt == null)
                    .OrderBy(x => x.CreatedAt)
                    .Take(50)
                    .ToListAsync(stoppingToken);

                if (batch.Count == 0)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                    continue;
                }

                foreach (var row in batch)
                {
                    var msg = new PaymentResultMessage(
                        MessageId: row.MessageId,
                        OrderId: row.OrderId,
                        Status: (PaymentStatus)row.Status);

                    publisher.Publish(opts.PaymentResultQueue, msg);
                    row.PublishedAt = DateTime.UtcNow;
                }

                await db.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Payments outbox publish failed. Will retry.");
                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }
        }
    }
}
