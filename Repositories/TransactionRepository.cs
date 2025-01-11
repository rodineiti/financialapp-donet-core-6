using FinancialAppMvc.Contracts;
using FinancialAppMvc.Data;
using Microsoft.EntityFrameworkCore;
using FinancialAppMvc.Models;
using MediatR;
using FinancialAppMvc.Events;
using FinancialAppMvc.Enums;

namespace FinancialAppMvc.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserContextService _userContextService;
        private readonly UserRepository _userRepository;
        private readonly IMediator _mediator;


        public TransactionRepository(AppDbContext context, IUserContextService userContextService, UserRepository userRepository, IMediator mediator)
        {
            _context = context;
            _userContextService = userContextService;
            _userRepository = userRepository;
            _mediator = mediator;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            var userId = _userContextService.GetUserId();

            return await _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            var userId = _userContextService.GetUserId();
            
            return await _context.Transactions
                .Where(t => t.UserId == userId && t.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task StoreAsync(Transaction transaction)
        {
            var userId = _userContextService.GetUserId();

            transaction.UserId = userId;

            _context.Transactions.Add(transaction);

            await _mediator.Publish(new TransactionBalanceEvent(transaction, EventBalanceType.Create));

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            var userId = _userContextService.GetUserId();

            transaction.UserId = userId;

            _context.Transactions.Update(transaction);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DestroyAsync(int id)
        {
            var userId = _userContextService.GetUserId();

            var transaction = await GetByIdAsync(id);

            if (transaction != null)
            {
                await _mediator.Publish(new TransactionBalanceEvent(transaction, EventBalanceType.Delete));

                _context.Transactions.Remove(transaction);
                
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}