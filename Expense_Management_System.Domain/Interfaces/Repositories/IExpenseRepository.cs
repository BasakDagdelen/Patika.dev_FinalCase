using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Enums;

namespace Expense_Management_System.Domain.Interfaces.Repositories;

public interface IExpenseRepository:IGenericRepository<Expense>
{
    Task<IEnumerable<Expense>> GetActiveExpenseSAsync(Guid userId);
    Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(Guid categoryId);
    Task<IEnumerable<Expense>> GetExpenseHistoryAsync(Guid userId);
    Task<IEnumerable<Expense>> GetExpensesByStatusAsync(ExpenseStatus status);
    Task<Expense> GetExpenseWithUserAndBankAccountAsync(Guid expenseId);

    //Task<IEnumerable<Expense>> FilterExpensesAsync(
    //Guid? userId = null,
    //Guid? categoryId = null,
    //ExpenseStatus? status = null,
    //DateTime? fromDate = null,
    //DateTime? toDate = null
    //);

}
