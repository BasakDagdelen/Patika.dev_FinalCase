using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Infrastructure.Repositories;

public class BankAccountRepository : GenericRepository<BankAccount>, IBankAccountRepository
{
    private readonly ApplicationDbContext _context;

    public BankAccountRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<BankAccount?> GetByUserIdAsync(Guid userId)
         =>  await _context.BankAccounts.FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive);
}
