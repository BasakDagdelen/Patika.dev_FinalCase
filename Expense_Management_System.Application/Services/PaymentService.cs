using AutoMapper;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Enums;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Expense_Management_System.Domain.Interfaces.UnitOfWorks;


namespace Expense_Management_System.Application.Services;

public class PaymentService : GenericService<Payment>, IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public PaymentService(IPaymentRepository paymentRepository, IUserRepository userRepository, IUnitOfWork unitOfWork) : base(paymentRepository, unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Payment> GetPaymentByExpenseIdAsync(Guid expenseId)
    {
        return await _paymentRepository.GetPaymentByExpenseIdAsync(expenseId);
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(Guid userId)
    {
        return await _paymentRepository.GetPaymentsByUserAsync(userId);
    }

    public async Task<Payment> ProcessPaymentAsync(Expense expense)
    {
        var user = await _userRepository.GetByIdAsync(expense.UserId);
        if (user is null)
            throw new Exception("User not found for the expense");

        if (expense.Status != ExpenseStatus.Approved)
            throw new Exception("Only approved expenses can be paid");

        var isPaymentSuccessful = SimulateBankTransfer();
        var payment = new Payment
        {
            UserId = user.Id,
            ExpenseId = expense.Id,
            Amount = expense.Amount,
            BankAccountNumber = GenerateFakeBankAccount(user),
            IsSuccessful = isPaymentSuccessful,
            FailureReason = isPaymentSuccessful ? null : "Bank payment error",
            PaymentDate = DateTime.Now,
            PaymentMethod = PaymentMethod.EFT,
            TransactionReference = Guid.NewGuid().ToString()
        };

        await _paymentRepository.AddAsync(payment);
        await _unitOfWork.SaveChangesAsync();

        return payment;
    }

    private bool SimulateBankTransfer()
    {

        var rnd = new Random();
        return rnd.NextDouble() < 0.9;
    }

    private string GenerateFakeBankAccount(User user)
    {
        return $"TR00{user.Id.ToString("N").Substring(0, 16)}";
    }
}