using Infrastructure.Data.Dtos;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data.Db;

internal sealed class OrderServiceDbContext(DbContextOptions<OrderServiceDbContext> options) : DbContext(options)
{
    public DbSet<OrderDto> Orders => Set<OrderDto>();

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
        });
    }
}