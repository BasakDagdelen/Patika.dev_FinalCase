using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Expense_Management_System.Infrastructure.Repositories;

public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    private readonly ApplicationDbContext _context;
    public PaymentRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Payment> GetPaymentByExpenseIdAsync(Guid expenseId)
        => await _context.Payments
        .Where(p => p.ExpenseId == expenseId && p.IsActive)
        .FirstOrDefaultAsync();

    public async Task<IEnumerable<Payment>> GetPaymentsByUserAsync(Guid userId)
        => await _context.Payments
        .Where(p => p.Expense.UserId == userId && p.IsActive)
        .OrderByDescending(p => p.PaymentDate)
        .ToListAsync();
}
