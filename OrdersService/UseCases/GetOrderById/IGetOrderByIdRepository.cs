using Entities;

namespace UseCases.GetOrderById;

public interface IGetOrderByIdRepository
{
    Order GetOrderById(Guid orderId);
}