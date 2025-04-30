using System;
using Expense_Management_System.Domain.Models.Common;
using Expense_Management_System.Domain.Models.Enums;

namespace Expense_Management_System.Domain.Models.Entities
{
    public class Payment : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ExpenseId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod Method { get; set; }
        public string TransactionReference { get; set; }
        
        public  User User { get; set; }
        public  Expense Expense { get; set; }
    }
}
