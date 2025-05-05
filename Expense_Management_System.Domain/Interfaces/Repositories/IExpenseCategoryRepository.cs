using Expense_Management_System.Domain.Entities;

namespace Expense_Management_System.Domain.Interfaces.Repositories;

public interface IExpenseCategoryRepository : IGenericRepository<ExpenseCategory>
{
    Task<bool> HasActiveExpensesAsync(Guid categoryId);
    Task<bool> IsCategoryNameUniqueAsync(string categoryName, Guid? excludeId = null); 
}
