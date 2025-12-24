using Infrastructure.Data.Dtos;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data.Db;

public sealed class OrderServiceDbContext(DbContextOptions<OrderServiceDbContext> options) : DbContext(options)
{
    public DbSet<OrderDto> Orders => Set<OrderDto>();
    public DbSet<PaymentRequestedOutboxDto> OutboxPaymentRequested => Set<PaymentRequestedOutboxDto>();
    public DbSet<OrdersInboxMessageDto> InboxMessages => Set<OrdersInboxMessageDto>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderDto>(builder =>
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.OrderStatus).IsRequired();

            builder.HasIndex(x => x.UserId);
            
            modelBuilder.ConfigureOrdersMessaging();
        });
    }
}