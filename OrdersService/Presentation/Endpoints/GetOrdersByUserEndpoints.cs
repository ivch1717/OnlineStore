using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UseCases.GetOrdersByUser;

namespace Presentation.Endpoints;

public static class GetOrdersByUserEndpoints
{
    public static RouteGroupBuilder MapGetOrdersByUser(this RouteGroupBuilder group)
    {
        group.MapPost("/{userId:guid}/orders", (Guid userId, IGetOrdersByUserRequestHandler handler) =>
        {
            if (userId == Guid.Empty)
            {
                return Results.BadRequest(new { error = "Некорректный UserId" });
            }
            var response = handler.Handle(new GetOrdersByUserRequest(userId));
            return Results.Ok(response);
        })
        .WithName("GetOrdersByUser")
        .WithSummary("Get orders by user id")
        .WithDescription("Возвращает список заказов пользователя")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
        return group;
    }
}