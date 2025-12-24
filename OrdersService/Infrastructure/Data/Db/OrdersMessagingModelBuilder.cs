using Infrastructure.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Db;

internal static class OrdersMessagingModelBuilder
{
    public static void ConfigureOrdersMessaging(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaymentRequestedOutboxDto>(b =>
        {
            b.ToTable("OutboxPaymentRequested");
            b.HasKey(x => x.MessageId);
            b.Property(x => x.OrderId).IsRequired();
            b.Property(x => x.UserId).IsRequired();
            b.Property(x => x.Amount).IsRequired();
            b.Property(x => x.CreatedAt).IsRequired();
            b.Property(x => x.PublishedAt);
            b.HasIndex(x => x.PublishedAt);
        });

        modelBuilder.Entity<OrdersInboxMessageDto>(b =>
        {
            b.ToTable("InboxMessages");
            b.HasKey(x => x.MessageId);
            b.Property(x => x.ProcessedAt).IsRequired();
        });
    }
}