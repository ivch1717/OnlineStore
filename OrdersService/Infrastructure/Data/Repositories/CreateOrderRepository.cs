using Entities;
using Infrastructure.Data.Db;
using Infrastructure.Data.Dtos;
using Infrastructure.Data.Mapper;
using Microsoft.EntityFrameworkCore;
using UseCases.CreateOrder;

namespace Infrastructure.Data.Repositories;

internal sealed class CreateOrderRepository(OrderServiceDbContext db) : ICreateOrderRepository
{
    public void Add(Order order)
    {
        using var tx = db.Database.BeginTransaction();
        
        db.Orders.Add(order.ToDto());
        db.SaveChanges();
        
        db.OutboxPaymentRequested.Add(new PaymentRequestedOutboxDto
        {
            MessageId = Guid.NewGuid(),
            OrderId = order.Id,    
            UserId = order.UserId,
            Amount = order.Amount,    
            CreatedAt = DateTime.UtcNow,
            PublishedAt = null
        });

        db.SaveChanges();

        tx.Commit();
    }
}