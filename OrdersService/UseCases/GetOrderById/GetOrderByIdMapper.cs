using Entities;

namespace UseCases.GetOrderById;

public static class GetOrderByIdMapper
{
    public static GetOrderByIdResponse ToResponse(Order order)
    {
        return new GetOrderByIdResponse(order.Id, order.Status);
    }
}