using Entities;

namespace UseCases.TopUpAccount;

public interface ITopUpAccountRepository
{
    void TopUpAccount(Guid userId, int amount);
    Account GetAccount(Guid userId);
}