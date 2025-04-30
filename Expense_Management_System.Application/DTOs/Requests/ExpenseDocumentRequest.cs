
using Expense_Management_System.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.DTOs.Requests;

public class ExpenseDocumentRequest: BaseRequest
{
    public IFormFile File { get; set; }
}
