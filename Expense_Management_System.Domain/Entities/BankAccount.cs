using Expense_Management_System.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Domain.Entities;

public class BankAccount: BaseEntity
{
    public Guid UserId { get; set; }  
    public string IBAN { get; set; }
    public string AccountNumber { get; set; }
  
    public User User { get; set; }
}
