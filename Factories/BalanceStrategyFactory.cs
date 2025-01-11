using FinancialAppMvc.Contracts;
using FinancialAppMvc.Enums;
using FinancialAppMvc.Strategies.Balances;

namespace FinancialAppMvc.Factories
{
    public class BalanceStrategyFactory
    {
      public IBalanceStrategy CreateBalanceStrategy(EventBalanceType eventBalanceType, TransactionType transactionType)
      {
          return (eventBalanceType, transactionType) switch
          {
              (EventBalanceType.Create, TransactionType.Income) => new CreateIncomeStrategy(),
              (EventBalanceType.Create, TransactionType.Expense) => new CreateExpenseStrategy(),
              (EventBalanceType.Delete, TransactionType.Income) => new DeleteIncomeStrategy(),
              (EventBalanceType.Delete, TransactionType.Expense) => new DeleteExpenseStrategy(),
              _ => throw new NotImplementedException("Strategy not implemented for the given types.")
          };
      }
    }
}