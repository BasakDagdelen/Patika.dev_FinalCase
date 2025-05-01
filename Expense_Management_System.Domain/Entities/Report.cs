using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Domain.Entities;

public class ExpenseReport
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalExpenses { get; set; }
    public int ApprovedCount { get; set; }
    public int RejectedCount { get; set; }
    public int PendingCount { get; set; }
    public List<ExpenseReportItem> Items { get; set; }
}

public class ExpenseReportItem
{
    public string CategoryName { get; set; }
    public decimal Amount { get; set; }
    public int Count { get; set; }
}

public class EmployeeExpenseReport
{
    public int UserId { get; set; }
    public string UserFullName { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalExpenses { get; set; }
    public List<ExpenseReportItem> CategoryBreakdown { get; set; }
}