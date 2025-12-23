using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UseCases.CreateOrder;

namespace Presentation.Endpoints;

public static class CreateOrderEndpoints
{
    public static RouteGroupBuilder MapCreateOrder(this RouteGroupBuilder group)
    {
        group.MapPost("", (CreateOrderRequest request, ICreateOrderRequestHandler handler) =>
            {
                var response = handler.Handle(request);
                
                return Results.Created($"/orders/{response.OrderId}", response);
            })
            .WithName("CreateOrder")
            .WithSummary("Create order")
            .WithDescription("Создание заказа и старт операции оплаты")
            .WithOpenApi()
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        return group;
    }
}