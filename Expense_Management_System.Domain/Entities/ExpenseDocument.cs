using Expense_Management_System.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Domain.Entities;

public class ExpenseDocument : BaseEntity
{
    public Guid ExpenseId { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public DateTime UploadDate { get; set; } = DateTime.Now;

    public Expense Expenses { get; set; }
}
