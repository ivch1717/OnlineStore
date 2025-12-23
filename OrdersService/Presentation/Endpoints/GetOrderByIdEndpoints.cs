using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UseCases.GetOrderById;

namespace Presentation.Endpoints;

public static class GetOrderByIdEndpoints
{
    public static RouteGroupBuilder MapGetOrderById(this RouteGroupBuilder group)
    {
        group.MapPost("/{id:guid}", (Guid id, IGetOrderByIdRequestHandler handler) =>
        {
            if (id == Guid.Empty)
            {
                return Results.BadRequest(new { error = "Некорректный OrderId" });
            }
            var response = handler.Handle(new GetOrderByIdRequest(id));
            if (response is null)
            {
                return Results.NotFound(new { error = "Заказ не найден" });
            }
            return Results.Ok(response);
        }).WithName("GetOrderById")
        .WithSummary("Get order by id")
        .WithDescription("Возвращает статус заказа")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
        
        return group;
    }
}