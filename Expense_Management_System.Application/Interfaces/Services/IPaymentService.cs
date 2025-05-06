using Expense_Management_System.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.Interfaces.Services;

public interface IPaymentService : IGenericService<Payment>
{
    Task<Payment> GetPaymentByExpenseIdAsync(Guid expenseId);
    Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(Guid userId);
    //Task<Payment> ProcessPaymentAsync(Expense expense);
}