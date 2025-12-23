using Infrastructure.Data.Db;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UseCases.CreateAccount;
using UseCases.GetBalance;
using UseCases.TopUpAccount;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPaymentsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not found.");

        services.AddDbContext<PaymentServiceDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICreateAccountRepository, CreateAccountRepository>();
        services.AddScoped<IGetBalanceRepository, GetBalanceRepository>();
        services.AddScoped<ITopUpAccountRepository, TopUpAccountRepository>();

        return services;
    }
}