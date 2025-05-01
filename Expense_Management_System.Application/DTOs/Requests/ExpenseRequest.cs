using Expense_Management_System.Application.DTOs.Common;
using Expense_Management_System.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.DTOs.entitys;

public class Expenseentity: Baseentity
{
    public Guid UserId { get; set; }
    public Guid ExpenseCategoryId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentLocation { get; set; }
    public string? Description { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public List<ExpenseDocumenTEntity> Documents { get; set; } = new();
}
