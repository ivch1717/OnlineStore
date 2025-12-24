using Entities;
using Infrastructure.Data.Db;
using Infrastructure.Data.Mapper;
using Microsoft.EntityFrameworkCore;
using UseCases.GetBalance;

namespace Infrastructure.Data.Repositories;

internal sealed class GetBalanceRepository(PaymentServiceDbContext db) : IGetBalanceRepository
{
    public Account GetBalance(Guid userId)
    {
        var dto = db.Accounts
            .AsNoTracking()
            .SingleOrDefault(x => x.UserId == userId);

        if (dto is null)
        {
            throw new InvalidOperationException($"User '{userId}' not found.");
        }

        return dto.ToEntity();
    }
}