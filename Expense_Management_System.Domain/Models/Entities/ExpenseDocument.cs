using Expense_Management_System.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Domain.Models.Entities;

public class ExpenseDocument: BaseEntity
{
    public Guid ExpenseId { get; set; }
    public Expense Expenses { get; set; } 

    public string FileName { get; set; } 
    public string FileUrl { get; set; } 
}
