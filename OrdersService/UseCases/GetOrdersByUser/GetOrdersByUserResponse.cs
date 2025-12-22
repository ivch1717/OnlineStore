namespace UseCases.GetOrdersByUser;

public record GetOrdersByUserResponse (
    Guid UserId,
    IReadOnlyList<OrderDto> Orders
    );