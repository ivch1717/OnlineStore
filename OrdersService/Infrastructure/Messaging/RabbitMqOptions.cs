namespace Infrastructure.Messaging;

public sealed class RabbitMqOptions
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    
    public string PaymentRequestedQueue { get; set; } = "payment-requested";
    public string PaymentResultQueue { get; set; } = "payment-result";
}