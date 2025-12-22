namespace Infrastructure.Data.Dtos;

public record OrderDto(
    Guid Id,
    Guid UserId,
    int Amount,
    string Description,
    int OrderStatus
    );