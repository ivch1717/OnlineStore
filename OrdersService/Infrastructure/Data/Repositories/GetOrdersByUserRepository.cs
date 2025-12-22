using Entities;
using Infrastructure.Data.Db;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Mapper;
using UseCases.GetOrdersByUser;

namespace Infrastructure.Data.Repositories;

internal sealed class GetOrdersByUserRepository(OrderServiceDbContext db) : IGetOrdersByUserRepository
{
    public IReadOnlyList<Order> GetOrdersByUser(Guid userId)
    {
        var dtos = db.Orders
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Id)
            .ToList();

        return dtos.Select(x => x.ToEntity()).ToList();
    }
}