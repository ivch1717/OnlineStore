namespace UseCases.GetOrderById;

public class GetOrderByIdRequestHandler : IGetOrderByIdRequestHandler
{
    IGetOrderByIdRepository _repository;

    public GetOrderByIdRequestHandler(IGetOrderByIdRepository repository)
    {
        _repository = repository;
    }
    
    public GetOrderByIdResponse Handle(GetOrderByIdRequest request)
    {
        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("Некорректный id заказа");
        }
        return GetOrderByIdMapper.ToResponse(_repository.GetOrderById(request.Id));
    }
}