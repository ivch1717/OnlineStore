using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Infrastructure.Messaging;

public interface IRabbitMqPublisher
{
    void Publish<T>(string queueName, T message);
}

internal sealed class RabbitMqPublisher(IRabbitMqConnection conn) : IRabbitMqPublisher
{
    public void Publish<T>(string queueName, T message)
    {
        using var channel = conn.Connection.CreateModel();

        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var props = channel.CreateBasicProperties();
        props.Persistent = true;

        channel.BasicPublish(
            exchange: "",
            routingKey: queueName,
            basicProperties: props,
            body: body);
    }
}