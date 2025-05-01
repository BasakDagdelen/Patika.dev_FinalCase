using System;
using Expense_Management_System.Domain.Common;
using Expense_Management_System.Domain.Enums;

namespace Expense_Management_System.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ExpenseId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod Method { get; set; }
        public string TransactionReference { get; set; }

        public User User { get; set; }
        public Expense Expense { get; set; }
    }
}
