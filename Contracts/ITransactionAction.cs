using FinancialAppMvc.Models;

namespace FinancialAppMvc.Contracts
{
    public interface ITransactionAction
    {
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(int id);
        Task StoreAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DestroyAsync(int id);
    }
}