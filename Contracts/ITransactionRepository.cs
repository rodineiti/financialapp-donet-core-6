using FinancialAppMvc.Models;

namespace FinancialAppMvc.Contracts
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(int id);
        Task StoreAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task<bool> DestroyAsync(int id);
    }
}