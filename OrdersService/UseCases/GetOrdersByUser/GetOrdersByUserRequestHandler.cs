namespace UseCases.GetOrdersByUser;

public class GetOrdersByUserRequestHandler : IGetOrdersByUserRequestHandler
{
    IGetOrdersByUserRepository _repository;

    public GetOrdersByUserRequestHandler(IGetOrdersByUserRepository repository)
    {
        _repository = repository;
    }

    public GetOrdersByUserResponse Handle(GetOrdersByUserRequest request)
    {
        if (request.UserId == Guid.Empty)
        {
            throw new ArgumentException("Некорректный id пользователя");
        }
        return GetOrdersByUserMapper.ToResponse(request.UserId, _repository.GetOrdersByUser(request.UserId));
    }
}