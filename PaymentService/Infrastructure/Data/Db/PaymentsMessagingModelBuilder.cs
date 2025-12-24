using Infrastructure.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Db;

internal static class PaymentsMessagingModelBuilder
{
    public static void ConfigurePaymentsMessaging(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InboxMessageDto>(b =>
        {
            b.ToTable("InboxMessages");
            b.HasKey(x => x.MessageId);
            b.Property(x => x.ProcessedAt).IsRequired();
        });

        modelBuilder.Entity<PaymentDto>(b =>
        {
            b.ToTable("Payments");
            b.HasKey(x => x.PaymentId);

            b.Property(x => x.OrderId).IsRequired();
            b.HasIndex(x => x.OrderId).IsUnique();

            b.Property(x => x.UserId).IsRequired();
            b.Property(x => x.Amount).IsRequired();
            b.Property(x => x.Status).IsRequired();
            b.Property(x => x.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<PaymentResultOutboxDto>(b =>
        {
            b.ToTable("OutboxPaymentResults");
            b.HasKey(x => x.MessageId);

            b.Property(x => x.OrderId).IsRequired();
            b.Property(x => x.Status).IsRequired();
            b.Property(x => x.CreatedAt).IsRequired();
            b.Property(x => x.PublishedAt);
            b.HasIndex(x => x.PublishedAt);
        });
    }
}