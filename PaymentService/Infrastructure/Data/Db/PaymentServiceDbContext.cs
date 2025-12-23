using Infrastructure.Data.Dtos;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data.Db;

internal sealed class PaymentServiceDbContext(DbContextOptions<PaymentServiceDbContext> options) : DbContext(options)
{
    public DbSet<AccountDto> Accounts => Set<AccountDto>();

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
    }
}