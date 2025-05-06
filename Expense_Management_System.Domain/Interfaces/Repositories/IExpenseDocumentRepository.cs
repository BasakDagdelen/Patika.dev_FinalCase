using Expense_Management_System.Domain.Entities;
using System.Linq.Expressions;

namespace Expense_Management_System.Domain.Interfaces.Repositories;

public interface IExpenseDocumentRepository : IGenericRepository<ExpenseDocument>
{
    Task<IEnumerable<ExpenseDocument>> GetDocumentsByExpenseIdAsync(Guid expenseId);
    Task<bool> IsDocumentLinkedToApprovedExpenseAsync(Guid documentId);
    Task<IEnumerable<ExpenseDocument>> GetDocumentsByUserIdAsync(Guid userId);
    Task<IEnumerable<ExpenseDocument>> WhereWithExpenseAsync(Expression<Func<ExpenseDocument, bool>> expression);
}
