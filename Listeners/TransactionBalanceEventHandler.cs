using FinancialAppMvc.Events;
using FinancialAppMvc.Factories;
using FinancialAppMvc.Models;
using FinancialAppMvc.Repositories;
using MediatR;

namespace FinancialAppMvc.Listeners
{
    public class TransactionBalanceEventHandler : INotificationHandler<TransactionBalanceEvent>
    {
        private readonly UserRepository _userRepository;
        private readonly AuditLogRepository _auditLog;
        private readonly BalanceStrategyFactory _balanceStrategyFactory;
        private readonly ILogger<TransactionBalanceEventHandler> _logger;

        public TransactionBalanceEventHandler(UserRepository userRepository, AuditLogRepository auditLog, BalanceStrategyFactory balanceStrategyFactory, ILogger<TransactionBalanceEventHandler> logger)
        {
            _userRepository = userRepository;
            _auditLog = auditLog;
            _balanceStrategyFactory = balanceStrategyFactory;
            _logger = logger;
        }

        public async Task Handle(TransactionBalanceEvent notification, CancellationToken cancellationToken)
        {
            var balance = await _userRepository.GetBalanceUserAsync(notification.Transaction.UserId);

            if (balance == null)
            {
                balance = new Balance { UserId = notification.Transaction.UserId, CurrentBalance = 0 };
                await _userRepository.AddBalanceUserAsync(balance);
            }

            _logger.LogInformation("Event balance type: {EventBalanceType}, Transaction type: {TransactionType}", notification.EventBalanceType, notification.Transaction.Type);

            var previousBalance = balance.CurrentBalance;

            var balanceStrategy = _balanceStrategyFactory
                .CreateBalanceStrategy(notification.EventBalanceType, notification.Transaction.Type);

            await balanceStrategy.UpdateBalanceAsync(balance, notification.Transaction);

            await _auditLog.AddAuditLogAsync(new AuditLog
            {
                UserId = notification.Transaction.UserId,
                PreviousBalance = previousBalance,
                AmountChanged = notification.Transaction.Amount,
                NewBalance = balance.CurrentBalance,
                TransactionId = notification.Transaction.Id
            });
        }
    }
}