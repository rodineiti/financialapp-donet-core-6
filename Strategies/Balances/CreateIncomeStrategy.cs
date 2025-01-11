using FinancialAppMvc.Contracts;
using FinancialAppMvc.Models;

namespace FinancialAppMvc.Strategies.Balances
{
    public class CreateIncomeStrategy : IBalanceStrategy
    {
        public Task UpdateBalanceAsync(Balance balance, Transaction transaction)
        {
            balance.CurrentBalance += transaction.Amount;

            return Task.CompletedTask;
        }
    }
}