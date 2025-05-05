using AutoMapper;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Enums;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Expense_Management_System.Domain.Interfaces.UnitOfWorks;
using System.Linq.Expressions;


namespace Expense_Management_System.Application.Services;

public class ExpenseService : GenericService<Expense>, IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentService _paymentService;

    public ExpenseService(
        IExpenseRepository expenseRepository,
        IUnitOfWork unitOfWork,
        IPaymentService paymentService) : base(expenseRepository, unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
        _paymentService = paymentService;
    }

    public async Task<Expense> ApproveExpenseAsync(Guid expenseId, Guid adminId)
    {
        var expense = await _expenseRepository.GetByIdAsync(expenseId);
        if (expense is null)
            throw new Exception("Expense not found");

        if (expense.Status != ExpenseStatus.Pending)
            throw new Exception("Only pending expenses can be approved");

        expense.Status = ExpenseStatus.Approved;
        expense.ApprovedAt = DateTime.UtcNow;
        expense.ApprovedByUserId = adminId;

        await _expenseRepository.UpdateAsync(expense);
        await _unitOfWork.SaveChangesAsync();

        // Trigger payment simulation
        await TriggerPaymentSimulationAsync(expenseId, expense.UserId);

        return expense;
    }

    public async Task<IEnumerable<Expense>> GetActiveExpensesByUserAsync(Guid userId)
    {
        return await _expenseRepository.GetActiveExpenseSAsync(userId);
    }

    public async Task<IEnumerable<Expense>> GetAllPendingExpensesAsync()
    {
        return await _expenseRepository.GetExpensesByStatusAsync(ExpenseStatus.Pending);
    }

    public async Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(Guid userId)
    {
        return await _expenseRepository.WhereAsync(e => e.UserId == userId && e.IsActive);
    }

    public async Task<IEnumerable<Expense>> GetExpensesHistoryByUserAsync(Guid userId)
    {
        return await _expenseRepository.GetExpenseHistoryAsync(userId);
    }

    public async Task<IEnumerable<Expense>> GetFilteredExpensesAsync(
        Guid? userId = null,
        ExpenseStatus? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        decimal? minAmount = null,
        decimal? maxAmount = null)
    {
        Expression<Func<Expense, bool>> filter = e => e.IsActive;

        if (userId.HasValue)
            filter = filter.And(e => e.UserId == userId.Value);

        if (status.HasValue)
            filter = filter.And(e => e.Status == status.Value);

        if (fromDate.HasValue)
            filter = filter.And(e => e.InsertedDate >= fromDate.Value);

        if (toDate.HasValue)
            filter = filter.And(e => e.InsertedDate <= toDate.Value);

        if (minAmount.HasValue)
            filter = filter.And(e => e.Amount >= minAmount.Value);

        if (maxAmount.HasValue)
            filter = filter.And(e => e.Amount <= maxAmount.Value);

        return await _expenseRepository.WhereAsync(filter);
    }

    public async Task<IEnumerable<Expense>> GetFilteredExpensesForUserAsync(
        Guid userId,
        ExpenseStatus? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        decimal? minAmount = null,
        decimal? maxAmount = null)
    {
        Expression<Func<Expense, bool>> filter = e => e.IsActive && e.UserId == userId;

        if (status.HasValue)
            filter = filter.And(e => e.Status == status.Value);

        if (fromDate.HasValue)
            filter = filter.And(e => e.InsertedDate >= fromDate.Value);

        if (toDate.HasValue)
            filter = filter.And(e => e.InsertedDate <= toDate.Value);

        if (minAmount.HasValue)
            filter = filter.And(e => e.Amount >= minAmount.Value);

        if (maxAmount.HasValue)
            filter = filter.And(e => e.Amount <= maxAmount.Value);

        return await _expenseRepository.WhereAsync(filter);
    }

    public async Task<IEnumerable<Expense>> GetRejectedExpensesWithReasonAsync(Guid userId)
    {
        return await _expenseRepository.WhereAsync(e =>
            e.UserId == userId &&
            e.Status == ExpenseStatus.Rejected &&
            e.IsActive);
    }

    public async Task<Expense> RejectExpenseAsync(Guid expenseId, Guid adminId, string rejectionReason)
    {
        var expense = await _expenseRepository.GetByIdAsync(expenseId);
        if (expense == null)
            throw new Exception("Expense not found");

        if (expense.Status != ExpenseStatus.Pending)
            throw new Exception("Only pending expenses can be rejected");

        expense.Status = ExpenseStatus.Rejected;
        expense.RejectionReason = rejectionReason;
        expense.ApprovedByUserId = adminId;

        await _expenseRepository.UpdateAsync(expense);
        await _unitOfWork.SaveChangesAsync();

        return expense;
    }

    public async Task<bool> TriggerPaymentSimulationAsync(Guid expenseId, Guid userId)
    {
        var expense = await _expenseRepository.GetByIdAsync(expenseId);
        if (expense == null)
            throw new Exception("Expense not found");

        if (expense.Status != ExpenseStatus.Approved)
            throw new Exception("Only approved expenses can trigger payment");

        // ödeme sistemi simülasyonuu
        var payment = await _paymentService.ProcessPaymentAsync(expense);
        return payment != null;
    }
}

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));
        var leftVisitor = new ReplaceParameterVisitor(expr1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expr1.Body);
        var rightVisitor = new ReplaceParameterVisitor(expr2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expr2.Body);
        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(left, right), parameter);
    }

    private class ReplaceParameterVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _old;
        private readonly ParameterExpression _new;

        public ReplaceParameterVisitor(ParameterExpression old, ParameterExpression @new)
        {
            _old = old;
            _new = @new;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return ReferenceEquals(node, _old) ? _new : base.VisitParameter(node);
        }
    }
}
