using System.Text;
using System.Text.Json;
using Infrastructure.Data.Db;
using Infrastructure.Data.Dtos;
using Infrastructure.Messaging.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Messaging;

public sealed class PaymentsPaymentRequestedConsumerHostedService(
    IRabbitMqConnection conn,
    RabbitMqOptions opts,
    IServiceScopeFactory scopeFactory,
    ILogger<PaymentsPaymentRequestedConsumerHostedService> log
) : BackgroundService
{
    private IModel? _channel;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = conn.Connection.CreateModel();
        _channel.QueueDeclare(
            opts.PaymentRequestedQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        _channel.BasicQos(0, prefetchCount: 10, global: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (_, ea) =>
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<PaymentServiceDbContext>();

                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var msg = JsonSerializer.Deserialize<PaymentRequestedMessage>(json)
                          ?? throw new InvalidOperationException("PaymentRequestedMessage deserialization failed");

                await using var tx = await db.Database.BeginTransactionAsync(stoppingToken);

                // Inbox (dedupe)
                db.InboxMessages.Add(new InboxMessageDto
                {
                    MessageId = msg.MessageId,
                    ProcessedAt = DateTime.UtcNow
                });

                await db.SaveChangesAsync(stoppingToken); 

                var alreadyPaid = await db.Payments
                    .AnyAsync(x => x.OrderId == msg.OrderId, stoppingToken);

                if (alreadyPaid)
                {
                    await tx.CommitAsync(stoppingToken);
                    _channel.BasicAck(ea.DeliveryTag, false);
                    return;
                }

                var affected = await db.Database.ExecuteSqlInterpolatedAsync(
                    $@"UPDATE ""Accounts""
                       SET ""Balance"" = ""Balance"" - {msg.Amount}
                       WHERE ""UserId"" = {msg.UserId}
                         AND ""Balance"" >= {msg.Amount};",
                    stoppingToken);

                var status = affected == 1
                    ? PaymentStatus.Succeeded
                    : PaymentStatus.Failed;

                var paymentId = Guid.NewGuid();

                db.Payments.Add(new PaymentDto
                {
                    PaymentId = paymentId,
                    OrderId = msg.OrderId,
                    UserId = msg.UserId,
                    Amount = msg.Amount,
                    Status = (short)status,
                    CreatedAt = DateTime.UtcNow
                });

                db.OutboxPaymentResults.Add(new PaymentResultOutboxDto
                {
                    MessageId = paymentId,
                    OrderId = msg.OrderId,
                    Status = (short)status,
                    CreatedAt = DateTime.UtcNow,
                    PublishedAt = null
                });

                await db.SaveChangesAsync(stoppingToken);
                await tx.CommitAsync(stoppingToken);

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (DbUpdateException dbEx)
            {
                log.LogWarning(dbEx, "Duplicate PaymentRequested, ACK.");
                _channel?.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to process PaymentRequested. NACK requeue.");
                _channel?.BasicNack(ea.DeliveryTag, false, requeue: true);
            }
        };

        _channel.BasicConsume(
            queue: opts.PaymentRequestedQueue,
            autoAck: false,
            consumer: consumer);

        log.LogInformation("Payments consumer started.");
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        base.Dispose();
    }
}
