using Infrastructure.Data.Db;
using Microsoft.EntityFrameworkCore;
using UseCases.TopUpAccount;

namespace Infrastructure.Data.Repositories;

internal sealed class TopUpAccountRepository(PaymentServiceDbContext db) : ITopUpAccountRepository
{
    public void TopUpAccount(Guid accountId, int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than 0.");

        var dto = db.Accounts
            .SingleOrDefault(x => x.Id == accountId);

        if (dto is null)
            throw new InvalidOperationException($"Account '{accountId}' not found.");

        dto = dto with { Balance = dto.Balance + amount };

        db.Accounts.Update(dto);
        db.SaveChanges();
    }

    public Entities.Account GetAccount(Guid accountId)
    {
        var dto = db.Accounts
            .AsNoTracking()
            .SingleOrDefault(x => x.Id == accountId);

        if (dto is null)
            throw new InvalidOperationException($"Account '{accountId}' not found.");

        return Infrastructure.Data.Mapper.DataMapper.ToEntity(dto);
    }
}