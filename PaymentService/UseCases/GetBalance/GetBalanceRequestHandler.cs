namespace UseCases.GetBalance;

public class GetBalanceRequestHandler : IGetBalanceRequestHandler
{
    IGetBalanceRepository _repository;

    public GetBalanceRequestHandler(IGetBalanceRepository repository)
    {
        _repository = repository;
    }

    GetBalanceResponse IGetBalanceRequestHandler.Handle(GetBalanceRequest request)
    {
        if (request.AccountId == Guid.Empty)
        {
            throw new ArgumentException("Некорректный id счёта");
        }
        return GetBalanceMapper.ToResponse(_repository.GetBalance(request.AccountId));
    }
}