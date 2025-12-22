using Entities;
namespace UseCases.CreateOrder;

public interface ICreateOrderRepository
{
    void Add(Order order);
}