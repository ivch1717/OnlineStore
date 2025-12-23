namespace UseCases.TopUpAccount;

public sealed record TopUpAccountRequest(
    Guid AccountId,
    int Amount
    );