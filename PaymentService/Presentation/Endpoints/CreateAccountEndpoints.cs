using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UseCases.CreateAccount;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Endpoints;

public static class CreateAccountEndpoints
{
    public static RouteGroupBuilder MapCreateAccount(this RouteGroupBuilder group)
    {
        group.MapPost("", (CreateAccountRequest request, ICreateAccountRequestHandler handler) =>
            {
                try
                {
                    var accountId = handler.Handle(request);
                    return Results.Created($"/accounts/{accountId}", new { accountId });
                }
                catch (DbUpdateException ex)
                    when (ex.InnerException is not null &&
                          ex.InnerException.Message.Contains("IX_Accounts_UserId"))
                {
                    return Results.Conflict(new
                    {
                        error = "AccountAlreadyExists",
                        userId = request.UserId
                    });
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