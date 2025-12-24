namespace UseCases.TopUpAccount;

public sealed record TopUpAccountRequest(
    Guid UserId,
    int Amount
    );