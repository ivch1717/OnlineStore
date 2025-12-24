using Infrastructure.Data.Dtos;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data.Db;

public sealed class PaymentServiceDbContext(DbContextOptions<PaymentServiceDbContext> options) : DbContext(options)
{
    public DbSet<AccountDto> Accounts => Set<AccountDto>();
    public DbSet<InboxMessageDto> InboxMessages => Set<InboxMessageDto>();
    public DbSet<PaymentDto> Payments => Set<PaymentDto>();
    public DbSet<PaymentResultOutboxDto> OutboxPaymentResults => Set<PaymentResultOutboxDto>();
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountDto>(builder =>
        {
            builder.ToTable("Accounts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.HasIndex(x => x.UserId).IsUnique();
            
            builder.Property(x => x.Balance).IsRequired();
        });
        
        modelBuilder.ConfigurePaymentsMessaging();
    }
}