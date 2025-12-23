using Entities;

namespace UseCases.CreateAccount;

public static class CreateAccountMapper
{
    public static Account ToEntity(CreateAccountRequest request, Guid accountId)
    {
        return new Account(accountId, request.UserId);
    }

    public static CreateAccountResponse ToResponse(Account account)
    {
        return new CreateAccountResponse(account.Id);
    }
}