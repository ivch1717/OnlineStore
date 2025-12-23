using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UseCases.GetOrdersByUser;

namespace Presentation.Endpoints;

public static class GetOrdersByUserEndpoints
{
    public static RouteGroupBuilder MapGetOrdersByUser(this RouteGroupBuilder group)
    {
        group.MapGet("/{userId:guid}/orders", (Guid userId, IGetOrdersByUserRequestHandler handler) =>
        {
            if (userId == Guid.Empty)
                return Results.BadRequest(new { error = "Некорректный UserId" });

            try
            {
                var response = handler.Handle(new GetOrdersByUserRequest(userId));
                return Results.Ok(response);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return Results.BadRequest(new { error = ex.Message, param = ex.ParamName });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
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