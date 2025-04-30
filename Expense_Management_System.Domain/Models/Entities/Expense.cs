using Expense_Management_System.Domain.Models.Common;
using Expense_Management_System.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Domain.Models.Entities;

public class Expense : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid ExpenseCategoryId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentLocation { get; set; }
    public string? Description { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.EFT;
    public ExpenseStatus Status { get; set; } = ExpenseStatus.Pending;
    public DateTime? ApprovedAt { get; set; }
    public string? RejectionReason { get; set; }

    public Guid? ApprovedByUserId { get; set; }
    public User? ApprovedByUser { get; set; }

    public User User { get; set; }
    public ExpenseCategory ExpenseCategory { get; set; }
    public ICollection<ExpenseDocument> ExpenseDocuments { get; set; }
    public ICollection<Payment> Payments { get; set; }

}
