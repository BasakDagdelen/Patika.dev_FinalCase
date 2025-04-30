using Expense_Management_System.Application.DTOs.Common;
using Expense_Management_System.Application.DTOs.Requests;
using Expense_Management_System.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.DTOs.Responses;

public class ExpenseResponse: BaseResponse
{
    public Guid UserId { get; set; }
    public string UserFullName { get; set; }

    public Guid ExpenseCategoryId { get; set; }
    public string ExpenseCategoryName { get; set; }

    public decimal Amount { get; set; }
    public string PaymentLocation { get; set; }
    public string? Description { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public ExpenseStatus Status { get; set; }

    public DateTime? ApprovedAt { get; set; }
    public string? RejectionReason { get; set; }

    public Guid? ApprovedByUserId { get; set; }
    public string? ApprovedByUserFullName { get; set; }

    public List<ExpenseDocumentRequest>? DocumentUrls { get; set; }

}
