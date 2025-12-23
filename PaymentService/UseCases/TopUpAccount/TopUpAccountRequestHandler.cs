namespace UseCases.TopUpAccount;

public class TopUpAccountRequestHandler : ITopUpAccountRequestHandler
{
    ITopUpAccountRepository _repository;

    public TopUpAccountRequestHandler(ITopUpAccountRepository repository)
    {
        _repository = repository;
    }

    public TopUpAccountResponse Handle(TopUpAccountRequest request)
    {
        if (request.AccountId == Guid.Empty)
        {
            throw new ArgumentException("Некорректный id счёта");
        }

        if (request.Amount <= 0)
        {
            throw new ArgumentException("Некорректный пополнить счёт на не положительное число");
        }
        _repository.TopUpAccount(request.AccountId, request.Amount);
        return TopUpAccountMapper.ToResponse(_repository.GetAccount(request.AccountId));
    }
}