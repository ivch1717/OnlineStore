using RabbitMQ.Client;

namespace Infrastructure.Messaging;

public interface IRabbitMqConnection : IDisposable
{
    IConnection Connection { get; }
}

internal sealed class RabbitMqConnection : IRabbitMqConnection
{
    public IConnection Connection { get; }

    public RabbitMqConnection(RabbitMqOptions options)
    {
        var factory = new ConnectionFactory
        {
            HostName = options.HostName,
            Port = options.Port,
            UserName = options.UserName,
            Password = options.Password,
            DispatchConsumersAsync = true
        };

        Connection = factory.CreateConnection();
    }

    public void Dispose() => Connection.Dispose();
}