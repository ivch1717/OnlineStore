namespace Infrastructure.Data.Dtos;

public sealed class InboxMessageDto
{
    public Guid MessageId { get; set; }
    public DateTime ProcessedAt { get; set; }
}