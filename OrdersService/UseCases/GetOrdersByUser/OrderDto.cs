using Entities;

namespace UseCases.GetOrdersByUser;

public record OrderDto(
    Guid OrderId,
    int Amount,
    string Description,
    OrderStatus Status 
    );