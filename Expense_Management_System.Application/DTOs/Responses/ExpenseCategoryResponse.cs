using Expense_Management_System.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.DTOs.Responses;

public class ExpenseCategoryResponse: BaseResponse
{
    public string Name { get; set; }
    public string Description { get; set; }
}
