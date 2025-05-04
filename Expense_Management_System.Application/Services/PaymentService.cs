using AutoMapper;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Expense_Management_System.Domain.Interfaces.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.Services;

public class PaymentService : GenericService<Payment>, IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;
    public PaymentService(IPaymentRepository paymentRepository, IUnitOfWork unitOfWork) : base(paymentRepository, unitOfWork)
    {
        _paymentRepository = paymentRepository;
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

}