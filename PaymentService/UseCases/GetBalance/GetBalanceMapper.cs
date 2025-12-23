using Entities;

namespace UseCases.GetBalance;

public static class GetBalanceMapper
{
    public static GetBalanceResponse ToResponse(Account account)
    {
        return new GetBalanceResponse(account.Id,  account.Balance);
    }
}