using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Enums;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Expense_Management_System.Domain.Interfaces.UnitOfWorks;
using System.Linq.Expressions;

namespace Expense_Management_System.Application.Services;

public class ExpenseDocumentService : GenericService<ExpenseDocument>, IExpenseDocumentService
{
    private readonly IExpenseDocumentRepository _expenseDocumentRepository;
    private readonly IUnitOfWork _unitOfWork;
    public ExpenseDocumentService(IExpenseDocumentRepository expenseDocumentRepository, IUnitOfWork unitOfWork) : base(expenseDocumentRepository, unitOfWork)
    {
        _expenseDocumentRepository = expenseDocumentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ExpenseDocument>> GetDocumentsByExpenseIdAsync(Guid expenseId)
          =>  await _expenseDocumentRepository.WhereWithExpenseAsync(e => e.ExpenseId == expenseId);


    public async Task<IEnumerable<ExpenseDocument>> GetDocumentsByUserIdAsync(Guid userId)
          => await _expenseDocumentRepository.WhereWithExpenseAsync(e => e.Expenses.UserId == userId);
 

    public async Task<bool> IsDocumentLinkedToApprovedExpenseAsync(Guid documentId)
    {
        var document = await _expenseDocumentRepository.GetByIdAsync(documentId);
        if (document is null)
            return false;

        var expense = await _expenseDocumentRepository.GetByIdAsync(document.ExpenseId);
        if (expense is null || expense.Expenses.Status != ExpenseStatus.Approved)
            return false;

        return true;
    }

}
