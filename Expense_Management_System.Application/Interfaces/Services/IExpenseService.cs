using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.Interfaces.Services;
public interface IExpenseService : IGenericService<Expense>
{
    // Personel işlemleri
    Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(Guid userId);
    Task<IEnumerable<Expense>> GetFilteredExpensesForUserAsync(
        Guid userId,
        ExpenseStatus? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        decimal? minAmount = null,
        decimal? maxAmount = null);
    Task<IEnumerable<Expense>> GetActiveExpensesByUserAsync(Guid userId);
    Task<IEnumerable<Expense>> GetExpensesHistoryByUserAsync(Guid userId);
    Task<IEnumerable<Expense>> GetRejectedExpensesWithReasonAsync(Guid userId);
    //Task<bool> TriggerPaymentSimulationAsync(Guid expenseId, Guid userId);  // Onaylanan talepler için ödeme simülasyonu başlatır.


    // Admin işlemleri
    Task<IEnumerable<Expense>> GetAllPendingExpensesAsync();
    Task<IEnumerable<Expense>> GetFilteredExpensesAsync(
        Guid? userId = null,
        ExpenseStatus? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        decimal? minAmount = null,
        decimal? maxAmount = null);
    Task<Expense> ApproveExpenseAsync(Guid expenseId, Guid adminId);
    Task<Expense> RejectExpenseAsync(Guid expenseId, Guid adminId, string rejectionReason);
}