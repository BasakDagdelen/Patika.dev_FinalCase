using Expense_Management_System.Domain.Entities;

namespace Expense_Management_System.Domain.Interfaces.Repositories;

public interface IExpenseDocumentRepository : IGenericRepository<ExpenseDocument>
{
    Task<IEnumerable<ExpenseDocument>> GetDocumentsByExpenseIdAsync(Guid expenseId);
    Task<bool> IsDocumentLinkedToApprovedExpenseAsync(Guid documentId);
    Task<IEnumerable<ExpenseDocument>> GetDocumentsByUserIdAsync(Guid userId);
}
