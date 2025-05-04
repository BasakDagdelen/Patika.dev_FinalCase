using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Enums;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Infrastructure.Repositories
{
    public class ExpenseDocumentRepository : GenericRepository<ExpenseDocument>, IExpenseDocumentRepository
    {
        private readonly ApplicationDbContext _context;
        public ExpenseDocumentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExpenseDocument>> GetDocumentsByExpenseIdAsync(Guid expenseId)
        {
            return await _context.ExpenseDocuments.Where(d => d.ExpenseId == expenseId).ToListAsync();

        }
        public async Task<bool> IsDocumentLinkedToApprovedExpenseAsync(Guid documentId)
        {
            return await _context.ExpenseDocuments.AnyAsync(d => d.Id == documentId && d.Expenses.Status == ExpenseStatus.Approved);
        }

        public async Task<IEnumerable<ExpenseDocument>> GetDocumentsByUserIdAsync(Guid userId)
        {
            return await _context.ExpenseDocuments.Where(d => d.Expenses.UserId == userId).ToListAsync();
        }
    }
}
