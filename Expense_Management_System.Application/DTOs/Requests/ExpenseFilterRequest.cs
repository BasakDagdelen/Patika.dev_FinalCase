using Expense_Management_System.Domain.Enums;

namespace Expense_Management_System.Application.DTOs.Requests;

public class ExpenseFilterRequest
{
    public ExpenseStatus? Status { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }

    public Guid? UserId { get; set; }
}