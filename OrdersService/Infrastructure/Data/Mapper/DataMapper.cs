using Entities;
using Infrastructure.Data.Dtos;

namespace Infrastructure.Data.Mapper;

public static class DataMapper
{
    public static OrderDto ToDto(this Order entity)
    {
        return new OrderDto(
            Id: entity.Id,
            UserId: entity.UserId,
            Amount: entity.Amount,
            Description: entity.Description,
            OrderStatus: (int)entity.Status
        );
    }

    public static Order ToEntity(this OrderDto dto)
    {
        var order = new Order(
            id: dto.Id,
            userId: dto.UserId,
            amount: dto.Amount,
            description: dto.Description
        );
        if (dto.OrderStatus == 1)
        {
            order.MarkFinished();
        }

        if (dto.OrderStatus == 2)
        {
            order.MarkCanceled();
        }
        return order;
    }

}