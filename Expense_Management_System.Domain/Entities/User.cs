using System;
using Expense_Management_System.Domain.Common;
using Expense_Management_System.Domain.Enums;

namespace Expense_Management_System.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string? WorkPhoneNumber { get; set; }
        public string Address { get; set; }
        //public string IBAN { get; set; }
        //public string BankAccountNumber { get; set; }
        public UserRole Role { get; set; }

        public BankAccount BankAccount { get; set; }
        public ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }

    }
}

