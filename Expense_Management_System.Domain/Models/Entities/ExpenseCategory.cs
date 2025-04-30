using System.Collections.Generic;
using Expense_Management_System.Domain.Models.Common;

namespace Expense_Management_System.Domain.Models.Entities
{
    public class ExpenseCategory : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public  ICollection<Expense> Expenses { get; set; }
    }
}
