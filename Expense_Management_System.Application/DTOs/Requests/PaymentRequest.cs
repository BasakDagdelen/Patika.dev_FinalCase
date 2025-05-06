using Expense_Management_System.Application.DTOs.Common;
using Expense_Management_System.Domain.Common;
using Expense_Management_System.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.DTOs.Requests;

public class PaymentRequest: BaseRequest
{      
    public Guid ExpenseId { get; set; }       
    public string BankAccountNumber { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}
