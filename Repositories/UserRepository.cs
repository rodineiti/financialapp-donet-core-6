using FinancialAppMvc.Contracts;
using FinancialAppMvc.Data;
using FinancialAppMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialAppMvc.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserContextService _userContextService;

        public UserRepository(AppDbContext context, IUserContextService userContextService)
        {
            _context = context;
            _userContextService = userContextService;
        }

        public async Task<decimal> GetBalanceAsync()
        {
            var userId = _userContextService.GetUserId();

            var balance = await _context.Balances
                .Where(b => b.UserId == userId)
                .FirstOrDefaultAsync();

            return balance?.CurrentBalance ?? 0;
        }

        public async Task<Balance?> GetBalanceUserAsync()
        {
            var userId = _userContextService.GetUserId();

            return await _context.Balances
                .Where(b => b.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task AddBalanceUserAsync(Balance balance)
        {
            balance.UserId = _userContextService.GetUserId();

            _context.Balances.Add(balance);
            
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBalanceUserAsync(Balance balance)
        {
            balance.UserId = _userContextService.GetUserId();
            
            _context.Balances.Update(balance);
            
            await _context.SaveChangesAsync();
        }
    }
}