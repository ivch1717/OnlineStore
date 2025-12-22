namespace UseCases.GetOrderById;

public interface IGetOrderByIdRequestHandler
{
    GetOrderByIdResponse Handle(GetOrderByIdRequest request);
}