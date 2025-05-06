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

    public ExpenseService( IExpenseRepository expenseRepository,
                            IUnitOfWork unitOfWork,
                            IPaymentService paymentService) : base(expenseRepository, unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
        _paymentService = paymentService;
    }

    public async Task<Expense> ApproveExpenseAndPayAsync(Guid expenseId, Guid adminId)
    {
        var expense = await _expenseRepository.GetExpenseWithUserAndBankAccountAsync(expenseId);
        if (expense is null)
            throw new Exception("Expense not found");

        if (expense.Status != ExpenseStatus.Pending)
            throw new Exception("Only pending expenses can be approved");

        expense.Status = ExpenseStatus.Approved;
        expense.ApprovedAt = DateTime.UtcNow;
        expense.ApprovedByUserId = adminId;
      
        await _expenseRepository.UpdateAsync(expense);
        await _unitOfWork.SaveChangesAsync();

        var payment = new Payment
        {
            UserId = expense.UserId,
            ExpenseId = expense.Id,
            Amount = expense.Amount,
            BankAccountNumber = "12345678",
            IsSuccessful = true,
            FailureReason = "",
            PaymentDate = DateTime.Now,
            PaymentMethod = PaymentMethod.EFT,
            TransactionReference = Guid.NewGuid().ToString(),
            InsertedUser = adminId.ToString(),
            InsertedDate = DateTime.Now,
            IsActive = true
        };
        await _paymentService.AddAsync(payment);

        return expense;
    }

    public async Task<IEnumerable<Expense>> GetActiveExpensesByUserAsync(Guid userId)
         => await _expenseRepository.GetActiveExpenseSAsync(userId);
 

    public async Task<IEnumerable<Expense>> GetAllPendingExpensesAsync()
          => await _expenseRepository.GetExpensesByStatusAsync(ExpenseStatus.Pending);
    

    public async Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(Guid userId)
         => await _expenseRepository.WhereAsync(e => e.UserId == userId && e.IsActive);
    

    public async Task<IEnumerable<Expense>> GetExpensesHistoryByUserAsync(Guid userId)
        => await _expenseRepository.GetExpenseHistoryAsync(userId);
 

    public async Task<Expense> GetExpenseWithUserAndBankAccountAsync(Guid expenseId)
         => await _expenseRepository.GetExpenseWithUserAndBankAccountAsync(expenseId);


    public async Task<IEnumerable<Expense>> GetRejectedExpensesWithReasonAsync(Guid userId)
    {
        return await _expenseRepository.WhereAsync(e =>
                                            e.UserId == userId &&
                                            e.Status == ExpenseStatus.Rejected &&
                                            e.IsActive);
    }

    public async Task<Expense> RejectExpenseAsync(Guid expenseId, Guid adminId, string rejectionReason)
    {
        var expense = await _expenseRepository.GetExpenseWithUserAndBankAccountAsync(expenseId);
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
        if (expense is null)
            throw new Exception("Expense not found");

        if (expense.Status != ExpenseStatus.Approved)
            throw new Exception("Only approved expenses can trigger payment");

        // ödeme sistemi simülasyonuu
        var payment = await _paymentService.ProcessPaymentAsync(expense);
        return payment != null;
    }
}

