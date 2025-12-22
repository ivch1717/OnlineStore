namespace UseCases.CreateOrder;

public class CreateOrderRequestHandler : ICreateOrderRequestHandler
{
    private readonly ICreateOrderRepository _repository;

    public CreateOrderRequestHandler(ICreateOrderRepository repository)
    {
        _repository = repository;
    }

    public CreateOrderResponse Handle(CreateOrderRequest request)
    {
        var order = CreateOrderMapper.ToEntity(request, Guid.NewGuid());
        _repository.Add(order);
        return CreateOrderMapper.ToResponse(order);
    }
}