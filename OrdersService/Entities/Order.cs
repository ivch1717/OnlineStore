namespace OrdersService.Entities;

public sealed class Order
{
    public Guid Id {get;}
    public Guid UserId {get;}
    public int Amount {get;}
    public string Description {get;}
    public OrderStatus Status {get;}

    public Order(Guid id, Guid userId, int amount, string description, OrderStatus status)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID некорректный");
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Количество должно быть положительным числом");
        }
        Id = id;
        UserId = userId;
        Amount = amount;
        Description = description;
        Status = status;
    }
}