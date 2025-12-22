using Entities;
using Infrastructure.Data.Db;
using Infrastructure.Data.Mapper;
using UseCases.CreateOrder;

namespace Infrastructure.Data.Repositories;

internal sealed class CreateOrderRepository(OrderServiceDbContext db) : ICreateOrderRepository
{
    public void Add(Order order)
    {
        db.Orders.Add(order.ToDto());
        db.SaveChanges();
    }
}