using Entities;
using Infrastructure.Data.Db;
using Infrastructure.Data.Mapper;
using Microsoft.EntityFrameworkCore;
using UseCases.GetBalance;

namespace Infrastructure.Data.Repositories;

internal sealed class GetBalanceRepository(PaymentServiceDbContext db) : IGetBalanceRepository
{
    public Account GetBalance(Guid accountId)
    {
        var dto = db.Accounts
            .AsNoTracking()
            .SingleOrDefault(x => x.Id == accountId);

        if (dto is null)
        {
            throw new InvalidOperationException($"Account '{accountId}' not found.");
        }

        return dto.ToEntity();
    }
}