namespace Infrastructure.Data.Dtos;

public sealed class OrdersInboxMessageDto
{
    public Guid MessageId { get; set; }  
    public DateTime ProcessedAt { get; set; }
}