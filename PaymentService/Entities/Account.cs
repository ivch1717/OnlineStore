namespace Entities;

public sealed class Account
{
    public Guid Id {get;}
    public Guid UserId {get;}
    public int Balance {get;}
    
    public Account(Guid id, Guid userId)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Order Id некорректный");
        }
        
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID некорректный");
        }

        Id = id;
        UserId = userId;
        Balance = 0;
    }
}