using System.Text;
using System.Text.Json;
using Entities;
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

public sealed class OrdersPaymentResultConsumerHostedService(
    IServiceScopeFactory scopeFactory,
    IRabbitMqConnection conn,
    RabbitMqOptions opts,
    ILogger<OrdersPaymentResultConsumerHostedService> log
) : BackgroundService
{
    private IModel? _channel;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = conn.Connection.CreateModel();

        _channel.QueueDeclare(
            queue: opts.PaymentResultQueue,
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
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var msg = JsonSerializer.Deserialize<PaymentResultMessage>(json)
                          ?? throw new InvalidOperationException("PaymentResultMessage deserialization failed");

                using var scope = scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<OrderServiceDbContext>();

                await using var tx = await db.Database.BeginTransactionAsync(stoppingToken);
                
                db.InboxMessages.Add(new OrdersInboxMessageDto
                {
                    MessageId = msg.MessageId,
                    ProcessedAt = DateTime.UtcNow
                });

                await db.SaveChangesAsync(stoppingToken); 

                var newStatus = msg.Status == PaymentStatus.Succeeded
                    ? OrderStatus.Finished
                    : OrderStatus.Canceled;

                await db.Database.ExecuteSqlInterpolatedAsync(
                    $@"UPDATE ""Orders""
                       SET ""OrderStatus"" = {(int)newStatus}
                       WHERE ""Id"" = {msg.OrderId};",
                    stoppingToken);

                await tx.CommitAsync(stoppingToken);

                _channel.BasicAck(ea.DeliveryTag, multiple: false);
            }
            catch (DbUpdateException dup)
            {
                log.LogWarning(dup, "Duplicate PaymentResult, ACK.");
                _channel?.BasicAck(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to process PaymentResult. NACK requeue.");
                _channel?.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        _channel.BasicConsume(queue: opts.PaymentResultQueue, autoAck: false, consumer: consumer);

        log.LogInformation("Orders PaymentResult consumer started.");
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        base.Dispose();
    }
}
