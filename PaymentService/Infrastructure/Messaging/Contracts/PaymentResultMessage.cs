namespace Infrastructure.Messaging.Contracts;

public enum PaymentStatus : short
{
    Succeeded = 1,
    Failed = 2
}

public sealed record PaymentResultMessage(
    Guid MessageId,
    Guid OrderId,
    PaymentStatus Status
);