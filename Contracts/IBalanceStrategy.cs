using FinancialAppMvc.Models;

namespace FinancialAppMvc.Contracts
{
    public interface IBalanceStrategy
    {
        Task UpdateBalanceAsync(Balance balance, Transaction transaction);
    }
}