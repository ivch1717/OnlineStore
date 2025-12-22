namespace UseCases.CreateOrder;

public interface ICreateOrderRequestHandler
{
    CreateOrderResponse Handle(CreateOrderRequest request);
}