namespace Infrastructure.Data.Dtos;

public sealed class PaymentResultOutboxDto
{
    public Guid MessageId { get; set; } 
    public Guid OrderId { get; set; }
    public short Status { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
}