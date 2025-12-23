namespace Infrastructure.Data.Dtos;

public record AccountDto(
    Guid Id,
    Guid UserId,
    int Balance
    );