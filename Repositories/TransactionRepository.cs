using FinancialAppMvc.Contracts;
using FinancialAppMvc.Data;
using Microsoft.EntityFrameworkCore;
using FinancialAppMvc.Models;

namespace FinancialAppMvc.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;
        
        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync(string userId)
        {
            try 
            {
                return await _context.Transactions
                    .Where(t => t.UserId == userId)
                    .OrderByDescending(t => t.Date)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Transaction?> GetByIdAsync(int id, string userId)
        {
            try 
            {
                return await _context.Transactions
                    .Where(t => t.UserId == userId && t.Id == id)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task StoreAsync(Transaction transaction)
        {
            try 
            {
                _context.Transactions.Add(transaction);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            try 
            {
                _context.Transactions.Update(transaction);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DestroyAsync(Transaction transaction)
        {
            try 
            {
                _context.Transactions.Remove(transaction);
            
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}