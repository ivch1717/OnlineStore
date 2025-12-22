namespace UseCases.GetOrdersByUser;

public interface IGetOrdersByUserRequestHandler
{
    GetOrdersByUserResponse Handle(GetOrdersByUserRequest request);
}