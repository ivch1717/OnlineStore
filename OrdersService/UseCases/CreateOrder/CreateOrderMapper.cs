using Entities;

namespace UseCases.CreateOrder;

public static class CreateOrderMapper
{
    public static Order ToEntity(CreateOrderRequest request)
    {
        return new Order(
            userId:  request.UserId,
            amount:  request.Amount,
            description:  request.Description
        );
    }

    public static CreateOrderResponse ToResponse(Order order)
    {
        return new CreateOrderResponse(
            order.Id
        );
    }
}