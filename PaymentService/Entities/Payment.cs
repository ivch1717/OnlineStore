namespace Entities;

public class Payment
{
    public Guid Id {get;}
    public Guid UserId {get;}
    public Guid OrderId {get;}
    public int Amount {get;}
    public DateTime Date {get;}

    public Payment(Guid id, Guid userId, Guid orderId, int amount, DateTime date)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Order Id некорректный");
        }
        
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID некорректный");
        }

        if (orderId == Guid.Empty)
        {
            throw new ArgumentException("Order ID некорректный");
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Цена не может быть отрицательной");
        }
        
        Id = id;
        UserId = userId;
        OrderId = orderId;
        Amount = amount;
        Date = date;
    }
}