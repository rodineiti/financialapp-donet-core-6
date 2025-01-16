using FinancialAppMvc.Data;
using FinancialAppMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialAppMvc.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetBalanceAsync(string userId)
        {
            var balance = await _context.Balances
                .Where(b => b.UserId == userId)
                .FirstOrDefaultAsync();

            return balance?.CurrentBalance ?? 0;
        }

        public async Task<Balance?> GetBalanceUserAsync(string userId)
        {
            return await _context.Balances
                .Where(b => b.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task AddBalanceUserAsync(Balance balance)
        {
            _context.Balances.Add(balance);
            
            await _context.SaveChangesAsync();
        }
    }
}