using Infrastructure.Data.Db;
using Microsoft.EntityFrameworkCore;
using UseCases.TopUpAccount;

namespace Infrastructure.Data.Repositories;

internal sealed class TopUpAccountRepository(PaymentServiceDbContext db) : ITopUpAccountRepository
{
    public void TopUpAccount(Guid userId, int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than 0.");

        var dto = db.Accounts
            .AsNoTracking()
            .SingleOrDefault(x => x.UserId == userId);

        if (dto is null)
            throw new InvalidOperationException($"User '{userId}' not found.");

        var updated = dto with { Balance = dto.Balance + amount };

        db.Accounts.Update(updated);
        db.SaveChanges();
    }

    public Entities.Account GetAccount(Guid userId)
    {
        var dto = db.Accounts
            .AsNoTracking()
            .SingleOrDefault(x => x.UserId == userId);

        if (dto is null)
            throw new InvalidOperationException($"User '{userId}' not found.");

        return Infrastructure.Data.Mapper.DataMapper.ToEntity(dto);
    }
}