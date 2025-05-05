using Expense_Management_System.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.DTOs.Requests;

public class ExpenseFilterRequest
{
    public ExpenseStatus? Status { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "The minimum amount cannot be negative.")]
    public decimal? MinAmount { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "The maximum amount cannot be negative.")]
    public decimal? MaxAmount { get; set; }

    public Guid? UserId { get; set; } 
}