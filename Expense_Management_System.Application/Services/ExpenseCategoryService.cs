using AutoMapper;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Expense_Management_System.Domain.Interfaces.UnitOfWorks;
using System.Linq.Expressions;


namespace Expense_Management_System.Application.Services;

public class ExpenseCategoryService : GenericService<ExpenseCategory>, IExpenseCategoryService
{
    private readonly IExpenseCategoryRepository _expenseCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ExpenseCategoryService(IExpenseCategoryRepository expenseCategoryRepository, IUnitOfWork unitOfWork) : base(expenseCategoryRepository, unitOfWork)
    {
        _expenseCategoryRepository = expenseCategoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CanDeleteCategoryAsync(Guid id)
    {
        return !await _expenseCategoryRepository.HasActiveExpensesAsync(id);
    }

    public async Task<bool> IsCategoryNameExistsAsync(string name, Guid? excludeId = null)
    {
        return await _expenseCategoryRepository.IsCategoryNameUniqueAsync(name, excludeId);
    }
}