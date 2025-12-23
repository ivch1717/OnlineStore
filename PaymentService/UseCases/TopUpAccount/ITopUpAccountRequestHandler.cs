using UseCases.GetBalance;

namespace UseCases.TopUpAccount;

public interface ITopUpAccountRequestHandler
{
    TopUpAccountResponse Handle(TopUpAccountRequest request);
}