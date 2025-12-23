namespace UseCases.GetBalance;

public interface IGetBalanceRequestHandler
{
    GetBalanceResponse Handle(GetBalanceRequest request);
}