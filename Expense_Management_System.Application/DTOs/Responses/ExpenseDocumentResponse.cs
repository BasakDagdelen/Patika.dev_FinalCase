using Expense_Management_System.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.DTOs.Responses;

public class ExpenseDocumenTEntity: BaseResponse
{
    public Guid ExpenseClaimId { get; set; }
    public string FileName { get; set; }
    public string FileUrl { get; set; }
}
