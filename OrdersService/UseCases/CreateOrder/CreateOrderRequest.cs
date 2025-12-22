namespace UseCases.CreateOrder;

public sealed record CreateOrderRequest(
    Guid UserId,
    int Amount,
    string Description
);