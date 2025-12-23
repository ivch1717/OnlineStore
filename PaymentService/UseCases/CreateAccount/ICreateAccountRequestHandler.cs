namespace UseCases.CreateAccount;

public interface ICreateAccountRequestHandler
{
    CreateAccountResponse Handle(CreateAccountRequest request);
}