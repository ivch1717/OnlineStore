namespace Infrastructure.Data.Dtos;

public sealed class PaymentRequestedOutboxDto
{
    public Guid MessageId { get; set; } 
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public int Amount { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
}