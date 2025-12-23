using Entities;

namespace UseCases.CreateAccount;

public interface ICreateAccountRepository
{
    void Add(Account account);
}