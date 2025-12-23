namespace UseCases.GetBalance;

public sealed record GetBalanceResponse(
    Guid AccountId,
    int Balance
    );