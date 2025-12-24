using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UseCases.GetBalance;

namespace Presentation.Endpoints;

public static class GetBalanceEndpoints
{
    public static RouteGroupBuilder MapGetBalance(this RouteGroupBuilder group)
    {
        group.MapGet("/{userId:guid}/balance", (Guid userId, IGetBalanceRequestHandler handler) =>
            {
                if (userId == Guid.Empty)
                    return Results.BadRequest(new { error = "Некорректный UserId" });

                try
                {
                    var response = handler.Handle(new GetBalanceRequest(userId));
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
                catch (InvalidOperationException)
                {
                    return Results.NotFound(new { error = "Счет не найден" });
                }
            })
            .WithName("GetBalance")
            .WithSummary("Get balance")
            .WithDescription("Просмотр баланса счета")
            .WithOpenApi()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return group;
    }
}