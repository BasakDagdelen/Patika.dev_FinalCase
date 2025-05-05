using Expense_Management_System.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Domain.Interfaces.Repositories;

public interface IBankAccountRepository : IGenericRepository<BankAccount>
{
    Task<BankAccount?> GetByUserIdAsync(Guid userId);
}