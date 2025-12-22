using Entities;

namespace UseCases.GetOrdersByUser;

public interface IGetOrdersByUserRepository
{
    IReadOnlyList<Order> GetOrdersByUser(Guid userId);
}