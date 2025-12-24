using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Presentation.Endpoints;
namespace Presentation;

public static class PaymentServiceEndpoint
{
    public static WebApplication MapAccountsEndpoints(this WebApplication app)
    {
        app.MapGroup("/accounts")
            .WithTags("Accounts")
            .MapCreateAccount()
            .MapTopUpAccount()
            .MapGetBalance();
        return app;
    }
}