using Entities;

namespace UseCases.GetOrderById;

public record GetOrderByIdResponse(
    Guid OrderId,
    OrderStatus OrderStatus
    );