namespace Infrastructure.Messaging.Contracts;

public sealed record PaymentRequestedMessage(
    Guid MessageId,
    Guid OrderId,
    Guid UserId,
    int Amount
);