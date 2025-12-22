using Infrastructure.Data.Db;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UseCases.CreateOrder;
using UseCases.GetOrderById;
using UseCases.GetOrdersByUser;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrdersInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not found.");

        services.AddDbContext<OrderServiceDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddScoped<ICreateOrderRepository, CreateOrderRepository>();
        services.AddScoped<IGetOrderByIdRepository, GetOrderByIdRepository>();
        services.AddScoped<IGetOrdersByUserRepository, GetOrdersByUserRepository>();

        return services;
    }
}