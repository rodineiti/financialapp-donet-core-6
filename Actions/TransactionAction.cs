using FinancialAppMvc.Contracts;
using FinancialAppMvc.Enums;
using FinancialAppMvc.Models;

namespace FinancialAppMvc.Actions
{
    public class TransactionAction : ITransactionAction
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionAction(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _transactionRepository.GetAllAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _transactionRepository.GetByIdAsync(id);
        }

        public async Task StoreAsync(Transaction transaction)
        {
            await _transactionRepository.StoreAsync(transaction);
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            await _transactionRepository.UpdateAsync(transaction);
        }

        public async Task DestroyAsync(int id)
        {
            await _transactionRepository.DestroyAsync(id);
        }
    }
}