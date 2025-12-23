using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UseCases.CreateAccount;

namespace Presentation.Endpoints;

public static class CreateAccountEndpoints
{
    public static RouteGroupBuilder MapCreateAccount(this RouteGroupBuilder group)
    {
        group.MapPost("", (CreateAccountRequest request, ICreateAccountRequestHandler handler) =>
            {
                try
                {
                    var response = handler.Handle(request);
                    return Results.Created($"/accounts/{response.AccountId}", response);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    return Results.BadRequest(new { error = ex.Message, param = ex.ParamName });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Conflict(new { error = ex.Message });
                }
            })
            .WithName("CreateAccount")
            .WithSummary("Create account")
            .WithDescription("Создание счета пользователя (не более одного счета на пользователя)")
            .WithOpenApi()
            .Produces<CreateAccountResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);

        return group;
    }
}