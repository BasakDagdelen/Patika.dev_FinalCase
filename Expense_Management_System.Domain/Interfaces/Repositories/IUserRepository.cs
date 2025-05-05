using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Domain.Interfaces.Repositories;

public interface IUserRepository:IGenericRepository<User>
{
    Task<IEnumerable<User>> GetAllByRoleAsync(UserRole role);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdWithExpensesAsync(Guid userId);
    Task<bool> UserExistsAsync(string email);
    Task ChangeUserRoleAsync(Guid userId, UserRole newRole);
}
