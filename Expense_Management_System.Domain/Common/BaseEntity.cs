using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public string InsertedUser { get; set; }
    public DateTime InsertedDate { get; set; }
    public string? UpdatedUser { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; }
}
