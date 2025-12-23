using Entities;
using Infrastructure.Data.Db;
using Infrastructure.Data.Mapper;
using UseCases.CreateAccount;

namespace Infrastructure.Data.Repositories;

internal sealed class CreateAccountRepository(PaymentServiceDbContext db) : ICreateAccountRepository
{
    public void Add(Account account)
    {
        db.Accounts.Add(account.ToDto());
        db.SaveChanges();
    }
}