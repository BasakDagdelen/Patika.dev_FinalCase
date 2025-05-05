using Expense_Management_System.Application.DTOs.Requests;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Enums;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Expense_Management_System.Domain.Interfaces.UnitOfWorks;


namespace Expense_Management_System.Application.Services;

public class UserService : GenericService<User>, IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork) : base(userRepository, unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ChangeUserRoleAsync(Guid userId, UserRole newRole)
    {
        await _userRepository.ChangeUserRoleAsync(userId, newRole);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> CheckUserExistsAsync(string email)
    {
        return await _userRepository.UserExistsAsync(email);
    }

    public async Task<User> CreateUserAsync(RegisterRequest request)
    {
        var userExists = await _userRepository.GetByEmailAsync(request.Email);
        if (userExists != null)
            throw new Exception("This e-mail address is already registered.");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = hashedPassword,
            PhoneNumber = request.PhoneNumber,
            WorkPhoneNumber = request.WorkPhoneNumber,
            Address = request.Address,
            //IBAN = request.IBAN,
            //BankAccountNumber = request.BankAccountNumber,
            BankAccount = new BankAccount
            {
                IBAN = request.IBAN,
                AccountNumber = request.BankAccountNumber,
                InsertedUser = "system",
                InsertedDate = DateTime.Now,
                IsActive = true
            },
            Role = UserRole.Personnel
        };

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }

    public async Task<User?> GetUserByIdWithExpensesAsync(Guid userId)
    {
        return await _userRepository.GetByIdWithExpensesAsync(userId);
    }

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
    {
        return await _userRepository.GetAllByRoleAsync(role);
    }
}