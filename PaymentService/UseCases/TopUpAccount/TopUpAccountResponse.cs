namespace UseCases.TopUpAccount;

public record TopUpAccountResponse(
    Guid AccountId,
    int Balance
    );