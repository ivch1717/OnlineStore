using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Presentation.Endpoints;
namespace Presentation;

public static class OrdersEndpoints
{
    public static WebApplication MapOrdersEndpoints(this WebApplication app)
    {
        app.MapGroup("/orders")
            .WithTags("Orders")
            .MapCreateAccount()
            .MapTopUpAccount()
            .MapGetBalance();
        return app;
    }
}