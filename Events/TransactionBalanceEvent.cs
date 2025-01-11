using FinancialAppMvc.Enums;
using FinancialAppMvc.Models;
using MediatR;

namespace FinancialAppMvc.Events
{
    public class TransactionBalanceEvent : INotification
    {
        public Transaction Transaction { get; set; }
        public EventBalanceType EventBalanceType { get; set; }

        public TransactionBalanceEvent(Transaction transaction, EventBalanceType eventBalanceType)
        {
            Transaction = transaction;
            EventBalanceType = eventBalanceType;
        }
    }
}