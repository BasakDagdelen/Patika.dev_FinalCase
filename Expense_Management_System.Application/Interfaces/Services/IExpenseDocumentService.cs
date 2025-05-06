using Expense_Management_System.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.Interfaces.Services;

public interface IExpenseDocumentService : IGenericService<ExpenseDocument>
{
    Task<IEnumerable<ExpenseDocument>> GetDocumentsByExpenseIdAsync(Guid expenseId);
    Task<IEnumerable<ExpenseDocument>> GetDocumentsByUserIdAsync(Guid userId);
    Task<bool> IsDocumentLinkedToApprovedExpenseAsync(Guid documentId);

}