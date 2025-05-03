using Expense_Management_System.Application.DTOs.Common;
using Expense_Management_System.Domain.Common;
using Expense_Management_System.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.DTOs.Requests;

public class PaymentRequest: BaseEntity
{
    public Guid ExpenseClaimId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
}
