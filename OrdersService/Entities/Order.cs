namespace Entities;

public sealed class Order
{
    public Guid Id {get;}
    public Guid UserId {get;}
    public int Amount {get;}
    public string Description {get;}
    public OrderStatus Status {get; private set; }

    public Order(Guid userId, int amount, string description)
    {
        if (Id == Guid.Empty)
        {
            throw new ArgumentException("Order Id некорректный");
        }
        
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID некорректный");
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Количество должно быть положительным числом");
        }
        Id = Guid.NewGuid();
        UserId = userId;
        Amount = amount;
        Description = description;
        Status = OrderStatus.New;
    }
    
    public void MarkFinished() => Status = OrderStatus.Finished;
    public void MarkCanceled() => Status = OrderStatus.Canceled;
}