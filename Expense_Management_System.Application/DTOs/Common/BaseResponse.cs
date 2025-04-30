using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.DTOs.Common;

public class BaseResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; } 
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; }
}
