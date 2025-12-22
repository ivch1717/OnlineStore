using Entities;

namespace UseCases.GetOrdersByUser;

public static class GetOrdersByUserMapper
{
    public static GetOrdersByUserResponse ToResponse(
        Guid userId,
        IReadOnlyList<Order> orders
    )
    {
        var orderDtos = orders
            .Select(order => new OrderDto(
                order.Id,
                order.Amount,
                order.Description,
                order.Status
            ))
            .ToList();

        return new GetOrdersByUserResponse(
            userId,
            orderDtos
        );
    }
}