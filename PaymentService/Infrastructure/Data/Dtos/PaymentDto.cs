namespace Infrastructure.Data.Dtos;

public sealed class PaymentDto
{
    public Guid PaymentId { get; set; }
    public Guid OrderId { get; set; } 
    public Guid UserId { get; set; }
    public int Amount { get; set; }
    public short Status { get; set; }        // 1 succeeded, 2 failed
    public DateTime CreatedAt { get; set; }
}