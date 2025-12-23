using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UseCases.TopUpAccount;

namespace Presentation.Endpoints;

public static class TopUpAccountEndpoints
{
    public static RouteGroupBuilder MapTopUpAccount(this RouteGroupBuilder group)
    {
        group.MapPost("/{accountId:guid}/top-up", (Guid accountId, TopUpAccountRequest request, ITopUpAccountRequestHandler handler) =>
            {
                if (accountId == Guid.Empty)
                    return Results.BadRequest(new { error = "Некорректный AccountId" });

                try
                {
                    var response = handler.Handle(request with { AccountId = accountId });

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
            .WithName("TopUpAccount")
            .WithSummary("Top up account")
            .WithDescription("Пополнение баланса счета (синхронно)")
            .WithOpenApi()
            .Produces<TopUpAccountResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return group;
    }
}