using Expense_Management_System.Application.DTOs.Common;
using Expense_Management_System.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.DTOs.Responses;

public class PaymentResponse: BaseResponse
{
    public Guid UserId { get; set; }
    public string UserFullName { get; set; }
    public Guid ExpenseClaimId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMethod Method { get; set; }
    public string TransactionReference { get; set; }
    public ExpenseStatus ExpenseStatus { get; set; }
}
