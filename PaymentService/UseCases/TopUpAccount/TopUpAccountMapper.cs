using Entities;
using UseCases.GetBalance;

namespace UseCases.TopUpAccount;

public static class TopUpAccountMapper
{
    public static TopUpAccountResponse ToResponse(Account account)
    {
        return new TopUpAccountResponse(account.Id,  account.Balance);
    }
}