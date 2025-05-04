using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Enums;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task ChangeUserRoleAsync(Guid userId, UserRole newRole)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) 
            throw new Exception("User not found");

        user.Role = newRole;
        _context.Users.Update(user);
    }

    public async Task<IEnumerable<User>> GetAllByRoleAsync(UserRole role)
    {
        return await _context.Users.Where(u => u.Role == role).ToListAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task<User?> GetByIdWithExpensesAsync(Guid userId)
    {
        return await _context.Users.Include(u => u.Expenses).FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
}
