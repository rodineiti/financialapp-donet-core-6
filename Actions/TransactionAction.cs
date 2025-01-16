using FinancialAppMvc.Contracts;
using FinancialAppMvc.Enums;
using FinancialAppMvc.Events;
using FinancialAppMvc.Models;
using MediatR;

namespace FinancialAppMvc.Actions
{
    public class TransactionAction : BaseAction, ITransactionAction
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMediator _mediator;

        public TransactionAction(ITransactionRepository transactionRepository, IMediator mediator, IUserContextService userContextService) : base(userContextService)
        {
            _transactionRepository = transactionRepository;
            _mediator = mediator;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            try
            {
                return await _transactionRepository.GetAllAsync(UserId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            try
            {
                return await _transactionRepository.GetByIdAsync(id, UserId);
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
                transaction.UserId = UserId;

                await _transactionRepository.StoreAsync(transaction);

                await _mediator.Publish(new TransactionBalanceEvent(transaction, EventBalanceType.Create));
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
                transaction.UserId = UserId;

                await _transactionRepository.UpdateAsync(transaction);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DestroyAsync(int id)
        {
            try
            {
                var transaction = await _transactionRepository.GetByIdAsync(id, UserId);

                await _transactionRepository.DestroyAsync(transaction);

                await _mediator.Publish(new TransactionBalanceEvent(transaction, EventBalanceType.Delete));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}