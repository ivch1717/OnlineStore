using Entities;

namespace UseCases.TopUpAccount;

public interface ITopUpAccountRepository
{
    void TopUpAccount(Guid accountId, int amount);
    Account GetAccount(Guid accountId);
}