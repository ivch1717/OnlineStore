using Entities;
using Infrastructure.Data.Dtos;

namespace Infrastructure.Data.Mapper;

public static class DataMapper
{
    public static AccountDto ToDto(this Account account)
    {
        return new AccountDto(
            Id: account.Id,
            UserId: account.UserId,
            Balance: account.Balance
        );
    }

    public static Account ToEntity(this AccountDto dto)
    {
        var account = new Account(dto.Id, dto.UserId);
        account.EditBalanse(dto.Balance);
        return account;
    }
}