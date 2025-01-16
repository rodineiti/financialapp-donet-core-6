using FinancialAppMvc.Models;

namespace FinancialAppMvc.Contracts
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllAsync(string userId);
        Task<Transaction?> GetByIdAsync(int id, string userId);
        Task StoreAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task<bool> DestroyAsync(Transaction transaction);
    }
}