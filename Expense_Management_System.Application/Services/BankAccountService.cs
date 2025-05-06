using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Expense_Management_System.Domain.Interfaces.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.Services;

public class BankAccountService : GenericService<BankAccount>, IBankAccountService
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUnitOfWork _unitOfWork;
    public BankAccountService(IBankAccountRepository bankAccountRepository, IUnitOfWork unitOfWork) : base(bankAccountRepository, unitOfWork)
    {
        _bankAccountRepository = bankAccountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BankAccount?> GetByUserIdAsync(Guid userId)
           => await _bankAccountRepository.GetByUserIdAsync(userId);

}
