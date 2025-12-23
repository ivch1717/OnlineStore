namespace UseCases.CreateAccount;

public class CreateAccountRequestHandler : ICreateAccountRequestHandler
{
    ICreateAccountRepository _repository;

    public CreateAccountRequestHandler(ICreateAccountRepository repository)
    {
        _repository = repository;
    }
    
    public CreateAccountResponse Handle(CreateAccountRequest request)
    {
        var account = CreateAccountMapper.ToEntity(request, Guid.NewGuid());
        _repository.Add(account);
        return CreateAccountMapper.ToResponse(account);
    }
}