using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.DTOs.Requests;

public class RejectExpenseRequest
{
    [Required(ErrorMessage = "Red sebebi boş olamaz.")]
    [MinLength(5, ErrorMessage = "Red sebebi en az 5 karakter olmalıdır.")]
    public string Reason { get; set; }
}
