using Entities;

namespace UseCases.GetBalance;

public interface IGetBalanceRepository
{
    Account GetBalance(Guid userId);
}   