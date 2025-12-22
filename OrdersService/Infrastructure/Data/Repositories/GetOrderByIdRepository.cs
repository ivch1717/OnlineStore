using Entities;
using Infrastructure.Data.Db;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Mapper;
using UseCases.GetOrderById;

namespace Infrastructure.Data.Repositories;

internal sealed class GetOrderByIdRepository(OrderServiceDbContext db) : IGetOrderByIdRepository
{
    public Order GetOrderById(Guid orderId)
    {
        var dto = db.Orders
            .AsNoTracking()
            .SingleOrDefault(x => x.Id == orderId);

        if (dto is null)
        {
            throw new InvalidOperationException($"Order '{orderId}' not found.");
        }

        return dto.ToEntity();
    }
}